using System.Collections.Generic;
using Components.Aphid.Parser;
using Components.Aphid.TypeSystem;
using Components.Aphid.Interpreter;
// <copyright file="AphidInterpreterFactory.cs">Copyright © AutoSec Tools LLC 2019</copyright>

using System;
using Microsoft.Pex.Framework;

namespace Components.Aphid.Interpreter
{
    /// <summary>A factory for Components.Aphid.Interpreter.AphidInterpreter instances</summary>
    public static partial class AphidInterpreterFactory
    {
        [PexFactoryMethod(typeof(AphidInterpreter))]
        public static AphidInterpreter Create() => new AphidInterpreter();

        /// <summary>A factory for Components.Aphid.Interpreter.AphidInterpreter instances</summary>
        [PexFactoryMethod(typeof(AphidInterpreter))]
        public static AphidInterpreter Create(
            AphidObject currentScope_aphidObject,
            AphidLoader loader_aphidLoader,
            Func<List<AphidExpression>, AphidExecutionContext, List<AphidExpression>> value_func,
            Action<AphidExecutionContext<AphidExpression>> value_action,
            Action<AphidExecutionContext<AphidExpression>> value_action1,
            Action<AphidExecutionContext<ObjectExpression>> value_action1_,
            Func<string, string> value_func1_,
            Func<string, string> value_func1_1,
            AphidExpression callExpression_aphidExpression,
            Lazy<string> name_lazy,
            object arg_o,
            IEnumerable<AphidExpression> block_iEnumerable,
            AphidObject sharedScope_aphidObject1,
            bool createChild_b,
            List<AphidExpression> expressions_list
        )
        {
            AphidInterpreter aphidInterpreter
               = new AphidInterpreter(currentScope_aphidObject, loader_aphidLoader);
            aphidInterpreter.OnInterpretBlock = value_func;
            aphidInterpreter.OnInterpretStatement = value_action;
            aphidInterpreter.OnInterpretExpression = value_action1;
            aphidInterpreter.OnInterpretObject = value_action1_;
            aphidInterpreter.OutFilter = value_func1_;
            aphidInterpreter.GatorEmitFilter = value_func1_1;
            aphidInterpreter.PushFrame(callExpression_aphidExpression, name_lazy, arg_o);
            aphidInterpreter.InsertNext(block_iEnumerable);
            aphidInterpreter.EnterSharedScope(sharedScope_aphidObject1, createChild_b);
            aphidInterpreter.Interpret(expressions_list);
            return aphidInterpreter;

            // TODO: Edit factory method of AphidInterpreter
            // This method should be able to configure the object in all possible ways.
            // Add as many parameters as needed,
            // and assign their values to each field by using the API.
        }
    }
}
