using System;
using System.Collections.Generic;
using System.Linq;

namespace Creelio.Framework.Templating.FormatHelpers
{
    public class ArgumentList : List<Argument>
    {
        #region Methods

        public static ArgumentList Create()
        {
            return new ArgumentList();
        }

        public static ArgumentList CreateFrom(ParameterList parameterList)
        {
            return CreateFrom(parameterList, (string[])null);
        }

        public static ArgumentList CreateFrom(ParameterList parameterList, params string[] includedParameters)
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

        public static ArgumentList CreateFrom(ParameterList parameterList, Func<ParameterList, IEnumerable<Parameter>> filter)
        {
            var argumentList = Create();
            var enumerableParams = filter == null ? parameterList : filter(parameterList);

            argumentList.AddRange(
                enumerableParams.ToList()
                                .ConvertAll(p => new Argument(p.Name, p.Modifier))
            );

            return argumentList;
        }

        public ArgumentList Add(string name)
        {
            var arg = new Argument(name);
            this.Add(arg);
            return this;
        }

        public ArgumentList Add(string name, string modifier)
        {
            var arg = new Argument(name, modifier);
            this.Add(arg);
            return this;
        }

        #endregion
    }
}