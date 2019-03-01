using Components.Aphid.Lexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.Parser
{
    public partial class AphidParser
    {
        private List<AphidToken> _tokens;

        private int _tokenIndex = -1;

        private AphidToken _currentToken;

        public AphidToken CurrentToken => _currentToken;

        public bool UseImplicitReturns { get; set; }

        public AphidParser(List<AphidToken> tokens)
        {
            UseImplicitReturns = true;
            _tokens = tokens;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public bool Match(AphidTokenType tokenType)
        {
            if (_currentToken.TokenType == tokenType)
            {
                NextToken();
                return true;
            }

            throw new AphidParserException(_currentToken, tokenType);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public bool NextToken()
        {
            _tokenIndex++;

            if (_tokenIndex < _tokens.Count)
            {
                _currentToken = _tokens[_tokenIndex];
                return true;
            }

            _currentToken = AphidToken.GetNone(_currentToken.Index + _currentToken.Lexeme.Length);

            return false;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public bool PreviousToken()
        {
            if (_tokens.Count == 0 || _tokenIndex == 0)
            {
                _currentToken = AphidToken.None;                

                return false;
            }

            _currentToken = _tokens[--_tokenIndex];

            return true;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public bool SetToken(int index)
        {
            _tokenIndex = index;

            if (_tokenIndex < _tokens.Count)
            {
                _currentToken = _tokens[_tokenIndex];
                return true;
            }

            _currentToken = AphidToken.None;            

            return false;
        }

        public static List<AphidExpression> Parse(List<AphidToken> tokens, string code) =>
            Parse(tokens, code, useImplicitReturns: true);

        public static List<AphidExpression> Parse(
            List<AphidToken> tokens,
            string code,
            bool useImplicitReturns = true) =>
            Parse(tokens, code, null, useImplicitReturns);

        public static List<AphidExpression> Parse(
            List<AphidToken> tokens,
            string code,
            string filename,
            bool useImplicitReturns = true)
        {
            var parser = new AphidParser(tokens)
            {
                UseImplicitReturns = useImplicitReturns
            };

            List<AphidExpression> ast;

            try
            {
                ast = parser.Parse();
            }
            catch (AphidParserException e)
                when (!string.IsNullOrEmpty(filename) && string.IsNullOrEmpty(e.Filename))
            {
                e.Filename = filename;
                throw;
            }

            if (ast.Count != 0)
            {
                if (code != null)
                {
                    ast[0].Code = code;
                }

                if (filename != null)
                {
                    ast[0].Filename = filename;
                }
            }

            new AphidCodeVisitor().Visit(ast);

            return ast;
        }

        public static AphidExpression ParseExpression(string code) =>
            ParseExpression(AphidLexer.GetTokens(code), code);

        public static AphidExpression ParseExpression(List<AphidToken> tokens, string code)
        {
            var ast = Parse(tokens, code);

            if (ast.Count != 1)
            {
                throw new AphidParserException(
                    "Expected end of expression, found: {0}",
                    code.Substring(ast[1].Index));
            }

            return ast[0];
        }

        public static List<AphidExpression> Parse(string code) =>
            Parse(code, useImplicitReturns: true);

        public static List<AphidExpression> Parse(
            string code,
            bool isTextDocument = false,
            bool useImplicitReturns = true) =>
            Parse(code, null, isTextDocument, useImplicitReturns);

        public static List<AphidExpression> Parse(
            string code,
            string filename,
            bool isTextDocument = false,
            bool useImplicitReturns = true)
        {
            var lexer = new AphidLexer(code);

            if (isTextDocument)
            {
                lexer.SetTextMode();
            }

            return Parse(
                lexer.GetTokens(),
                code,
                filename,
                useImplicitReturns: useImplicitReturns);
        }

        public static List<AphidExpression> ParseFile(string filename) =>
            ParseFile(
                filename,
                isTextDocument: false,
                useImplicitReturns: true);

        public static List<AphidExpression> ParseFile(
            string filename,
            bool isTextDocument = false,
            bool useImplicitReturns = true) =>
            Parse(
                AphidScript.Read(filename),
                filename,
                isTextDocument,
                useImplicitReturns);
    }
}
