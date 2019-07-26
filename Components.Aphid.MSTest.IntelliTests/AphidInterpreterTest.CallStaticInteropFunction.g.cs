using Components.Aphid.Lexer;
using System.Collections.Generic;
using Microsoft.Pex.Framework.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Components.Aphid.Parser;
using Components.Aphid.TypeSystem;
using Components.Aphid.Interpreter;
// <copyright file="AphidInterpreterTest.CallStaticInteropFunction.g.cs">Copyright © AutoSec Tools LLC 2019</copyright>
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
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException516()
{
    AphidInterpreter aphidInterpreter;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    aphidObject =
      this.CallStaticInteropFunction(aphidInterpreter, (CallExpression)null);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException219()
{
    AphidInterpreter aphidInterpreter;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)null, 
                          (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException880()
{
    AphidInterpreter aphidInterpreter;
    List<AphidExpression> list;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    AphidExpression[] aphidExpressions = new AphidExpression[0];
    list = new List<AphidExpression>((IEnumerable<AphidExpression>)aphidExpressions);
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)null, list, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException119()
{
    AphidInterpreter aphidInterpreter;
    ArrayAccessExpression arrayAccessExpression;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    arrayAccessExpression = ArrayAccessExpressionFactory.Create
                                ((AphidExpressionContext)null, (AphidExpression)null, 
                                 (List<AphidExpression>)null, 0, 0);
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)arrayAccessExpression, 
                          (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException628()
{
    AphidInterpreter aphidInterpreter;
    ForExpression forExpression;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    forExpression = ForExpressionFactory.Create((AphidExpressionContext)null, 
                                                (AphidExpression)null, (AphidExpression)null, 
                                                (AphidExpression)null, (List<AphidExpression>)null, 0, 0);
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)forExpression, 
                          (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException215()
{
    AphidInterpreter aphidInterpreter;
    IfExpression ifExpression;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    ifExpression = IfExpressionFactory.Create((AphidExpressionContext)null, 
                                              (AphidExpression)null, (List<AphidExpression>)null, 
                                              (List<AphidExpression>)null, 0, 0);
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)ifExpression, 
                          (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException328()
{
    AphidInterpreter aphidInterpreter;
    PatternMatchingExpression patternMatchingExpression;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    patternMatchingExpression = PatternMatchingExpressionFactory.Create
                                    ((AphidExpressionContext)null, (AphidExpression)null, 
                                     (List<PatternExpression>)null, 0, 0);
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)patternMatchingExpression, 
                          (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException439()
{
    AphidInterpreter aphidInterpreter;
    ImplicitArgumentsExpression implicitArgumentsExpression;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    implicitArgumentsExpression = ImplicitArgumentsExpressionFactory.Create
                                      ((AphidExpressionContext)null, AphidTokenType.CustomOperator011, 0, 0);
    callExpression = CallExpressionFactory.Create((AphidExpressionContext)null, 
                                                  (AphidExpression)implicitArgumentsExpression, 
                                                  (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[PexRaisedException(typeof(NullReferenceException), Microsoft.Pex.Engine.Exceptions.PexExceptionState.Expected), ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException601()
{
    AphidInterpreter aphidInterpreter;
    BooleanExpression booleanExpression;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    booleanExpression =
      BooleanExpressionFactory.Create((AphidExpressionContext)null, false, 0, 0);
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)booleanExpression, 
                          (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException821()
{
    AphidInterpreter aphidInterpreter;
    GatorEmitExpression gatorEmitExpression;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    gatorEmitExpression = GatorEmitExpressionFactory.Create
                              ((AphidExpressionContext)null, (AphidExpression)null, 0, 0);
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)gatorEmitExpression, 
                          (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}

[TestMethod]
[PexGeneratedBy(typeof(AphidInterpreterTest))]
[ExpectedException(typeof(NullReferenceException))]
public void CallStaticInteropFunctionThrowsNullReferenceException618()
{
    AphidInterpreter aphidInterpreter;
    ObjectExpression objectExpression;
    CallExpression callExpression;
    AphidObject aphidObject;
    aphidInterpreter = AphidInterpreterFactory.Create();
    objectExpression = ObjectExpressionFactory.Create
                           ((AphidExpressionContext)null, (List<BinaryOperatorExpression>)null, 
                            (IdentifierExpression)null, 0, 0);
    callExpression = CallExpressionFactory.Create
                         ((AphidExpressionContext)null, (AphidExpression)objectExpression, 
                          (List<AphidExpression>)null, 0, 0);
    aphidObject = this.CallStaticInteropFunction(aphidInterpreter, callExpression);
}
    }
}