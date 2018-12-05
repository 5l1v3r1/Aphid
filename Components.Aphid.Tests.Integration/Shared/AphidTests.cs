﻿using Components.Aphid.Interpreter;
using Components.Aphid.Lexer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Dynamic;
using Components.Aphid.Parser;
using Components.Aphid.TypeSystem;
using System.Collections;
using Components.External.ConsolePlus;

namespace Components.Aphid.Tests.Integration.Shared
{
    [Parallelizable(ParallelScope.All)]
    public class AphidTests : IDisposable
    {
        private ThreadLocal<AphidInterpreter> _nextInterpreter = new ThreadLocal<AphidInterpreter>();

        private static object _cachedStdNodesSync = new object();

        private static List<AphidExpression> _cachedStdNodes;

        private static AphidExpression _cachedUsing = new UnaryOperatorExpression(
            AphidTokenType.usingKeyword,
            new IdentifierExpression("System"));

        private static List<AphidExpression> _cachedUsingBlock = new List<AphidExpression>
        {
            new UnaryOperatorExpression(
                AphidTokenType.usingKeyword,
                new IdentifierExpression("System"))
        };

        private static List<AphidExpression> _cachedUsingIncludeBlock = new List<AphidExpression>
        {
            new UnaryOperatorExpression(
                AphidTokenType.usingKeyword,
                new IdentifierExpression("System")),
            new UnaryOperatorExpression(
                AphidTokenType.LoadScriptOperator,
                new StringExpression("'Std.alx'")),
        };

        protected virtual bool LoadStd { get { return false; } }

        private static TextWriterTraceListener _listener = new TextWriterTraceListener(@"Tests.log");

        static AphidTests() => Initialize();
        public AphidTests() => Initialize();

        [Conditional("TRACE_SCRIPTED_TESTS")]
        protected static void Initialize()
        {
            if (!Trace.Listeners.Contains(_listener))
            {
                Trace.Listeners.Add(_listener);
                Trace.AutoFlush = true;
                Cli.UseTrace = true;
                WriteInfoMessage("Initialize called");
            }
        }

        [Conditional("TRACE_SCRIPTED_TESTS")]
        protected static void WriteInfoMessage(string message) => Cli.WriteInfoMessage(message);

        [Conditional("TRACE_SCRIPTED_TESTS")]
        protected static void WriteQueryMessage(string message) => Cli.WriteQueryMessage(message);

        [Conditional("TRACE_SCRIPTED_TESTS")]
        protected static void WriteSuccessMessage(string message) => Cli.WriteSuccessMessage(message);

        protected virtual List<AphidExpression> ParseScript(AphidInterpreter interpreter, string script)
        {
            List<AphidExpression> initNodes = null;
            
            if (LoadStd)
            {
                lock (_cachedStdNodesSync)
                {
                    if (_cachedStdNodes == null)
                    {
                        var stdFile = interpreter.Loader.FindScriptFile(null, "Std.alx");
                        _cachedStdNodes = PreprocessAst(AphidParser.ParseFile(stdFile));
                        _cachedStdNodes.Add(_cachedUsing);
                    }

                    initNodes = _cachedStdNodes;
                }
            }
            else
            {
                initNodes = _cachedUsingBlock;
            }

            return initNodes.Concat(PreprocessAst(AphidParser.Parse(script))).ToList();
        }

        private List<AphidExpression> PreprocessAst(List<AphidExpression> ast)
        {
            var stage1 = new PartialOperatorMutator().MutateRecursively(ast);
            var stage2 = new AphidMacroMutator().MutateRecursively(stage1);
            var stage3 = new AphidPreprocessorDirectiveMutator().MutateRecursively(stage2);

            return stage3;
        }

        protected AphidInterpreter GetNextInterpreter()
        {
            lock (_nextInterpreter)
            {
                return _nextInterpreter.Value != null ?
                    _nextInterpreter.Value :
                    _nextInterpreter.Value = new AphidInterpreter();
            }
        }

        protected AphidObject Execute(string script)
        {
            AphidInterpreter interpreter;

            lock (_nextInterpreter)
            {
                interpreter = _nextInterpreter.Value ?? new AphidInterpreter();
                _nextInterpreter.Value = null;
            }

            interpreter.Loader.SearchPaths.Add(Path.Combine(Environment.CurrentDirectory, "Library"));
            interpreter.Interpret(ParseScript(interpreter, script));
            return interpreter.GetReturnValue();
        }

