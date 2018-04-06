﻿using Components.Aphid.Interpreter;
using Components.Aphid.Library.Net.Http;
using Components.Aphid.Parser;
using Components.Aphid.TypeSystem;
using Components.External.ConsolePlus;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Components.Aphid.Library.Net
{
    public class HttpServer
    {
        private const string _configCacheTime = "ConfigCache";

        private const string _formUrlEncoded = "application/x-www-form-urlencoded";

        private string[] _prefixes;

        private string _webRoot;

        private Encoding _encoding = Encoding.GetEncoding(1252);

        private AphidSessionManager _sessionManager = new AphidSessionManager();

        private AphidObject _globals = new AphidObject();

        private string _config = "Config.alx";

        public HttpServer(string prefix)
            : this(prefix, GetDefaultWebRoot())
        {
        }

        public HttpServer(string[] prefixes)
            : this(prefixes, GetDefaultWebRoot())
        {
        }

        public HttpServer(string prefix, string webRoot)
            : this(new[] { prefix }, webRoot)
        {
        }

        public HttpServer(string[] prefixes, string webRoot)
        {
            _prefixes = prefixes;
            _webRoot = webRoot;
        }

        private static string GetDefaultWebRoot()
        {
            return Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "WebRoot");
        }

        public void Start()
        {
            Cli.WriteInfoMessage("Web root: ~Cyan~{0}~R~", _webRoot);
            var listener = new HttpListener();

            foreach (var p in _prefixes)
            {
                listener.Prefixes.Add(p);
                Cli.WriteQueryMessage("Listening on ~Cyan~{0}~R~", p);
            }

            listener.Start();
            Cli.WriteInfoMessage("Server started");

            while (true)
            {
                var context = listener.GetContext();

                ThreadPool.QueueUserWorkItem(x =>
                {
                    Cli.WriteQueryMessage(
                        "Client ~Green~{0}~R~ requested ~Cyan~{1}~R~",
                        context.Request.RemoteEndPoint.Address,
                        context.Request.Url);

                    HandleClient(context);
                });
            }
        }

        private void HandleClient(HttpListenerContext context)
        {
            byte[] resp;

            if (!Debugger.IsAttached)
            {
                try
                {
                    resp = CreateResponse(context);
                }
                catch (FileNotFoundException)
                {
                    resp = SetError(context, 404, "404 not found: {0}", context.Request.Url);
                }
                catch (Exception e)
                {
                    resp = SetError(context, 500, "500 internal server error: {0}", e.Message);
                }
            }
            else
            {
                try
                {
                    resp = CreateResponse(context);
                }
                catch (FileNotFoundException)
                {
                    resp = SetError(context, 404, "404 not found: {0}", context.Request.Url);
                }
            }

            try
            {
                WriteResponse(context, resp);
            }
            catch (HttpListenerException e)
            {
                Cli.WriteErrorMessage("Error writing response: {0}", e.Message);
            }
        }

        private byte[] SetError(HttpListenerContext context, int error, string format, params object[] args)
        {
            context.Response.StatusCode = error;
            var errorMsg = string.Format(format, args);

            return _encoding.GetBytes(errorMsg);
        }

        byte[] CreateResponse(HttpListenerContext context)
        {
            var session = GetSession(context);
            var result = TryInterpretHandler(context, session);

            if (result == null)
            {
                var localPath = GetLocalPath(context.Request.Url);
                var code = File.ReadAllText(localPath);
                result = InterpretAphid(localPath, code, context, session);
            }

            return result;
        }

        private byte[] TryInterpretHandler(
            HttpListenerContext context,
            AphidObject session)
        {
            var handlerInterpreter = GetSessionInterpreter(session);

            lock (handlerInterpreter)
            {
                return TryInterpretHandlerUnsafe(
                    handlerInterpreter,
                    context,
                    session);
            }
        }

        private byte[] TryInterpretHandlerUnsafe(
            AphidInterpreter handlerInterpreter,
            HttpListenerContext context,
            AphidObject session)
        {
            
            var scope = new AphidObject();
            var oldLoaderPaths = handlerInterpreter.Loader.SearchPaths.ToArray();
            SetupInterpreterScope(handlerInterpreter, scope, null, context, session);

            var handler = handlerInterpreter
                .CurrentScope["http"]["handlers"]
                .GetList()
                .Select(x => x.Value)
                .Cast<dynamic>()
                .Select(x => new
                {
                    x.Callback,
                    Result = handlerInterpreter.ValueHelper.Unwrap(
                        handlerInterpreter.CallFunction(x.Predicate, scope))
                })
                .FirstOrDefault(x => x.Result is bool && (bool)x.Result);

            if (handler != null)
            {


                var callback = handler.Callback;
                //AphidFunction callback = handler.Callback.Clone();
                //callback.Body = callback.Body.ToList();

                //var include = string.Format(
                //    "#'{0}'",
                //    GetHeaderFile().FullName.Replace("\\", "\\\\"));
                
                //callback.Body.Insert(0, AphidParser.ParseExpression(include));

                return RenderResponse(
                    handlerInterpreter,
                    () => handlerInterpreter.CallFunctionWithScope(
                        callback,
                        scope,
                        scope),
                    scope,
                    context);
            }
            else
            {
                return null;
            }
        }

        private byte[] InterpretAphid(
            string codeFile,
            string code,
            HttpListenerContext context,
            AphidObject session)
        {
            var interpreter = new AphidInterpreter();
            SetupInterpreterScope(interpreter, codeFile, context, session);
            
            return RenderResponse(
                interpreter,
                () => interpreter.Interpret(code, isTextDocument: true),
                interpreter.CurrentScope,
                context);
        }

        private AphidObject CreateQueryObject(HttpListenerContext context)
        {
            return CreateQueryObject(context.Request.Url.Query);
        }

        private AphidObject CreateQueryObject(string query)
        {
            var s = HttpUtility.ParseQueryString(query);

            var table = s.Keys
                .OfType<string>()
                .ToDictionary(x => x, x => new AphidObject(s[x]));

            var obj = new AphidObject();

            foreach (var kvp in table)
            {
                obj.Add(kvp.Key, kvp.Value);
            }

            return obj;
        }

        private string GetLocalPath(Uri uri)
        {
            var relativePath = uri.LocalPath.Replace('/', '\\');

            if (relativePath.Any() && relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }

            var p = Path.GetFullPath(
                Path.Combine(
                    _webRoot,
                    relativePath));

            if (Directory.Exists(p) && !File.Exists(p))
            {
                p = Path.Combine(p, "Index.alx");
            }

            // Protect against directory traversal attacks.
            if (!p.StartsWith(_webRoot))
            {
                throw new SecurityException("Cannot read files outside of web root.");
            }

            // Just in case.
            if (Regex.IsMatch(p, @"([\\/]\.\.)|(\.\.[\\/])"))
            {
                throw new SecurityException("Invalid characters in path.");
            }

            return p;
        }

        private void WriteResponse(HttpListenerContext context, byte[] response)
        {
            var r = context.Response;

            using (r.OutputStream)
            {
                if (r.ContentType == null)
                {
                    r.ContentType = "text/html";
                }

                r.ContentLength64 = response.Length;
                r.OutputStream.Write(response, 0, response.Length);
            }

            Cli.WriteQueryMessage(
                "Client ~Green~{0}~R~ disconnected",
                context.Request.RemoteEndPoint.Address);
        }

        private AphidObject GetSession(HttpListenerContext context)
        {
            var cookie = _sessionManager.GetCookie(context);
            AphidObject session = null;

            if (cookie != null)
            {
                session = _sessionManager.GetSession(cookie.Value);
            }

            if (cookie == null || session == null)
            {
                cookie = _sessionManager.CreateCookie();
                context.Response.SetCookie(cookie);
                session = _sessionManager.CreateSession(cookie.Value);
            }

            HandleConfigCaching(session);

            return session;
        }

        private void HandleConfigCaching(AphidObject session)
        {
            AphidObject lastWriteObj;
            
            if (!session.TryGetValue(_configCacheTime, out lastWriteObj))
            {
                session.Add(
                    _configCacheTime,
                    lastWriteObj = new AphidObject(DateTime.MinValue));
            }

            var lastWrite = (DateTime)lastWriteObj.Value;
            var config = GetConfigFile();

            //if (config.LastWriteTime == lastWrite)
            //{
            //    return;
            //}

            var interpreter = new AphidInterpreter();
            interpreter.InterpretFile(config.FullName);
            SetSessionInterpreter(session, interpreter);
            session[_configCacheTime].Value = config.LastWriteTime;
        }

        private void CacheConfig(AphidObject session)
        {
            session[_configCacheTime] = new AphidObject(DateTime.Now);
        }

        private void SetSessionInterpreter(
            AphidObject session,
            AphidInterpreter interpreter)
        {
            var obj = new AphidObject(interpreter);

            if (session.ContainsKey(AphidName.Interpreter))
            {
                session[AphidName.Interpreter] = obj;
            }
            else
            {
                session.Add(AphidName.Interpreter, obj);
            }
        }

        private AphidInterpreter GetSessionInterpreter(AphidObject session)
        {
            return (AphidInterpreter)session[AphidName.Interpreter].Value;
        }

        private FileInfo GetConfigFile()
        {
            return GetWebRootFile("Config.alx");
        }

        private FileInfo GetHeaderFile()
        {
            return GetWebRootFile("HttpServer.alx");
        }

        private FileInfo GetWebRootFile(string name)
        {
            return new FileInfo(Path.Combine(_webRoot, name));
        }

        private void SetupInterpreterScope(
            AphidInterpreter interpreter,
            string codeFile,
            HttpListenerContext context,
            AphidObject session)
        {
            SetupInterpreterScope(interpreter, interpreter.CurrentScope, codeFile, context, session);
        }

        private void SetupInterpreterScope(
            AphidInterpreter interpreter,
            AphidObject scope,
            string codeFile,
            HttpListenerContext context,
            AphidObject session)
        {
            string body = "";

            if (context.Request.ContentLength64 > 0)
            {
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    body = reader.ReadToEnd();
                }
            }

            scope.Add("url", new AphidObject(context.Request.Url));
            scope.Add("request", new AphidObject(context.Request));
            scope.Add("response", new AphidObject(context.Response));
            scope.Add("context", new AphidObject(context));
            scope.Add("query", CreateQueryObject(context));
            scope.Add("session", session);
            scope.Add("body", new AphidObject(body));
            scope.Add("globals", _globals);
            scope.Add("content", new AphidObject(null));
            scope.Add("contentType", new AphidObject(null));

            scope.Add(
                "post",
                context.Request.ContentType != null &&
                context.Request.ContentType.StartsWith(_formUrlEncoded) ?
                    CreateQueryObject(body) :
                    new AphidObject());

            if (codeFile != null)
            {
                interpreter.Loader.SearchPaths.Add(Path.GetDirectoryName(codeFile));
            }
        }

        private byte[] RenderResponse(
            AphidInterpreter interpreter,
            Action renderAction,
            AphidObject scope,
            HttpListenerContext context)
        {
            interpreter.GatorEmitFilter = WebUtility.HtmlEncode;

            string scriptOut;

            using (interpreter.Out = new StringWriter())
            {
                renderAction();                
                interpreter.Out.Flush();

                scriptOut = interpreter.Out.ToString();
            }

            var contentType = scope["contentType"];

            if (contentType.Value != null)
            {
                context.Response.ContentType = contentType.Value.ToString();
            }

            var content = scope["content"];

            if (content.Value == null)
            {
                return _encoding.GetBytes(scriptOut);
            }
            else
            {
                byte[] buffer;
                string text;

                if ((buffer = content.Value as byte[]) != null)
                {
                    return buffer;
                }
                else if ((text = content.Value as string) != null)
                {
                    return _encoding.GetBytes(text);
                }
                else
                {
                    return SetError(
                        context,
                        500,
                        "500 internal server error: unsupported Aphid response type: {0}",
                        content.Value.GetType().FullName);
                }
            }
        }
    }
}
