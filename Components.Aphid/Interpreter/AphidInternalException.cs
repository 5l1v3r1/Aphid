﻿using Components.Aphid.Parser;
using Components.Aphid.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Interpreter
{
    public class AphidInternalException : AphidRuntimeException
    {
        public AphidInternalException(
            AphidObject exceptionScope,
            AphidExpression currentStatement,
            AphidExpression currentExpression,
            string message,
            params object[] args)
            : base(
                exceptionScope,
                currentStatement,
                currentExpression,
                message,
                args)
        {
        }
    }
}