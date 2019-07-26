using Microsoft.Pex.Framework.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Components.Aphid.Interpreter;
// <copyright file="AphidInterpreterTest.GetImports.g.cs">Copyright © AutoSec Tools LLC 2019</copyright>
// <auto-generated>
// This file contains automatically generated tests.
// Do not modify this file manually.
// 
// If the contents of this file becomes outdated, you can delete it.
// For example, if it no longer compiles.
// </auto-generated>
using System;

namespace Components.Aphid.Interpreter.Tests
{
    public partial class AphidInterpreterTest
    {

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
public void GetImports961()
{
    AphidInterpreter aphidInterpreter;
    HashSet<string> hashSet;
    aphidInterpreter = AphidInterpreterFactory.Create();
    hashSet = this.GetImports(aphidInterpreter);
    Assert.IsNotNull((object)hashSet);
    Assert.AreEqual<int>(2, hashSet.Count);
    Assert.IsNotNull(hashSet.Comparer);
    Assert.IsNotNull((object)aphidInterpreter);
    Assert.IsNull(aphidInterpreter.OnInterpretBlock);
    Assert.AreEqual<bool>(false, aphidInterpreter.OnInterpretBlockExecuting);
    Assert.IsNull(aphidInterpreter.OnInterpretStatement);
    Assert.AreEqual<bool>(false, aphidInterpreter.OnInterpretStatementExecuting);
    Assert.IsNull(aphidInterpreter.OnInterpretExpression);
    Assert.AreEqual<bool>(false, aphidInterpreter.OnInterpretExpressionExecuting);
    Assert.IsNull(aphidInterpreter.OnInterpretObject);
    Assert.AreEqual<bool>(false, aphidInterpreter.OnInterpretObjectExecuting);
    Assert.IsNotNull(aphidInterpreter.InitialScope);
    Assert.AreEqual<bool>(false, aphidInterpreter.InitialScope.IsScalar);
    Assert.AreEqual<bool>(true, aphidInterpreter.InitialScope.IsComplex);
    Assert.AreEqual<bool>(true, aphidInterpreter.InitialScope.IsComplexitySet);
    Assert.IsNull(aphidInterpreter.InitialScope.Value);
    Assert.IsNull(aphidInterpreter.InitialScope.Parent);
    Assert.IsNotNull(aphidInterpreter.InitialScope.Comparer);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.InitialScope.Comparer, hashSet.Comparer));
    Assert.AreEqual<int>(6, aphidInterpreter.InitialScope.Count);
    Assert.IsNotNull(aphidInterpreter.CurrentScope);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.CurrentScope, aphidInterpreter.InitialScope));
    Assert.IsNull(aphidInterpreter.PreviousScope);
    Assert.AreEqual<int>(-1, aphidInterpreter.OwnerThread);
    Assert.IsNotNull(aphidInterpreter.AsmBuilder);
    Assert.AreEqual<string>("AphidModule", 
                            aphidInterpreter.AsmBuilder.AssemblyName.RemoveAtIndexOf('_'));
    Assert.AreEqual<string>("AphidModule", 
                            aphidInterpreter.AsmBuilder.AssemblyFilename.RemoveAtIndexOf('_'));
    Assert.IsNull(aphidInterpreter.AsmBuilder.Assembly);
    Assert.IsNotNull(aphidInterpreter.AsmBuilder.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.AsmBuilder.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.InteropMethodResolver);
    Assert.IsNotNull(aphidInterpreter.InteropMethodResolver.Interpreter);
    Assert.IsTrue
        (object.ReferenceEquals(aphidInterpreter.InteropMethodResolver.Interpreter, 
                                (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.OperatorHelper);
    Assert.IsNotNull(aphidInterpreter.OperatorHelper.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.OperatorHelper.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.ValueHelper);
    Assert.IsNotNull(aphidInterpreter.ValueHelper.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.ValueHelper.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.InteropTypeResolver);
    Assert.IsNotNull(aphidInterpreter.InteropTypeResolver.Interpreter);
    Assert.IsTrue
        (object.ReferenceEquals(aphidInterpreter.InteropTypeResolver.Interpreter, 
                                (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.TypeExtender);
    Assert.IsNotNull(aphidInterpreter.TypeExtender.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.TypeExtender.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.TypeConverter);
    Assert.IsNotNull(aphidInterpreter.TypeConverter.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.TypeConverter.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.FunctionConverter);
    Assert.IsNotNull(aphidInterpreter.FunctionConverter.Interpreter);
    Assert.IsTrue
        (object.ReferenceEquals(aphidInterpreter.FunctionConverter.Interpreter, 
                                (object)aphidInterpreter));
    Assert.IsNull(aphidInterpreter.IpcContext);
    Assert.IsNotNull(aphidInterpreter.Serializer);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.IgnoreLazyLists);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.IgnoreFunctions);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.IgnoreSpecialVariables);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.QuoteToStringResults);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.AlwaysQuoteKeys);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.ToStringClrTypes);
    Assert.AreEqual<int>(-1, aphidInterpreter.Serializer.MaxElements);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.SafeCollectionAccess);
    Assert.IsNotNull(aphidInterpreter.Serializer.InlineStrings);
    Assert.AreEqual<int>(0, aphidInterpreter.Serializer.InlineStrings.Count);
    Assert.IsNotNull(aphidInterpreter.Serializer.InlineStrings.Comparer);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Serializer.InlineStrings.Comparer, hashSet.Comparer));
    Assert.IsNull(aphidInterpreter.Serializer.MapClrObject);
    Assert.AreEqual<int>(60, aphidInterpreter.Serializer.StringReferenceThreshold);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.SplitStrings);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.SplitAtNewLine);
    Assert.AreEqual<int>(100, aphidInterpreter.Serializer.StringChunkSize);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.UseDoubleQuotes);
    Assert.IsNotNull(aphidInterpreter.Serializer.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Serializer.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.Out);
    Assert.IsNull(aphidInterpreter.OutFilter);
    Assert.IsNull(aphidInterpreter.GatorEmitFilter);
    Assert.IsNotNull(aphidInterpreter.Loader);
    Assert.IsNotNull(aphidInterpreter.Loader.SystemSearchPaths);
    Assert.AreEqual<int>(1, aphidInterpreter.Loader.SystemSearchPaths.Count);
    Assert.IsNotNull(aphidInterpreter.Loader.SystemSearchPaths.Comparer);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Loader.SystemSearchPaths.Comparer, hashSet.Comparer));
    Assert.IsNotNull(aphidInterpreter.Loader.SearchPaths);
    Assert.AreEqual<int>(0, aphidInterpreter.Loader.SearchPaths.Count);
    Assert.IsNotNull(aphidInterpreter.Loader.SearchPaths.Comparer);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Loader.SearchPaths.Comparer, hashSet.Comparer));
    Assert.AreEqual<bool>(false, aphidInterpreter.Loader.InlineCachedScripts);
    Assert.AreEqual<bool>(false, aphidInterpreter.Loader.DisableConstantFolding);
    Assert.IsNotNull(aphidInterpreter.Loader.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Loader.Interpreter, (object)aphidInterpreter));
    Assert.IsNull(aphidInterpreter.CurrentStatement);
    Assert.IsNull(aphidInterpreter.CurrentExpression);
    Assert.AreEqual<bool>(true, aphidInterpreter.StrictMode);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
