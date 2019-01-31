using System;
using Microsoft.Pex.Framework;

namespace System.Reflection
{
    /// <summary>A factory for System.Reflection.RuntimeAssembly instances</summary>
    public static partial class RuntimeAssemblyFactory
    {
        /// <summary>A factory for System.Reflection.RuntimeAssembly instances</summary>
        [PexFactoryMethod(typeof(GC), "System.Reflection.RuntimeAssembly")]
        public static object Create() => Assembly.GetExecutingAssembly();
    }
}
