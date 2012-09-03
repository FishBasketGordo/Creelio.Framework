using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Creelio.Framework.Web.Extensions
{
    public static class ControlExtensions
    {
        public static void IncludeScriptInHead(this Control ctrl, string url)
        {
            Validate(ctrl, url);

            url = ctrl.ResolveUrl(url);
            
            int addAtIndex;
            if (NeedToInclude(ctrl.Page.Header, url, out addAtIndex))
            {
                HtmlGenericControl scriptInclude = CreateScriptInclude(url);                
                ctrl.Page.Header.Controls.AddAt(addAtIndex, scriptInclude);
            }
        }

        private static void Validate(Control ctrl, string url)
        {
            if (ctrl == null || ctrl.Page == null || ctrl.Page.Header == null || ctrl.Page.Header.Controls == null)
                throw new InvalidOperationException("Cannot execute script include. Page components are not initialized.");

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");
        }

        private static bool NeedToInclude(HtmlHead headTag, string url, out int addAtIndex)
        {
            IEnumerable<HtmlGenericControl> headControls = headTag.Controls.OfType<HtmlGenericControl>();

            bool needToInclude = !headControls.Any<HtmlGenericControl>(
                hgc => hgc.TagName == "script" && hgc.Attributes["src"] == url
                );

            // Insert after the last inserted <script> control to preserve dependencies.
            addAtIndex = 0;
            if (headControls.Count() > 0)
            {
                addAtIndex = headTag.Controls.IndexOf(headControls.Last()) + 1;
            }

            return needToInclude;
        }

        private static HtmlGenericControl CreateScriptInclude(string url)
        {
            HtmlGenericControl scriptInclude = new HtmlGenericControl("script");
            scriptInclude.Attributes["type"] = "text/javascript";
            scriptInclude.Attributes["language"] = "javascript";
            scriptInclude.Attributes["src"] = url;
            return scriptInclude;
        }
    }
}