        protected void Execute(string script, Action<AphidObject> action)
        {
            action(Execute(script));
        }

        protected void Is(string script, Func<AphidObject, bool> predicate)
        {
            Assert.IsTrue(predicate(Execute(script)));
        }

        protected void Is<TValue>(string script, Func<TValue, bool> predicate)
        {
            Assert.IsTrue(predicate((TValue)Execute(script).Value));
        }

        protected List<AphidToken> GetTokens(string script)
        {
            return new AphidLexer(script).GetAllTokens();
        }

        protected void AssertTokens(string script, AphidToken[] tokens)
        {
            var tokens2 = GetTokens(script);
            Assert.AreEqual(tokens.Length, tokens2.Count);

            for (int i = 0; i < tokens.Length; i++)
            {
                Assert.AreEqual(tokens[i].TokenType, tokens2[i].TokenType);

                if (tokens[i].Lexeme != null)
                {
                    Assert.AreEqual(tokens[i].Lexeme, tokens2[i].Lexeme);
                }
            }
        }

        protected AphidToken Token(AphidTokenType type, string lexeme)
        {
            return new AphidToken(type, lexeme, 0);
        }

        protected AphidToken Token(AphidTokenType type)
        {
            return Token(type, null);
        }

        public static void IsFalse(bool value)
        {
            Assert.IsFalse(value);
        }

        public static void IsTrue(bool value)
        {
            Assert.IsTrue(value);
        }

        public static void IsFoo(object value)
        {
            Assert.AreEqual("foo", value);
        }

        public static void Is9(object value)
        {
            Assert.AreEqual(9m, value);
        }

        public static void IsNull(object value)
        {
            Assert.IsNull(value);
        }

        public static void NotNull(object value)
        {
            Assert.NotNull(value);
        }

        public static void IsFail(Action action)
        {
            IsThrow<AssertionException>(action);
        }

        public static void AllFail(params Action[] actions)
        {
            CollectionAssert.IsNotEmpty(actions);
            actions.Iter(IsFail);
        }

        public static Exception IsThrow(Action action)
        {
            return Assert.Catch(() => action());
        }

        public static Exception[] AllThrow(params Action[] actions)
        {
            CollectionAssert.IsNotEmpty(actions);

            return actions.Select(IsThrow).ToArray();
        }

        public static TException IsThrow<TException>(Action action)
            where TException : Exception
        {
            return Assert.Catch<TException>(() => action());
        }

        public static TException[] AllThrow<TException>(
            params Action[] actions)
            where TException : Exception
        {
            CollectionAssert.IsNotEmpty(actions);

            return actions.Select(IsThrow<TException>).ToArray();
        }

        protected void AssertEquals(object expected, string script)
        {
            Assert.AreEqual(expected, Execute(script).Value);
        }

        protected void AssertFoo(string script)
        {
            AssertEquals("foo", script);
        }

        protected void Assert9(string script)
        {
            AssertEquals(9m, script);
        }

        protected void AssertTrue(string script)
        {
            AssertEquals(true, script);
        }

        protected void AssertFalse(string script)
        {
            AssertEquals(false, script);
        }

        private string CreateStatement(string expression)
        {
            return string.Format("ret {0};", expression);
        }

        protected void AssertExpEquals(object expected, string expression)
        {
            AssertEquals(expected, CreateStatement(expression));
        }

        protected void AssertExpFoo(string expression)
        {
            AssertFoo(CreateStatement(expression));
        }

        protected void AssertExp9(string expression)
        {
            Assert9(CreateStatement(expression));
        }

        protected void AssertExpTrue(string expression)
        {
            AssertTrue(CreateStatement(expression));
        }

        protected void AssertExpFalse(string expression)
        {
            AssertFalse(CreateStatement(expression));
        }

        protected void AssertCollectionIs<TElement>(string script, params TElement[] expected)
        {
            CollectionAssert.AreEqual(expected, (IEnumerable)Execute(script).Value);
        }

        public virtual void Dispose()
        {
            WriteSuccessMessage("Dispose called");
            Trace.Listeners.Remove(_listener);

            using (_listener)
            {
                _listener = null;
            }
        }
    }
}
