using Microsoft.Pex.Framework.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework;
using Components.Aphid.Interpreter;
// <auto-generated>
// This file contains automatically generated tests.
// Do not modify this file manually.
// 
// If the contents of this file becomes outdated, you can delete it.
// For example, if it no longer compiles.
// </auto-generated>
using System;

namespace Components.Aphid.UI.Tests
{
    public partial class AphidCliTest
    {

[TestMethod]
[PexGeneratedBy(typeof(AphidCliTest))]
public void TryActionTest185()
{
    bool b;
    b = this.TryActionTest((AphidInterpreter)null, (string)null, (Action)null);
    PexAssert.AreEqual<bool>(false, b);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidCliTest))]
public void TryActionTest466()
{
    AphidInterpreter aphidInterpreter;
    bool b;
    aphidInterpreter = AphidInterpreterFactory.Create();
    b = this.TryActionTest
            (aphidInterpreter, (string)null, PexChoose.CreateDelegate<Action>());
    PexAssert.AreEqual<bool>(true, b);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidCliTest))]
[Ignore]
[PexDescription("the test state was: path bounds exceeded")]
public void TryActionTest115()
{
    AphidInterpreter aphidInterpreter;
    bool b;
    aphidInterpreter = AphidInterpreterFactory.Create();
    b = this.TryActionTest(aphidInterpreter, (string)null, (Action)null);
}
    }
}
