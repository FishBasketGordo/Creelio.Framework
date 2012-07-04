namespace Creelio.Framework.Core.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Creelio.Framework.Core.Extensions.MaybeMonad;

    public static class PageExtensions
    {
        public static void RegisterScript(this Page page, string path)
        {
            page.ThrowIfNull(() => new ArgumentNullException("page"));
            path.ThrowIfNullOrWhiteSpace(() => new ArgumentNullException("path"));

            bool noScript = page.With(p => p.Request)
                                .With(r => r.QueryString)
                                .With(q => q["noscript"])
                                .Return(s => s == "1", false);

            if (!noScript)
            {
                path = page.ResolveUrl(path);
                page.ClientScript.RegisterClientScriptInclude(path, path);
            }
        }

        public static void RegisterScript(this Page page, string debugPath, string releasePath)
        {
            if (!string.IsNullOrEmpty(debugPath) && string.IsNullOrEmpty(releasePath))
            {
                RegisterScript(page, debugPath);
            }
            else if (!string.IsNullOrEmpty(releasePath) && string.IsNullOrEmpty(debugPath))
            {
                RegisterScript(page, releasePath);
            }
            else if (Debugger.IsAttached)
            {
                RegisterScript(page, debugPath);
            }
            else
            {
                RegisterScript(page, releasePath);
            }
        }

        public static string FormatHtmlID(this Page page, params string[] idParts)
        {
            return FormatHtmlID(idParts);
        }

        public static string FormatHtmlID(this MasterPage master, params string[] idParts)
        {
            return FormatHtmlID(idParts);
        }

        public static void BindListControl<T>(this Page page, ListControl ctrl, IEnumerable<T> list)
        {
            BindListControl(page, ctrl, list, "Text", "Value");
        }

        public static void BindListControl<T>(this Page page, ListControl ctrl, IEnumerable<T> list, string dataTextField, string dataValueField)
        {
            ctrl.DataSource = list;
            ctrl.DataTextField = dataTextField;
            ctrl.DataValueField = dataValueField;
            ctrl.DataBind();
        }

        public static T As<T>(this MasterPage master)
            where T : MasterPage
        {
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
            idParts.ThrowIf(idp => idp == null || idp.Length < 1, () => new ArgumentNullException("idParts"));

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