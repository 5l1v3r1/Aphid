﻿using Components.Aphid.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Components.Aphid.TypeSystem
{
    [Serializable]
    public class AphidFunction : IEnumerable<AphidExpression>, IAphidCallable
    {
        [NonSerialized]
        private AphidObject _parentScope;

        [ScriptIgnore, IgnoreDataMember]
        public AphidObject ParentScope
        {
            get { return _parentScope; }
            set { _parentScope = value; }
        }

        public string[] Args { get; set; }

        public List<AphidExpression> Body { get; set; }

        public AphidFunction()
            : this(new string[0], new List<AphidExpression>(), null)
        {
        }

        public AphidFunction(string[] args, List<AphidExpression> body, AphidObject parentScope)
        {
            Args = args;
            Body = body;
            ParentScope = parentScope;
        }

        public AphidFunction Clone()
        {
            return new AphidFunction(Args, Body, ParentScope);
        }

        public IEnumerator<AphidExpression> GetEnumerator()
        {
            return Body != null ?
                Body.GetEnumerator() :
                ((IEnumerable<AphidExpression>)new AphidExpression[0]).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Body != null ?
                ((System.Collections.IEnumerable)Body).GetEnumerator() :
                new AphidExpression[0].GetEnumerator();
        }
    }
}
