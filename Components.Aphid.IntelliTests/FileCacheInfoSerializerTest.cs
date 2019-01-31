using System.Text;
using System.IO;
using System.Reflection;
using System;
using Components.Aphid;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Components.Aphid.Tests
{
    /// <summary>This class contains parameterized unit tests for FileCacheInfoSerializer</summary>
    [TestFixture]
    [PexClass(typeof(FileCacheInfoSerializer))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class FileCacheInfoSerializerTest
    {

        /// <summary>Test stub for .ctor(Version)</summary>
        [PexMethod]
        public FileCacheInfoSerializer ConstructorTest(Version dependencyVersion)
        {
            FileCacheInfoSerializer target = new FileCacheInfoSerializer(dependencyVersion);
            return target;
            // TODO: add assertions to method FileCacheInfoSerializerTest.ConstructorTest(Version)
        }

        /// <summary>Test stub for .ctor(Assembly)</summary>
        [PexMethod]
        [PexAllowedException(typeof(NullReferenceException))]
        public FileCacheInfoSerializer ConstructorTest01(Assembly dependency)
        {
            FileCacheInfoSerializer target = new FileCacheInfoSerializer(dependency);
            return target;
            // TODO: add assertions to method FileCacheInfoSerializerTest.ConstructorTest01(Assembly)
        }

        /// <summary>Test stub for Deserialize(Stream)</summary>
        [PexMethod]
        [PexAllowedException(typeof(ArgumentNullException))]
        public ICacheInfo DeserializeTest([PexAssumeUnderTest]FileCacheInfoSerializer target, Stream s)
        {
            ICacheInfo result = target.Deserialize(s);
            return result;
            // TODO: add assertions to method FileCacheInfoSerializerTest.DeserializeTest(FileCacheInfoSerializer, Stream)
        }

        /// <summary>Test stub for Serialize(Stream, FileCacheInfo)</summary>
        [PexMethod(MaxConditions = 100, Timeout = 30)]
        [PexAllowedException(typeof(ArgumentException))]
        [PexAllowedException(typeof(ArgumentNullException))]
        [PexAllowedException(typeof(NotSupportedException))]
        [PexAllowedException(typeof(NullReferenceException))]
        [PexAllowedException(typeof(EncoderFallbackException))]
        public void SerializeTest(
            [PexAssumeUnderTest]FileCacheInfoSerializer target,
            Stream s,
            FileCacheInfo cacheInfo
        )
        {
            target.Serialize(s, cacheInfo);
            // TODO: add assertions to method FileCacheInfoSerializerTest.SerializeTest(FileCacheInfoSerializer, Stream, FileCacheInfo)
        }
    }
}
