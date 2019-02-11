// <copyright file="OperatorHelperTest.cs">Copyright © AutoSec Tools LLC 2018</copyright>
using System;
using Components.Aphid.Interpreter;
using Components.Aphid.TypeSystem;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Components.Aphid.TypeSystem.IntelliTests
{
    /// <summary>This class contains parameterized unit tests for OperatorHelper</summary>
    [PexClass(typeof(OperatorHelper))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class OperatorHelperTest
    {
        private AphidInterpreter _interpreter = new AphidInterpreter();

        /// <summary>Test stub for Equals(AphidInterpreter, Object, Object)</summary>
        [PexMethod(MaxRunsWithoutNewTests = 200)]
        [PexAllowedException(typeof(AphidRuntimeException))]
        public bool EqualsTest(object left, object right)
        {
            bool result = OperatorHelper.Equals(_interpreter, left, right);
            return result;
            // TODO: add assertions to method OperatorHelperTest.EqualsTest(AphidInterpreter, Object, Object)
        }
    }
}
