// <copyright file="FileCacheInfoTest.cs">Copyright © AutoSec Tools LLC 2018</copyright>
using System;
using Components.Aphid;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Components.Aphid.Tests
{
    /// <summary>This class contains parameterized unit tests for FileCacheInfo</summary>
    [PexClass(typeof(FileCacheInfo))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class FileCacheInfoTest
    {
        /// <summary>Test stub for .ctor(FileCacheSource[])</summary>
        [PexMethod]
        public FileCacheInfo ConstructorTest(FileCacheSource[] sources)
        {
            FileCacheInfo target = new FileCacheInfo(sources);
            return target;
            // TODO: add assertions to method FileCacheInfoTest.ConstructorTest(FileCacheSource[])
        }

        /// <summary>Test stub for GetIsOutdated()</summary>
        [PexMethod]
        [PexAllowedException(typeof(ArgumentNullException))]
        [PexAllowedException(typeof(NullReferenceException))]
        public bool GetIsOutdatedTest([PexAssumeUnderTest]FileCacheInfo target)
        {
            bool result = target.GetIsOutdated();
            return result;
            // TODO: add assertions to method FileCacheInfoTest.GetIsOutdatedTest(FileCacheInfo)
        }

        /// <summary>Test stub for get_IsOutdated()</summary>
        [PexMethod(MaxRunsWithoutNewTests = 200)]
        [PexAllowedException(typeof(NullReferenceException))]
        [PexAllowedException(typeof(ArgumentNullException))]
        public bool IsOutdatedGetTest([PexAssumeUnderTest]FileCacheInfo target)
        {
            bool result = target.IsOutdated;
            return result;
            // TODO: add assertions to method FileCacheInfoTest.IsOutdatedGetTest(FileCacheInfo)
        }
    }
}
