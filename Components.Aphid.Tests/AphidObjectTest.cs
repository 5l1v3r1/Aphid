using System.Reflection;
using System.Collections.Generic;
// <copyright file="AphidObjectTest.cs">Copyright © AutoSec Tools LLC 2019</copyright>

using System;
using Components.Aphid.TypeSystem;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Components.Aphid.TypeSystem.Tests
{
    [TestClass]
    [PexClass(typeof(AphidObject))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class AphidObjectTest
    {

        [PexMethod]
        public AphidObject TryResolveParent([PexAssumeUnderTest]AphidObject target, string key)
        {
            AphidObject result = target.TryResolveParent(key);
            return result;
            // TODO: add assertions to method AphidObjectTest.TryResolveParent(AphidObject, String)
        }

        [PexMethod]
        [PexMethodUnderTest("GetPropertyInfo(Object, Boolean)")]
        internal IEnumerable<AphidPropertyInfo> GetPropertyInfo(object obj, bool allProperties)
        {
            object[] args = new object[2];
            args[0] = obj;
            args[1] = (object)allProperties;
            Type[] parameterTypes = new Type[2];
            parameterTypes[0] = typeof(object);
            parameterTypes[1] = typeof(bool);
            IEnumerable<AphidPropertyInfo> result0
               = ((MethodBase)(typeof(AphidObject).GetMethod("GetPropertyInfo",
                                                             BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic, (Binder)null,
                                                             CallingConventions.Standard, parameterTypes, (ParameterModifier[])null)))
                     .Invoke((object)null, args) as IEnumerable<AphidPropertyInfo>;
            IEnumerable<AphidPropertyInfo> result = result0;
            return result;
            // TODO: add assertions to method AphidObjectTest.GetPropertyInfo(Object, Boolean)
        }
    }
}
