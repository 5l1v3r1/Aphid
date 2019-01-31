using System.IO;
using System;
using Components.Aphid;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Components.Aphid.Tests
{
    /// <summary>This class contains parameterized unit tests for FileMemoryCache</summary>
    [TestFixture]
    [PexClass(typeof(FileMemoryCache))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class FileMemoryCacheTest
    {

        /// <summary>Test stub for OpenRead(String)</summary>
        [PexMethod]
        public Stream OpenReadTest(string filename)
        {
            Stream result = FileMemoryCache.OpenRead(filename);
            return result;
            // TODO: add assertions to method FileMemoryCacheTest.OpenReadTest(String)
        }

        /// <summary>Test stub for ReadAllBytes(String)</summary>
        [PexMethod]
        public byte[] ReadAllBytesTest(string filename)
        {
            byte[] result = FileMemoryCache.ReadAllBytes(filename);
            return result;
            // TODO: add assertions to method FileMemoryCacheTest.ReadAllBytesTest(String)
        }

        /// <summary>Test stub for WriteAllBytes(String, Byte[])</summary>
        [PexMethod]
        public void WriteAllBytesTest(string filename, byte[] buffer)
        {
            FileMemoryCache.WriteAllBytes(filename, buffer);
            // TODO: add assertions to method FileMemoryCacheTest.WriteAllBytesTest(String, Byte[])
        }
    }
}
