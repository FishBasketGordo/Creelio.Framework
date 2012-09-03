namespace Creelio.Framework.Web.Extensions
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using Creelio.Framework.Extensions;

    public static class PageExtensions
    {
        public static string FormatHtmlID(this Page page, params string[] idParts)
        {
            return FormatHtmlID(idParts);
        }

        public static string FormatHtmlID(this MasterPage master, params string[] idParts)
        {
            return FormatHtmlID(idParts);
        }

        public static T As<T>(this MasterPage master)
            where T : MasterPage
        {
            master = master.Page.Master;

            while (master != null)
            {
                if (master is T)
                {
                    return (T)master;
                }
                else
                {
                    master = master.Master;
                }
            }

            return null;
        }

        private static string FormatHtmlID(params string[] idParts)
        {
            idParts.ThrowIfNullOrEmpty(_ => new ArgumentNullException("idParts"));

            // According to the W3C:
            // ID and NAME tokens must begin with a letter ([A-Za-z]) and 
            // may be followed by any number of letters, digits ([0-9]), 
            // hyphens ("-"), underscores ("_"), colons (":"), and periods (".").
            //
            // http://www.w3.org/TR/html4/types.html#type-name
            var idSb = new StringBuilder();
            var invalidCharPattern = new Regex(@"[^a-zA-Z0-9\-_:.]");

            idParts = (from idPart in idParts
                       select invalidCharPattern.Replace(idPart, "_")).ToArray();

            if (!char.IsLetter(idParts[0][0]))
            {
                idSb.Append("id_");
            }

            idSb.Append(string.Join("_", idParts));
            return idSb.ToString();
        }
    }
}