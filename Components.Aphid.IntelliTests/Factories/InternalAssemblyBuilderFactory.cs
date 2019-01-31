// <copyright file="InternalAssemblyBuilderFactory.cs">Copyright © AutoSec Tools LLC 2018</copyright>

using System;
using Microsoft.Pex.Framework;

namespace System.Reflection.Emit
{
    /// <summary>A factory for System.Reflection.Emit.InternalAssemblyBuilder instances</summary>
    public static partial class InternalAssemblyBuilderFactory
    {
        /// <summary>A factory for System.Reflection.Emit.InternalAssemblyBuilder instances</summary>
        [PexFactoryMethod(typeof(GC), "System.Reflection.Emit.InternalAssemblyBuilder")]
        public static object Create() =>
            AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("TestAsm"),
                AssemblyBuilderAccess.RunAndSave);
    }
}
