using Components.Aphid.Interpreter;
using System;
using Components.Aphid.TypeSystem;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Components.Aphid.TypeSystem.Tests
{
    /// <summary>This class contains parameterized unit tests for AphidFunctionWrapper</summary>
    [TestClass]
    [PexClass(typeof(AphidFunctionWrapper))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class AphidFunctionWrapperTest
    {

        /// <summary>Test stub for .ctor(AphidInterpreter, AphidFunction)</summary>
        [PexMethod]
        public AphidFunctionWrapper ConstructorTest(AphidInterpreter interpreter, AphidFunction function)
        {
            AphidFunctionWrapper target = new AphidFunctionWrapper(interpreter, function);
            return target;
            // TODO: add assertions to method AphidFunctionWrapperTest.ConstructorTest(AphidInterpreter, AphidFunction)
        }

        /// <summary>Test stub for Call()</summary>
        [PexMethod]
        public void CallTest([PexAssumeUnderTest]AphidFunctionWrapper target)
        {
            target.Call();
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest(AphidFunctionWrapper)
        }

        /// <summary>Test stub for Call(!!0)</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public void CallTest01<T1>([PexAssumeUnderTest]AphidFunctionWrapper target, T1 arg)
        {
            target.Call<T1>(arg);
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest01(AphidFunctionWrapper, !!0)
        }

        /// <summary>Test stub for Call(!!0, !!1)</summary>
        [PexGenericArguments(typeof(int), typeof(int))]
        [PexMethod]
        public void CallTest02<T1, T2>(
            [PexAssumeUnderTest]AphidFunctionWrapper target,
            T1 arg,
            T2 arg2
        )
        {
            target.Call<T1, T2>(arg, arg2);
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest02(AphidFunctionWrapper, !!0, !!1)
        }

        /// <summary>Test stub for Call(!!0, !!1, !!2)</summary>
        [PexGenericArguments(typeof(int), typeof(int), typeof(int))]
        [PexMethod]
        public void CallTest03<T1, T2, T3>(
            [PexAssumeUnderTest]AphidFunctionWrapper target,
            T1 arg,
            T2 arg2,
            T3 arg3
        )
        {
            target.Call<T1, T2, T3>(arg, arg2, arg3);
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest03(AphidFunctionWrapper, !!0, !!1, !!2)
        }

        /// <summary>Test stub for Call(!!0, !!1, !!2, !!3)</summary>
        [PexGenericArguments(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) })]
        [PexMethod]
        public void CallTest04<T1, T2, T3, T4>(
            [PexAssumeUnderTest]AphidFunctionWrapper target,
            T1 arg,
            T2 arg2,
            T3 arg3,
            T4 arg4
        )
        {
            target.Call<T1, T2, T3, T4>(arg, arg2, arg3, arg4);
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest04(AphidFunctionWrapper, !!0, !!1, !!2, !!3)
        }

        /// <summary>Test stub for Call()</summary>
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public TResult CallTest05<TResult>([PexAssumeUnderTest]AphidFunctionWrapper target)
        {
            TResult result = target.Call<TResult>();
            return result;
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest05(AphidFunctionWrapper)
        }

        /// <summary>Test stub for Call(!!0)</summary>
        [PexGenericArguments(typeof(int), typeof(int))]
        [PexMethod]
        public TResult CallTest06<T1, TResult>([PexAssumeUnderTest]AphidFunctionWrapper target, T1 arg)
        {
            TResult result = target.Call<T1, TResult>(arg);
            return result;
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest06(AphidFunctionWrapper, !!0)
        }

        /// <summary>Test stub for Call(!!0, !!1)</summary>
        [PexGenericArguments(typeof(int), typeof(int), typeof(int))]
        [PexMethod]
        public TResult CallTest07<T1, T2, TResult>(
            [PexAssumeUnderTest]AphidFunctionWrapper target,
            T1 arg,
            T2 arg2
        )
        {
            TResult result = target.Call<T1, T2, TResult>(arg, arg2);
            return result;
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest07(AphidFunctionWrapper, !!0, !!1)
        }

        /// <summary>Test stub for Call(!!0, !!1, !!2)</summary>
        [PexGenericArguments(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) })]
        [PexMethod]
        public TResult CallTest08<T1, T2, T3, TResult>(
            [PexAssumeUnderTest]AphidFunctionWrapper target,
            T1 arg,
            T2 arg2,
            T3 arg3
        )
        {
            TResult result = target.Call<T1, T2, T3, TResult>(arg, arg2, arg3);
            return result;
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest08(AphidFunctionWrapper, !!0, !!1, !!2)
        }

        /// <summary>Test stub for Call(!!0, !!1, !!2, !!3)</summary>
        [PexGenericArguments(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) })]
        [PexMethod]
        public TResult CallTest09<T1, T2, T3, T4, TResult>(
            [PexAssumeUnderTest]AphidFunctionWrapper target,
            T1 arg,
            T2 arg2,
            T3 arg3,
            T4 arg4
        )
        {
            TResult result = target.Call<T1, T2, T3, T4, TResult>(arg, arg2, arg3, arg4);
            return result;
            // TODO: add assertions to method AphidFunctionWrapperTest.CallTest09(AphidFunctionWrapper, !!0, !!1, !!2, !!3)
        }
    }
}
