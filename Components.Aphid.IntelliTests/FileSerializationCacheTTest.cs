using System;
using Components.Aphid;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Components.Aphid.IntelliTests
{
    /// <summary>This class contains parameterized unit tests for FileSerializationCache`1</summary>
    [TestFixture]
    [PexClass(typeof(FileSerializationCache<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class FileSerializationCacheTTest
    {

        /// <summary>Test stub for Read(String)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public T ReadTest<T>([PexAssumeNotNull]FileSerializationCache<T> target, string filename)
        {
            T result = target.Read(filename);
            return result;
            // TODO: add assertions to method FileSerializationCacheTTest.ReadTest(FileSerializationCache`1<!!0>, String)
        }

        /// <summary>Test stub for Read(String, FileCacheSource[]&amp;)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public T ReadTest01<T>(
            [PexAssumeNotNull]FileSerializationCache<T> target,
            string filename,
            out FileCacheSource[] cacheSources
        )
        {
            T result = target.Read(filename, out cacheSources);
            return result;
            // TODO: add assertions to method FileSerializationCacheTTest.ReadTest01(FileSerializationCache`1<!!0>, String, FileCacheSource[]&)
        }
    }
}