public void GetImports854()
{
    AphidInterpreter aphidInterpreter;
    HashSet<string> hashSet;
    aphidInterpreter = AphidInterpreterFactory.Create();
    hashSet = this.GetImports(aphidInterpreter);
    Assert.IsNotNull((object)hashSet);
    Assert.AreEqual<int>(2, hashSet.Count);
    Assert.IsNotNull(hashSet.Comparer);
    Assert.IsNotNull((object)aphidInterpreter);
    Assert.IsNull(aphidInterpreter.OnInterpretBlock);
    Assert.AreEqual<bool>(false, aphidInterpreter.OnInterpretBlockExecuting);
    Assert.IsNull(aphidInterpreter.OnInterpretStatement);
    Assert.AreEqual<bool>(false, aphidInterpreter.OnInterpretStatementExecuting);
    Assert.IsNull(aphidInterpreter.OnInterpretExpression);
    Assert.AreEqual<bool>(false, aphidInterpreter.OnInterpretExpressionExecuting);
    Assert.IsNull(aphidInterpreter.OnInterpretObject);
    Assert.AreEqual<bool>(false, aphidInterpreter.OnInterpretObjectExecuting);
    Assert.IsNotNull(aphidInterpreter.InitialScope);
    Assert.AreEqual<bool>(false, aphidInterpreter.InitialScope.IsScalar);
    Assert.AreEqual<bool>(true, aphidInterpreter.InitialScope.IsComplex);
    Assert.AreEqual<bool>(true, aphidInterpreter.InitialScope.IsComplexitySet);
    Assert.IsNull(aphidInterpreter.InitialScope.Value);
    Assert.IsNull(aphidInterpreter.InitialScope.Parent);
    Assert.IsNotNull(aphidInterpreter.InitialScope.Comparer);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.InitialScope.Comparer, hashSet.Comparer));
    Assert.AreEqual<int>(6, aphidInterpreter.InitialScope.Count);
    Assert.IsNotNull(aphidInterpreter.CurrentScope);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.CurrentScope, aphidInterpreter.InitialScope));
    Assert.IsNull(aphidInterpreter.PreviousScope);
    Assert.AreEqual<int>(-1, aphidInterpreter.OwnerThread);
    Assert.IsNotNull(aphidInterpreter.AsmBuilder);
    Assert.AreEqual<string>("AphidModule", 
                            aphidInterpreter.AsmBuilder.AssemblyName.RemoveAtIndexOf('_'));
    Assert.AreEqual<string>("AphidModule", 
                            aphidInterpreter.AsmBuilder.AssemblyFilename.RemoveAtIndexOf('_'));
    Assert.IsNull(aphidInterpreter.AsmBuilder.Assembly);
    Assert.IsNotNull(aphidInterpreter.AsmBuilder.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.AsmBuilder.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.InteropMethodResolver);
    Assert.IsNotNull(aphidInterpreter.InteropMethodResolver.Interpreter);
    Assert.IsTrue
        (object.ReferenceEquals(aphidInterpreter.InteropMethodResolver.Interpreter, 
                                (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.OperatorHelper);
    Assert.IsNotNull(aphidInterpreter.OperatorHelper.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.OperatorHelper.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.ValueHelper);
    Assert.IsNotNull(aphidInterpreter.ValueHelper.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.ValueHelper.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.InteropTypeResolver);
    Assert.IsNotNull(aphidInterpreter.InteropTypeResolver.Interpreter);
    Assert.IsTrue
        (object.ReferenceEquals(aphidInterpreter.InteropTypeResolver.Interpreter, 
                                (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.TypeExtender);
    Assert.IsNotNull(aphidInterpreter.TypeExtender.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.TypeExtender.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.TypeConverter);
    Assert.IsNotNull(aphidInterpreter.TypeConverter.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.TypeConverter.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.FunctionConverter);
    Assert.IsNotNull(aphidInterpreter.FunctionConverter.Interpreter);
    Assert.IsTrue
        (object.ReferenceEquals(aphidInterpreter.FunctionConverter.Interpreter, 
                                (object)aphidInterpreter));
    Assert.IsNull(aphidInterpreter.IpcContext);
    Assert.IsNotNull(aphidInterpreter.Serializer);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.IgnoreLazyLists);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.IgnoreFunctions);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.IgnoreSpecialVariables);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.QuoteToStringResults);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.AlwaysQuoteKeys);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.ToStringClrTypes);
    Assert.AreEqual<int>(-1, aphidInterpreter.Serializer.MaxElements);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.SafeCollectionAccess);
    Assert.IsNotNull(aphidInterpreter.Serializer.InlineStrings);
    Assert.AreEqual<int>(0, aphidInterpreter.Serializer.InlineStrings.Count);
    Assert.IsNotNull(aphidInterpreter.Serializer.InlineStrings.Comparer);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Serializer.InlineStrings.Comparer, hashSet.Comparer));
    Assert.IsNull(aphidInterpreter.Serializer.MapClrObject);
    Assert.AreEqual<int>(60, aphidInterpreter.Serializer.StringReferenceThreshold);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.SplitStrings);
    Assert.AreEqual<bool>(true, aphidInterpreter.Serializer.SplitAtNewLine);
    Assert.AreEqual<int>(100, aphidInterpreter.Serializer.StringChunkSize);
    Assert.AreEqual<bool>(false, aphidInterpreter.Serializer.UseDoubleQuotes);
    Assert.IsNotNull(aphidInterpreter.Serializer.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Serializer.Interpreter, (object)aphidInterpreter));
    Assert.IsNotNull(aphidInterpreter.Out);
    Assert.IsNull(aphidInterpreter.OutFilter);
    Assert.IsNull(aphidInterpreter.GatorEmitFilter);
    Assert.IsNotNull(aphidInterpreter.Loader);
    Assert.IsNotNull(aphidInterpreter.Loader.SystemSearchPaths);
    Assert.AreEqual<int>(1, aphidInterpreter.Loader.SystemSearchPaths.Count);
    Assert.IsNotNull(aphidInterpreter.Loader.SystemSearchPaths.Comparer);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Loader.SystemSearchPaths.Comparer, hashSet.Comparer));
    Assert.IsNotNull(aphidInterpreter.Loader.SearchPaths);
    Assert.AreEqual<int>(0, aphidInterpreter.Loader.SearchPaths.Count);
    Assert.IsNotNull(aphidInterpreter.Loader.SearchPaths.Comparer);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Loader.SearchPaths.Comparer, hashSet.Comparer));
    Assert.AreEqual<bool>(false, aphidInterpreter.Loader.InlineCachedScripts);
    Assert.AreEqual<bool>(false, aphidInterpreter.Loader.DisableConstantFolding);
    Assert.IsNotNull(aphidInterpreter.Loader.Interpreter);
    Assert.IsTrue(object.ReferenceEquals
                      (aphidInterpreter.Loader.Interpreter, (object)aphidInterpreter));
    Assert.IsNull(aphidInterpreter.CurrentStatement);
    Assert.IsNull(aphidInterpreter.CurrentExpression);
    Assert.AreEqual<bool>(true, aphidInterpreter.StrictMode);
}
    }
}
