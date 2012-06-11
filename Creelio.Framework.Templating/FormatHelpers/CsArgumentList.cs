namespace Creelio.Framework.Templating.FormatHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CsArgumentList : List<CsArgument>
    {
        public static CsArgumentList Create()
        {
            return new CsArgumentList();
        }

        public static CsArgumentList CreateFrom(CsParameterList parameterList)
        {
            return CreateFrom(parameterList, (string[])null);
        }

        public static CsArgumentList CreateFrom(CsParameterList parameterList, params string[] includedParameters)
        {
            if (includedParameters == null)
            {
                return CreateFrom(parameterList, pl => pl);
            }
            else
            {
                return CreateFrom(parameterList, pl => pl.Where(p => includedParameters.Contains(p.Name)));
            }
        }

        public static CsArgumentList CreateFrom(CsParameterList parameterList, Func<CsParameterList, IEnumerable<CsParameter>> filter)
        {
            var argumentList = Create();
            var enumerableParams = filter == null ? parameterList : filter(parameterList);

            argumentList.AddRange(
                enumerableParams.ToList()
                                .ConvertAll(p => new CsArgument(p.Name, p.Modifier)));

            return argumentList;
        }

        public CsArgumentList Add(string name)
        {
            var arg = new CsArgument(name);
            this.Add(arg);
            return this;
        }

        public CsArgumentList Add(string name, string modifier)
        {
            var arg = new CsArgument(name, modifier);
            this.Add(arg);
            return this;
        }
    }
}