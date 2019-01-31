using System;
using Components.Aphid;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Components.Aphid.Tests
{
    /// <summary>This class contains parameterized unit tests for PrimitiveCacheInfo</summary>
    [TestFixture]
    [PexClass(typeof(PrimitiveCacheInfo))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class PrimitiveCacheInfoTest
    {

        /// <summary>Test stub for .ctor(Boolean)</summary>
        [PexMethod]
        public PrimitiveCacheInfo ConstructorTest(bool isOutdated)
        {
            PrimitiveCacheInfo target = new PrimitiveCacheInfo(isOutdated);
            return target;
            // TODO: add assertions to method PrimitiveCacheInfoTest.ConstructorTest(Boolean)
        }
    }
}
