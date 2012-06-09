using System;
using System.Collections.Generic;

namespace Creelio.Framework.Templating.FormatHelpers
{
    public class ParameterList : List<Parameter>
    {
        #region Methods

        public static ParameterList Create()
        {
            return new ParameterList();
        }

        public ParameterList Add(Type type, string name)
        {
            var param = new Parameter(type, name);
            this.Add(param);
            return this;
        }

        public ParameterList Add(string type, string name)
        {
            var param = new Parameter(type, name);
            this.Add(param);
            return this;
        }

        public ParameterList Add(Type type, string name, string modifier)
        {
            var param = new Parameter(type, name, modifier);
            this.Add(param);
            return this;
        }

        public ParameterList Add(string type, string name, string modifer)
        {
            var param = new Parameter(type, name, modifer);
            this.Add(param);
            return this;
        }

        #endregion
    }
}