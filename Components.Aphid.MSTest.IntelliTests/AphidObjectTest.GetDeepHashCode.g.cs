using Microsoft.Pex.Framework.Generated;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Components.Aphid.TypeSystem;
// <copyright file="AphidObjectTest.GetDeepHashCode.g.cs">Copyright © AutoSec Tools LLC 2019</copyright>
// <auto-generated>
// This file contains automatically generated tests.
// Do not modify this file manually.
// 
// If the contents of this file becomes outdated, you can delete it.
// For example, if it no longer compiles.
// </auto-generated>
using System;

namespace Components.Aphid.TypeSystem.Tests
{
    public partial class AphidObjectTest
    {

[TestMethod]
[PexGeneratedBy(typeof(AphidObjectTest))]
public void GetDeepHashCode866()
{
    object o;
    int i;
    o = AphidObjectFactory.Create(679);
    i = this.GetDeepHashCode((AphidObject)o);
    Assert.AreEqual<int>(2100800, i);
    Assert.IsNotNull(o);
    Assert.AreEqual<bool>(false, ((AphidObject)o).IsScalar);
    Assert.AreEqual<bool>(true, ((AphidObject)o).IsComplex);
    Assert.AreEqual<bool>(true, ((AphidObject)o).IsComplexitySet);
    Assert.IsNull(((AphidObject)o).Value);
    Assert.IsNotNull(((AphidObject)o).Parent);
    Assert.AreEqual<bool>(false, ((AphidObject)o).Parent.IsScalar);
    Assert.AreEqual<bool>(true, ((AphidObject)o).Parent.IsComplex);
    Assert.AreEqual<bool>(true, ((AphidObject)o).Parent.IsComplexitySet);
    Assert.IsNull(((AphidObject)o).Parent.Value);
    Assert.IsNull(((AphidObject)o).Parent.Parent);
    Assert.IsNotNull(((AphidObject)o).Parent.Comparer);
    Assert.AreEqual<int>(0, ((AphidObject)o).Parent.Count);
    Assert.IsNotNull(((Dictionary<string, AphidObject>)o).Comparer);
    Assert.IsTrue
        (object.ReferenceEquals(((Dictionary<string, AphidObject>)o).Comparer, 
                                ((AphidObject)o).Parent.Comparer));
    Assert.AreEqual<int>(0, ((Dictionary<string, AphidObject>)o).Count);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidObjectTest))]
public void GetDeepHashCode976()
{
    object o;
    int i;
    o = AphidObjectFactory.Create(0);
    i = this.GetDeepHashCode((AphidObject)o);
    //Assert.AreEqual<int>(-707320578, i);
    Assert.IsNotNull(o);
    Assert.AreEqual<bool>(true, ((AphidObject)o).IsScalar);
    Assert.AreEqual<bool>(false, ((AphidObject)o).IsComplex);
    Assert.AreEqual<bool>(true, ((AphidObject)o).IsComplexitySet);
    Assert.IsNotNull(((AphidObject)o).Value);
    Assert.IsNull(((AphidObject)o).Parent);
    Assert.IsNotNull(((Dictionary<string, AphidObject>)o).Comparer);
    Assert.AreEqual<int>(0, ((Dictionary<string, AphidObject>)o).Count);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidObjectTest))]
public void GetDeepHashCode100()
{
    object o;
    int i;
    o = AphidObjectFactory.Create(7);
    i = this.GetDeepHashCode((AphidObject)o);
    //Assert.AreEqual<int>(-309629839, i);
    Assert.IsNotNull(o);
    Assert.AreEqual<bool>(false, ((AphidObject)o).IsScalar);
    Assert.AreEqual<bool>(true, ((AphidObject)o).IsComplex);
    Assert.AreEqual<bool>(true, ((AphidObject)o).IsComplexitySet);
    Assert.IsNull(((AphidObject)o).Value);
    Assert.IsNull(((AphidObject)o).Parent);
    Assert.IsNotNull(((Dictionary<string, AphidObject>)o).Comparer);
    Assert.AreEqual<int>(3, ((Dictionary<string, AphidObject>)o).Count);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidObjectTest))]
public void GetDeepHashCode608()
{
    object o;
    int i;
    o = AphidObjectFactory.Create();
    i = this.GetDeepHashCode((AphidObject)o);
    //Assert.AreEqual<int>(-309629839, i);
    Assert.IsNotNull(o);
    Assert.AreEqual<bool>(false, ((AphidObject)o).IsScalar);
    Assert.AreEqual<bool>(true, ((AphidObject)o).IsComplex);
    Assert.AreEqual<bool>(true, ((AphidObject)o).IsComplexitySet);
    Assert.IsNull(((AphidObject)o).Value);
    Assert.IsNull(((AphidObject)o).Parent);
    Assert.IsNotNull(((Dictionary<string, AphidObject>)o).Comparer);
    Assert.AreEqual<int>(3, ((Dictionary<string, AphidObject>)o).Count);
}
    }
}
