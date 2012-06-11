namespace Creelio.Framework.Templating.FormatHelpers
{
    using System;
    using System.Collections.Generic;

    public class CsParameterList : List<CsParameter>
    {
        public static CsParameterList Create()
        {
            return new CsParameterList();
        }

        public CsParameterList Add(Type type, string name)
        {
            var param = new CsParameter(type, name);
            this.Add(param);
            return this;
        }

        public CsParameterList Add(string type, string name)
        {
            var param = new CsParameter(type, name);
            this.Add(param);
            return this;
        }

        public CsParameterList Add(Type type, string name, string modifier)
        {
            var param = new CsParameter(type, name, modifier);
            this.Add(param);
            return this;
        }

        public CsParameterList Add(string type, string name, string modifer)
        {
            var param = new CsParameter(type, name, modifer);
            this.Add(param);
            return this;
        }
    }
}