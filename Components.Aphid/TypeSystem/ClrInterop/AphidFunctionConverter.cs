﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Components.External;
using Components.Aphid.Interpreter;

namespace Components.Aphid.TypeSystem
{
    public class AphidFunctionConverter : AphidRuntimeComponent
    {
        public AphidFunctionConverter(AphidInterpreter interpreter)
            : base(interpreter)
        {

        }

        public Delegate Convert(
            Type methodType,
            object function,
            Type[] genericArguments)
        {
            var methodInfo = methodType.GetMethod("Invoke");
            var funcObj = Interpreter.ValueHelper.Unwrap(function);
            var func = funcObj as AphidFunction;

            if (func == null)
            {
                throw Interpreter.CreateRuntimeException(
                    "Conversion from {0} to {1} not supported.",
                    function.GetType(),
                    methodInfo.ToString());
            }

            return ConvertFunctionWrapper(methodInfo, methodType, func, genericArguments);
        }

        public Delegate ConvertFunctionWrapper(
            MethodInfo method,
            Type delegateType,
            AphidFunction function,
            Type[] genericArguments)
        {
            var methodParams = method.GetParameters();
            
            var wrapper = new AphidFunctionWrapper(Interpreter, function);
            var call = wrapper
                .GetType()
                .GetMethods()
                .Where(x =>
                    x.Name == "Call" &&
                    (method.ReturnType == typeof(void) ? 
                        x.ReturnType == typeof(void) :
                        x.ReturnType != typeof(void)) &&
                    methodParams.Length == x.GetParameters().Length)
                .SingleOrDefault();

            if (call == null)
            {
                throw Interpreter.CreateRuntimeException(
                    "Could not match conversion function for {0}",
                    method);
            }

            //Todo: Add support for T Call().
            if (methodParams.Length > 0)
            {
                var argOffset = 0;

                var paramTypes = methodParams
                    .Select(x => !x.ParameterType.IsGenericParameter ?
                        x.ParameterType :
                        genericArguments[argOffset++]);

                call = call.MakeGenericMethod(
                    (method.ReturnType == typeof(void) ?
                        paramTypes :
                        paramTypes.Concat(new[] { method.ReturnType }))
                        .ToArray());
            }

            if (delegateType.ContainsGenericParameters)
            {
                var argOffset = 0;
                var curDelegateArgs = delegateType.GetGenericArguments();
                var delegateTypeParams = new Type[curDelegateArgs.Length];

                for (var i = 0; i < curDelegateArgs.Length; i++)
                {
                    delegateTypeParams[i] = !curDelegateArgs[i].IsGenericParameter ?
                        curDelegateArgs[i] :
                        genericArguments[argOffset++];
                }

                delegateType = delegateType
                    .GetGenericTypeDefinition()
                    .MakeGenericType(delegateTypeParams);
            }

            return call.CreateDelegate(delegateType, wrapper);
        }
    }
}