using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Creelio.Framework.Extensions
{
    public static class StringEx
    {
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            else if (str.Length == 1)
            {
                return str.ToLower();
            }
            else
            {
                return string.Format("{0}{1}", str[0].ToString().ToLower(), str.Substring(1));
            }
        }
    }
}
