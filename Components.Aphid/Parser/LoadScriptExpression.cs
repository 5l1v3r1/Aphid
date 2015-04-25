﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.Aphid.Parser
{
    public class LoadScriptExpression : AphidExpression, IParentNode
    {
        public override AphidExpressionType Type
        {
            get { return AphidExpressionType.LoadScriptExpression; }
        }

        public AphidExpression FileExpression { get; set; }

        public LoadScriptExpression(AphidExpression fileExpression)
        {
            FileExpression = fileExpression;
        }

        public IEnumerable<AphidExpression> GetChildren()
        {
            return new[] { FileExpression };
        }
    }
}
