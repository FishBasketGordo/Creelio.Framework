namespace Creelio.Framework.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using Creelio.Framework.Extensions;

    public static class ClientScriptManagerExtensions
    {
        public static void RegisterClientScriptIncludes(this ClientScriptManager scriptManager, ScriptPathCollection scriptPaths, Uri uri)
        {
            RegisterClientScriptIncludes(scriptManager, scriptPaths.GetAllScripts(uri));
        }

        public static void RegisterClientScriptIncludes(this ClientScriptManager scriptManager, IEnumerable<ScriptPath> scriptPaths)
        {
            foreach (var scriptPath in scriptPaths)
            {
                RegisterClientScriptInclude(scriptManager, scriptPath);
            }
        }

        public static void RegisterClientScriptInclude(this ClientScriptManager scriptManager, ScriptPath scriptPath)
        {
            scriptManager.ThrowIfNull(() => new ArgumentNullException("scriptManager"));
            scriptPath.ThrowIfNull(() => new ArgumentNullException("path"));

            string path = scriptPath.CurrentPath;
            if (!scriptManager.IsClientScriptIncludeRegistered(path))
            {
                scriptManager.RegisterClientScriptInclude(path, path);
            }
        }
    }
}