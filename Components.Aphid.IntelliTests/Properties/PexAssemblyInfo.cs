using System;
using System.Reflection.Emit;
using Microsoft.Pex.Framework.Using;
using System.Text;
using System.Runtime.ExceptionServices;
using System.IO;
using Microsoft.Pex.Framework.Suppression;
// <copyright file="PexAssemblyInfo.cs">Copyright © AutoSec Tools LLC 2018</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "NUnit3")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("Components.Aphid.Debug")]
[assembly: PexInstrumentAssembly("System.Configuration")]
[assembly: PexInstrumentAssembly("System.Web")]
[assembly: PexInstrumentAssembly("Microsoft.CSharp")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("System.Runtime.Serialization")]
[assembly: PexInstrumentAssembly("System.Web.Extensions")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Configuration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Web")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.CSharp")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Runtime.Serialization")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Web.Extensions")]
[assembly: PexSuppressStaticFieldStore("Components.Caching.FileCacheInfo+<>c", "<>9__7_0")]
[assembly: PexInstrumentType(typeof(File))]
[assembly: PexInstrumentType(typeof(ExceptionDispatchInfo))]
[assembly: PexInstrumentType(typeof(Path))]
[assembly: PexInstrumentType("mscorlib", "System.AppContextSwitches")]
[assembly: PexInstrumentType("mscorlib", "System.IO.PathInternal")]
[assembly: PexInstrumentType("mscorlib", "System.IO.PathHelper")]
[assembly: PexInstrumentType(typeof(BinaryReader))]
[assembly: PexInstrumentType(typeof(EncoderReplacementFallback))]
[assembly: PexInstrumentType(typeof(DecoderReplacementFallback))]
[assembly: PexUseType(typeof(AssemblyBuilder))]
[assembly: PexUseType(typeof(GC), "System.Reflection.RuntimeAssembly")]
[assembly: PexUseType(typeof(GC), "System.Reflection.Emit.InternalAssemblyBuilder")]
[assembly: PexInstrumentType(typeof(BinaryWriter))]
[assembly: PexInstrumentType(typeof(EncoderFallback))]
[assembly: PexInstrumentType(typeof(DecoderFallback))]
[assembly: PexInstrumentType(typeof(Decoder))]
[assembly: PexInstrumentType("mscorlib", "System.Text.UTF8Encoding+UTF8Decoder")]
[assembly: PexInstrumentType("mscorlib", "System.Text.UTF8Encoding+UTF8Encoder")]
[assembly: PexInstrumentType("mscorlib", "System.IO.__Error")]
[assembly: PexInstrumentType(typeof(EncoderExceptionFallback))]
[assembly: PexInstrumentType(typeof(EncoderFallbackBuffer))]
[assembly: PexInstrumentType("mscorlib", "System.Text.EncoderNLS")]
[assembly: PexInstrumentType(typeof(Encoder))]
[assembly: PexInstrumentType(typeof(Buffer))]
[assembly: PexInstrumentType(typeof(EncoderExceptionFallbackBuffer))]
[assembly: PexInstrumentType(typeof(Buffer))]
[assembly: PexInstrumentType(typeof(AppDomain))]
[assembly: PexInstrumentType(typeof(Buffer))]
[assembly: PexInstrumentType(typeof(Buffer))]
[assembly: PexInstrumentType(typeof(Buffer))]

