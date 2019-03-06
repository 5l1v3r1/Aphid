﻿using Components.Aphid.Interpreter;
using Components.Aphid.Lexer;
using Components.Aphid.Parser;
using Components.Aphid.Serialization;
using Components.Aphid.TypeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace VSCodeDebug
{
    public class AphidDebugSession : DebugSession
    {
        private readonly string[] _fileExtensions = new [] { ".alx" };

        private const int _maxChildren = 10000,
            _maxConnectionAttempts = 10,
            _connectionAttemptInterval = 500;

        private AutoResetEvent _resumeEvent = new AutoResetEvent(false);
        private bool _debuggeeExecuting;
        private readonly object _lock = new object();

        private volatile bool _debuggeeKilled = true;
        private SortedDictionary<long, AphidExpression> _breakpoints;

        private System.Diagnostics.Process _process;
        private Handles<KeyValuePair<string, AphidObject>[]> _variableHandles;
        private Dictionary<AphidObject, string> _scopeNames = new Dictionary<AphidObject, string>();
        private Handles<StackFrame> _frameHandles;
        private Dictionary<int, AphidFrame[]> _aphidFrameMap = new Dictionary<int, AphidFrame[]>();
        private Dictionary<int, AphidObject> _frameScopes = new Dictionary<int, AphidObject>();
        private AphidObject _exception;
        private readonly Dictionary<int, Thread> _seenThreads = new Dictionary<int, Thread>();
        private bool _attachMode;
        private string _script;
        private int _threadId;
        private string _code;
        private List<AphidExpression> _ast;
        private bool _isRunning;

        public AphidInterpreter Interpreter { get; } = new AphidInterpreter();

        public AphidDebugSession()
            : base()
        {
            _variableHandles = new Handles<KeyValuePair<string, AphidObject>[]>();
            _frameHandles = new Handles<StackFrame>();
            _seenThreads = new Dictionary<int, Thread>();
            Interpreter.CurrentScope.Add("$debugger", AphidObject.Scalar(this));
            Interpreter.HandleExecutionBreak = HandleBreak;
        }

        private void HandleBreak(AphidExpression expression)
        {
            Program.Log("Execution break on {0}", expression.ToString());
            Stopped();
            Program.Log("Stopped on {0}", expression.ToString());

            SendEvent(
                new StoppedEvent(
                    _threadId,
                    "breakpoint",
                    expression.ToString()));

            _resumeEvent.Set();
        }

        public override void Initialize(Response response, dynamic arguments)
        {
            var os = Environment.OSVersion;

            if (os.Platform != PlatformID.MacOSX &&
                os.Platform != PlatformID.Unix &&
                os.Platform != PlatformID.Win32NT)
            {
                SendErrorResponse(response, 3000, "Aphid is not supported on this platform ({_platform}).", new { _platform = os.Platform.ToString() }, true, true);
                return;
            }

            SendResponse(response, new Capabilities()
            {
                // This debug adapter does not need the configurationDoneRequest.
                supportsConfigurationDoneRequest = false,

                // This debug adapter does not support function breakpoints.
                supportsFunctionBreakpoints = true,

                // This debug adapter doesn't support conditional breakpoints.
                supportsConditionalBreakpoints = true,

                // This debug adapter does not support a side effect free evaluate request for data hovers.
                supportsEvaluateForHovers = true,

                // This debug adapter does not support exception breakpoint filters
                exceptionBreakpointFilters = new dynamic[0]
            });

            // Mono Debug is ready to accept breakpoints immediately
            SendEvent(new InitializedEvent());
        }

        public List<AphidExpression> Parse(Response response, string file)
        {
            var code = File.ReadAllText(file);

            try
            {
                return MutatorGroups.GetStandard().Mutate(AphidParser.Parse(code, file));
            }
            catch (AphidParserException e)
            {
                SendErrorResponse(
                    response,
                    0x523000,
                    ParserErrorMessage.Create(code, e, highlight: false));

                return null;
            }
        }

        public override async void Launch(Response response, dynamic arguments)
        {
            _attachMode = false;

            //SetExceptionBreakpoints(args.__exceptionOptions);

            // validate argument 'program'
            string programPath = getString(arguments, "program");
            if (programPath == null)
            {
                SendErrorResponse(response, 3001, "Property 'program' is missing or empty.", null);
                return;
            }

            //Program.Log("Program path: {0}\r\n", programPath);

            programPath = ConvertClientPathToDebugger(programPath);
            if (!File.Exists(programPath) && !Directory.Exists(programPath))
            {
                SendErrorResponse(response, 3002, "Program '{path}' does not exist.", new { path = programPath });
                return;
            }

            // validate argument 'cwd'
            var workingDirectory = (string)arguments.cwd;
            if (workingDirectory != null)
            {
                workingDirectory = workingDirectory.Trim();
                if (workingDirectory.Length == 0)
                {
                    SendErrorResponse(response, 3003, "Property 'cwd' is empty.");
                    return;
                }
                workingDirectory = ConvertClientPathToDebugger(workingDirectory);
                if (!Directory.Exists(workingDirectory))
                {
                    SendErrorResponse(response, 3004, "Working directory '{path}' does not exist.", new { path = workingDirectory });
                    return;
                }
            }

            // Todo: handle case insensitive file systems and forward slashes
            _script = Path.GetFullPath(programPath).Replace('/', '\\');
            _code = File.ReadAllText(programPath);

            if ((_ast = Parse(response, _script)) == null)
            {
                return;
            }

            const string host = "127.0.0.1";
            var port = Utilities.FindFreePort(55555);
            bool debug = !getBool(arguments, "noDebug", false);

            if (debug)
            {
                Connect(IPAddress.Parse(host), port);
            }

            var termArgs = new
            {
                kind = "external",
                title = "Aphid Debug Console",
                cwd = workingDirectory,
                args = programPath,
                //env = environmentVariables
            };

            var resp = await SendRequest("runInTerminal", termArgs);

            SendResponse(response);
        }

        public override void Attach(Response response, dynamic arguments)
        {
            _attachMode = true;

            var host = getString(arguments, "address");
            if (host == null)
            {
                SendErrorResponse(response, 3007, "Property 'address' is missing or empty.");
                return;
            }

            // validate argument 'port'
            var port = getInt(arguments, "port", -1);
            if (port == -1)
            {
                SendErrorResponse(response, 3008, "Property 'port' is missing.");
                return;
            }

            IPAddress address = Utilities.ResolveIPAddress(host);
            if (address == null)
            {
                SendErrorResponse(response, 3013, "Invalid address '{address}'.", new { address = address });
                return;
            }

            Connect(address, port);

            SendResponse(response);
        }

        public override void Disconnect(Response response, dynamic arguments)
        {
            if (_attachMode)
            {
                lock (_lock)
                {
                    //if (_session != null) {
                    _debuggeeExecuting = true;
                    _breakpoints.Clear();
                    //_session.Breakpoints.Clear();
                    //_session.Continue();
                    //_session = null;
                    //}
                }
            }
            else
            {
                // Let's not leave dead Mono processes behind...
                if (_process != null)
                {
                    _process.Kill();
                    _process = null;
                }
                else
                {
                    //PauseDebugger();
                    DebuggerKill();

                    while (!_debuggeeKilled)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }

            SendResponse(response);
        }

        public override void Continue(Response response, dynamic arguments)
        {
            WaitForSuspend();
            SendResponse(response);
            Interpreter.Continue();
            //Program.Log("Continuing execution");

            _debuggeeExecuting = true;
            //lock (_lock) {
            //    if (_session != null && !_session.IsRunning && !_session.HasExited) {
            //        _session.Continue();
            //        _debuggeeExecuting = true;
            //    }
            //}
        }

        public override void Next(Response response, dynamic arguments)
        {
            WaitForSuspend();
            SendResponse(response);
            Interpreter.SingleStep();
            _debuggeeExecuting = true;
            //lock (_lock) {
            //    if (_session != null && !_session.IsRunning && !_session.HasExited) {
            //        _session.NextLine();
            //        _debuggeeExecuting = true;
            //    }
            //}
        }

        public override void StepIn(Response response, dynamic arguments)
        {
            WaitForSuspend();
            SendResponse(response);
            Interpreter.SingleStep();
            _debuggeeExecuting = true;
            //         lock (_lock) {
            //    if (_session != null && !_session.IsRunning && !_session.HasExited) {
            //        _session.StepLine();
            //        _debuggeeExecuting = true;
            //    }
            //}
        }

        public override void StepOut(Response response, dynamic arguments)
        {
            WaitForSuspend();
            SendResponse(response);
            Interpreter.SingleStep();
            _debuggeeExecuting = true;
            //         lock (_lock) {
            //    if (_session != null && !_session.IsRunning && !_session.HasExited) {
            //        _session.Finish();
            //        _debuggeeExecuting = true;
            //    }
            //}
        }

        public override void Pause(Response response, dynamic arguments)
        {
            SendResponse(response);
            PauseDebugger();
        }

        public override void SetExceptionBreakpoints(Response response, dynamic arguments) => SendResponse(response);

        public override void SetBreakpoints(Response response, dynamic arguments)
        {
            string path = null;
            if (arguments.source != null)
            {
                var p = (string)arguments.source.path;
                if (p?.Trim().Length > 0)
                {
                    path = p;
                }
            }
            if (path == null)
            {
                SendErrorResponse(response, 3010, "setBreakpoints: property 'source' is empty or misformed", null, false, true);
                return;
            }
            path = ConvertClientPathToDebugger(path);

            if (!HasMonoExtension(path))
            {
                // we only support breakpoints in files mono can handle
                SendResponse(response, new SetBreakpointsResponseBody());
                return;
            }

            var clientLines = (int[])arguments.lines.ToObject<int[]>();

            var bps = new AphidBreakpointController().UpdateBreakpoints(
                this,
                response,
                path,
                clientLines);

            if (bps == null)
            {
                return;
            }

            SendResponse(response, new SetBreakpointsResponseBody(bps));

            //         HashSet<int> lin = new HashSet<int>();
            //for (int i = 0; i < clientLines.Length; i++) {
            //    lin.Add(ConvertClientLineToDebugger(clientLines[i]));
            //}

            // find all breakpoints for the given path and remember their id and line number
            //var bpts = new List<Tuple<int, int>>();
            //foreach (var be in _breakpoints) {
            //    var bp = be.Value as Mono.Debugging.Client.Breakpoint;
            //    if (bp != null && bp.FileName == path) {
            //        bpts.Add(new Tuple<int,int>((int)be.Key, (int)bp.Line));
            //    }
            //}

            //HashSet<int> lin2 = new HashSet<int>();
            //foreach (var bpt in bpts) {
            //    if (lin.Contains(bpt.Item2)) {
            //        lin2.Add(bpt.Item2);
            //    }
            //    else {
            //        // Program.Log("cleared bpt #{0} for line {1}", bpt.Item1, bpt.Item2);

            //        BreakEvent b;
            //        if (_breakpoints.TryGetValue(bpt.Item1, out b)) {
            //            _breakpoints.Remove(bpt.Item1);
            //            _session.Breakpoints.Remove(b);
            //        }
            //    }
            //}

            //for (int i = 0; i < clientLines.Length; i++) {
            //    var l = ConvertClientLineToDebugger(clientLines[i]);
            //    if (!lin2.Contains(l)) {
            //        var id = _nextBreakpointId++;
            //        _breakpoints.Add(id, _session.Breakpoints.Add(path, l));
            //        // Program.Log("added bpt #{0} for line {1}", id, l);
            //    }
            //}

            //var breakpoints = new List<Breakpoint>();
            //foreach (var l in clientLines) {
            //    breakpoints.Add(new Breakpoint(true, l));
            //}

            //SendResponse(response, new SetBreakpointsResponseBody(breakpoints));
        }

        public override void StackTrace(Response response, dynamic arguments)
        {
            //int maxLevels = getInt(args, "levels", 10);
            //int threadReference = getInt(args, "threadId", 0);

            //WaitForSuspend();

            //ThreadInfo thread = DebuggerActiveThread();
            //if (thread.Id != threadReference) {
            //    // Program.Log("stackTrace: unexpected: active thread should be the one requested");
            //    thread = FindThread(threadReference);
            //    if (thread != null) {
            //        thread.SetActive();
            //    }
            //}

            //var stackFrames = new List<StackFrame>();
            //int totalFrames = 0;

            //var bt = thread.Backtrace;
            //if (bt != null && bt.FrameCount >= 0) {

            //    totalFrames = bt.FrameCount;

            //    for (var i = 0; i < Math.Min(totalFrames, maxLevels); i++) {

            //        var frame = bt.GetFrame(i);

            //        string path = frame.SourceLocation.FileName;

            //        var hint = "subtle";
            //        Source source = null;
            //        if (!string.IsNullOrEmpty(path)) {
            //            string sourceName = Path.GetFileName(path);
            //            if (!string.IsNullOrEmpty(sourceName)) {
            //                if (File.Exists(path)) {
            //                    source = new Source(sourceName, ConvertDebuggerPathToClient(path), 0, "normal");
            //                    hint = "normal";
            //                } else {
            //                    source = new Source(sourceName, null, 1000, "deemphasize");
            //                }
            //            }
            //        }

            //        var frameHandle = _frameHandles.Create(frame);
            //        string name = frame.SourceLocation.MethodName;
            //        int line = frame.SourceLocation.Line;
            //        stackFrames.Add(new StackFrame(frameHandle, name, source, ConvertDebuggerLineToClient(line), 0, hint));
            //    }
            //}

            var id = 0;

            var lineResolver = new AphidLineResolver();

            StackFrame nextFrame(AphidExpression expression) =>
                new StackFrame(
                id++,
                expression.ToString(),
                GetSource(expression),
                lineResolver.ResolveExpressionLine(
                    GetAst(response, expression),
                    expression),
                0,
                "test2");

            StackFrame[] expFrames;

            if (Interpreter.CurrentExpression != null &&
                Interpreter.CurrentStatement != Interpreter.CurrentStatement)
            {
                if (Interpreter.CurrentStatement != null)
                {
                    expFrames = new[]
                    {
                        nextFrame(Interpreter.CurrentExpression),
                        nextFrame(Interpreter.CurrentStatement)
                    };
                }
                else
                {
                    expFrames = new[] { nextFrame(Interpreter.CurrentExpression) };
                }
            }
            else if (Interpreter.CurrentStatement != null)
            {
                if (Interpreter.CurrentExpression != null)
                {
                    expFrames = new[] { nextFrame(Interpreter.CurrentExpression) };
                }
                else
                {
                    expFrames = Array.Empty<StackFrame>();
                }
            }
            else
            {
                expFrames = Array.Empty<StackFrame>();
            }

            var aphidFrames = Interpreter.GetStackTrace();

            var stackFrames = expFrames
                .Concat(aphidFrames
                    .Select(x => new StackFrame(
                        id++,
                        x.Name,
                        GetSource(x.Expression),
                        lineResolver.ResolveExpressionLine(
                            GetAst(response, x.Expression),
                            x.Expression),
                        0,
                        "test2")))
                .ToList();

            var frames2 = new List<StackFrame>();
            //frames2.Add(new StackFrame(
            //    _frameHandles.Create(currentFrame),
            //    currentFrame.name,
            //    currentFrame.line,
            //    currentFrame.column,
            //    currentFrame.presentationHint

            //_frameScopes.Clear();

            var i = -1;
            foreach (var f in stackFrames)
            {
                var handle = _frameHandles.Create(f);
                var scope = ++i == 0 ?
                        Interpreter.CurrentScope :
                        aphidFrames[i - 1].Scope;

                if (!_aphidFrameMap.ContainsKey(handle))
                {
                    _aphidFrameMap.Add(handle, aphidFrames.Skip(i - 1).ToArray());

                    Program.Log(
                        "VSC Frame: {0}, Aphid Frames: {1}",
                        f.name,
                        string.Join(", ", _aphidFrameMap[handle].Select(x => x.Name)));
                }

                if (!_frameScopes.ContainsKey(handle))
                {
                    _frameScopes.Add(handle, scope);
                }

                if (!_scopeNames.ContainsKey(scope))
                {
                    _scopeNames.Add(scope, f.name);
                }

                frames2.Add(new StackFrame(handle, f.name, f.source, f.line, f.column, f.presentationHint));
            }

            SendResponse(response, new StackTraceResponseBody(frames2, frames2.Count));
        }

        private List<AphidExpression> GetAst(Response response, AphidExpression expression)
        {
            Program.Log("Getting filename from {0}", expression);
            Program.Log("Filename: {0}", expression);

            return expression?.Filename != null ?
                Parse(response, expression.Filename) :
                _ast;
        }

        private Source GetSource(AphidExpression expression)
        {
            return expression?.Filename != null ?
                new VSCodeDebug.Source(
                    Path.GetFileName(expression.Filename),
                    Path.GetFullPath(expression.Filename),
                    0,
                    "normal") :
                new VSCodeDebug.Source(
                    Path.GetFileName(_script),
                    _script,
                    0,
                    "normal");
        }

        public override void Source(Response response, dynamic arguments) => SendErrorResponse(response, 1020, "No source available");

        public override void Scopes(Response response, dynamic arguments)
        {
            int frameId = getInt(arguments, "frameId", 0);
            var frame = _frameHandles.Get(frameId, null);

            foreach (var kvp in _frameScopes)
            {
                Program.Log(
                    "Frame scope {0}: {1}",
                    kvp.Key,
                    new AphidSerializer(Interpreter).Serialize(kvp.Value));
            }

            var scopes = new List<Scope>();
            var kvps = _frameScopes[frameId].ToArray();
            _variableHandles.Create(kvps.Where(x => x.Value != null).SelectMany(x => x.Value).ToArray());
            scopes.Add(new Scope(frame.name, _variableHandles.Create(kvps)));

            var mergedScope = AphidObject.Scope();
            var aphidScopes = _frameScopes[frameId].FlattenScope();

            scopes.Add(new Scope("Locals", _variableHandles.Create(_frameScopes[frameId].FlattenScope().SelectMany(x => x).ToArray())));

            foreach (var aphidScope in aphidScopes)
            {

                //foreach (var kvp in aphidScope)
                //{
                //    if (mergedScope.ContainsKey(kvp.Key))
                //    {
                //        continue;
                //    }
                //    else
                //    {
                //        mergedScope.Add(kvp.Key, kvp.Value);
                //    }
                //}
            }

            //Program.Log("Merged scope keys: {0}", string.Join(", ", mergedScope.Keys));

            //scopes.Add(new Scope("locals", _variableHandles.Create(aphidScopes)));

            //foreach (var aphidScope in mergedScope.Where(x => x.Value != null))
            //{
            //    var handle = _variableHandles.Create(
            //        aphidScope.Value
            //            .ToArray()
            //            .Select(x => x.Value)
            //            .ToArray());

            //}

            SendResponse(response, new ScopesResponseBody(scopes));
        }

        public override void Variables(Response response, dynamic arguments)
        {
            int reference = getInt(arguments, "variablesReference", -1);
            if (reference == -1)
            {
                SendErrorResponse(response, 3009, "variables: property 'variablesReference' is missing", null, false, true);
                return;
            }

            //Program.Log("[variables] Waiting for {0}", reference);
            WaitForSuspend();
            //Program.Log("[variables] Done waiting for {0}", reference);
            var variables = new List<Variable>();

            if (_variableHandles.TryGet(reference, out var children))
            {
                Program.Log("[variables] Got {0} children for {1}", children.Length, reference);
                foreach (var c in children)
                {
                    Program.Log(
                        "[variables] Got child for {0} is {1}",
                        reference,
                        c.Value != null ? c.Value.GetType().FullName : c.ToString());

                    if (c.Value == null)
                    {
                        variables.Add(CreateVariable(c));
                    }
                    else if (c.Value.Value == null)
                    {
                        variables.Add(CreateVariable(c));
                        //c.Value.ToArray()
                        //foreach (var kvp in c.Value)
                        //{
                        //    variables.Add(CreateVariable(kvp));
                        //}
                    }
                    else if (false && c.Value.Value.GetType() == typeof(List<AphidObject>))
                    {
                        variables.Add(
                            CreateVariable(
                                new KeyValuePair<string, AphidObject>(
                                    c.Key,
                                    AphidObject.Scalar(c.Value.GetList()))));

                        //variables.Add(CreateVariable(c));
                        foreach (var o in c.Value.GetList().Select(x => x.ToArray()))
                        {
                        }
                        //variables.Add(CreateVariable(c));

                        foreach (var obj in c.Value.GetList())
                        {
                            //new KeyValuePair<string, AphidObject>(c.Key, obj.Va

                            //foreach (var kvp in obj)
                            //{
                            //    variables.Add(CreateVariable(kvp));
                            //}
                        }
                    }
                    else
                    {
                        variables.Add(CreateVariable(c));
                    }
                }

                //variables.AddRange(children
                //    .Where(x => x != null)
                //    .SelectMany(x => x
                //        .Where(y => y.Key != null)
                //        .Select(y => CreateVariable(y.Key, y.Value))));
                //.GroupBy(x => x.Key)
                //.Select(x => x.First().Key, x.First().Value)));
                //variables.AddRange(children
                //    .SelectMany(x => x.ToArray())
                //    .GroupBy(x => x.Key)
                //    .Select(x => CreateVariable(x.First().Key, x.First().Value)));
            }
            else if (false)
            {
                if (children?.Length > 0)
                {
                    var more = false;
                    if (children.Length > _maxChildren)
                    {
                        children = children.Take(_maxChildren).ToArray();
                        more = true;
                    }

                    if (children.Length < 20)
                    {
                        // Wait for all values at once.
                        //WaitHandle.WaitAll(children.Select(x => x.WaitHandle).ToArray());
                        foreach (var v in children)
                        {
                            //variables.Add(CreateVariable(v));
                        }
                    }
                    else
                    {
                        foreach (var v in children)
                        {
                            //v.WaitHandle.WaitOne();
                            //variables.Add(CreateVariable(v));
                        }
                    }

                    if (more)
                    {
                        variables.Add(new Variable("...", null, null));
                    }
                }
            }
            else
            {
                Program.Log("No variable handle created");
            }

            SendResponse(response, new VariablesResponseBody(variables));
        }

        private void HandleException(AphidInterpreter interpreter, Exception exception)
        {
            Program.Log("Unhandled exception: {0}", exception.Message);
            Stopped();

            SendEvent(
                new StoppedEvent(
                    _threadId,
                    "exception",
                    ExceptionHelper.Unwrap(exception).ToString()));

            _resumeEvent.Set();
        }

        public override void Threads(Response response, dynamic arguments)
        {
            if (!_isRunning)
            {
                using (var reset = new ManualResetEvent(false))
                {
                    new System.Threading.Thread(() =>
                    {
                        _threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                        reset.Set();
                        SendEvent(new ThreadEvent("started", _threadId));

                        var backup = Interpreter.SetIsInTryCatchFinally(true);

                        try
                        {
                            Interpreter.Interpret(_ast);
                        }
                        catch (Exception e)
                        {
                            HandleException(Interpreter, e);
                        }

                        Interpreter.SetIsInTryCatchFinally(false);
                        SendEvent(new ThreadEvent("exited", _threadId));
                        //Terminate("target exited");
                    }).Start();
                    _isRunning = true;

                    reset.WaitOne();
                    var threads2 = new List<Thread>();
                    threads2.Add(new Thread(_threadId, "Main THread"));
                    SendResponse(response, new ThreadsResponseBody(threads2));
                }

                return;
            }

            var threads = new List<Thread>();
            threads.Add(new Thread(_threadId, "Main Thread"));
            SendResponse(response, new ThreadsResponseBody(threads));
        }

        public override void Evaluate(Response response, dynamic arguments)
        {
            var expression = getString(arguments, "expression");

            var exp = AphidParser.ParseExpression(expression.ToString());

            var retExp =
                exp.Type != AphidExpressionType.UnaryOperatorExpression ||
                ((UnaryOperatorExpression)exp).Operator != AphidTokenType.retKeyword ?
                    new UnaryOperatorExpression(
                        AphidTokenType.retKeyword,
                        exp,
                        isPostfix: false)
                        .WithPositionFrom(exp) :
                    exp;

            var value = (AphidObject)new AphidInterpreter(Interpreter.CurrentScope).Interpret(exp);
            
            if (false)
            {
                SendErrorResponse(response, 3014, "Evaluate request failed, invalid expression");
            }
            else
            {
                var handle = _variableHandles.Create(value.ToArray());
                SendResponse(
                    response,
                    new EvaluateResponseBody(
                        new AphidSerializer(Interpreter).Serialize(value),
                        handle));
            }
        }

        private void Stopped()
        {
            _exception = null;
            _variableHandles.Reset();
            _frameHandles.Reset();
        }

        private static string GetObjectPreview(AphidObject obj) =>
            obj.Keys.Count > 0 ? string.Format("{{ {0} }}", string.Join(", ", obj.Keys)) : "{ [Empty] }";

        private static string GetCollectionPreview(IEnumerable collection)
        {
            var c = collection.Cast<object>().Count();
            return string.Format("[ {0:n0} element{1} ]", c, c != 1 ? "s" : "");
        }

        private Variable CreateVariable(KeyValuePair<string, AphidObject> v)
        {
            if (v.Value == null)
            {
                return new Variable(v.Key, "null", AphidType.Null);
            }
            else if (v.Value.Value == null)
            {
                return new Variable(
                    v.Key,
                    GetObjectPreview(v.Value),
                    AphidType.Object,
                    _variableHandles.Create(v.Value.ToArray()));
            }
            else
            {
                if (v.Value.Value.GetType() == typeof(List<AphidObject>))
                {
                    return new Variable(
                        v.Key,
                        GetCollectionPreview((List<AphidObject>)v.Value.Value),
                        AphidType.List,
                        _variableHandles.Create(((List<AphidObject>)v.Value.Value)
                            .SelectMany((x, i) => x.Value == null ?
                                x.ToArray() :
                                new[]
                                {
                                    new KeyValuePair<string, AphidObject>(
                                        i.ToString(),
                                        ValueHelper.Wrap(x.Value))
                                })
                            .ToArray()));
                }
                else if (v.Value.IsAphidType())
                {
                    return new Variable(
                        v.Key,
                        new AphidSerializer(Interpreter).Serialize(v.Value),
                        v.Value.GetValueType(),
                        0);
                }
                else if (v.Value.Value.GetType() == typeof(AphidObject))
                {
                    return new Variable(
                        v.Key,
                        GetObjectPreview((AphidObject)v.Value.Value),
                        AphidType.Object,
                        _variableHandles.Create(((AphidObject)v.Value.Value).ToArray()));
                }
                else if (v.Value.Value.GetType().GetInterface("IEnumerable") != null)
                {
                    var l = new List<AphidObject>();

                    foreach (var o in (IEnumerable)v.Value.Value)
                    {
                        l.Add(ValueHelper.Wrap(o));
                    }

                    return new Variable(
                        v.Key,
                        GetCollectionPreview(l),
                        "list",
                        _variableHandles.Create(l
                            .Select((x, i) => new KeyValuePair<string, AphidObject>(
                                i.ToString(),
                                x))
                            .ToArray()));
                }
                else
                {
                    var t = v.Value.Value.GetType();

                    Program.Log("Reflecting type {0}", t.FullName);

                    if (t.IsPrimitive || t.IsEnum)
                    {
                        return new Variable(v.Key, v.Value.Value.ToString(), t.FullName);
                    }

                    return new Variable(
                        v.Key,
                        t.Name,
                        t.FullName,
                        _variableHandles.Create(t
                            .GetProperties()
                            .Select((x, i) => new KeyValuePair<string, AphidObject>(
                                x.Name,
                                ValueHelper.Wrap(TryGetValue(x, v.Value.Value))))
                            .ToArray()));
                }
            }
        }

        private object TryGetValue(PropertyInfo property, object target)
        {
            try
            {
                return property.GetValue(target);
            }
            catch (Exception e)
            {
                Program.Log(e.ToString());
                return e.Message;
            }
        }

        private bool HasMonoExtension(string path)
        {
            foreach (var e in _fileExtensions)
            {
                if (path.EndsWith(e))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool getBool(dynamic container, string propertyName, bool dflt = false)
        {
            try
            {
                return (bool)container[propertyName];
            }
            catch (Exception)
            {
                // ignore and return default value
            }
            return dflt;
        }

        private static int getInt(dynamic container, string propertyName, int dflt = 0)
        {
            try
            {
                return (int)container[propertyName];
            }
            catch (Exception)
            {
                // ignore and return default value
            }
            return dflt;
        }

        private static string getString(dynamic args, string property, string dflt = null)
        {
            var s = (string)args[property];
            if (s == null)
            {
                return dflt;
            }
            s = s.Trim();
            if (s.Length == 0)
            {
                return dflt;
            }
            return s;
        }

        //-----------------------

        private void WaitForSuspend()
        {
            if (_debuggeeExecuting)
            {
                Program.Log("[i] Debuggee waiting for resume event");
                _resumeEvent.WaitOne();
                Program.Log("[+] Debuggee resumeevent set");
                _debuggeeExecuting = false;
            }
            else
            {
                //Program.Log("[!] Debuggee not executing");
            }
        }

        private void Connect(IPAddress address, int port)
        {
            lock (_lock)
            {
                _debuggeeKilled = false;

                _debuggeeExecuting = true;
            }
        }

        private void PauseDebugger() => Interpreter.Pause();

        private void DebuggerKill() => Environment.Exit(0);
    }
}
