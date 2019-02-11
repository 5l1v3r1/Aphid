using System.IO;
using System.Collections.Generic;
using Components.Aphid.Lexer;
// <copyright file="AphidParserTest.cs">Copyright © AutoSec Tools LLC 2018</copyright>

using System;
using Components.Aphid.Parser;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Components.Aphid.Parser.IntelliTests
{
    /// <summary>This class contains parameterized unit tests for AphidParser</summary>
    [TestFixture]
    [PexClass(typeof(AphidLexer))]    
    public partial class AphidLexerTest
    {

        /// <summary>Test stub for .ctor(List`1&lt;AphidToken&gt;)</summary>
        [PexMethod(MaxRuns = 2000, MaxConstraintSolverTime = 10, MaxBranches = 1000)]
        [PexAllowedException(typeof(NullReferenceException))]
        public List<AphidToken> GetAllTokensTest(string text)
        {
            var lexer = new AphidLexer(text);
            
            return lexer.GetAllTokens();
            // TODO: add assertions to method AphidParserTest.ConstructorTest(List`1<AphidToken>)
        }
    }
}
