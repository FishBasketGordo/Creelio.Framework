namespace Creelio.Framework.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;

    [DebuggerDisplay("ReleasePath = {ReleasePath}, DebugPath = {DebugPath}")]
    public class ScriptPath : IComparable<ScriptPath>
    {
        private string _releasePath = null;
        
        private string _debugPath = null;

        public ScriptPath(string path)
            : this(path, path)
        {
        }

        public ScriptPath(string releasePath, string debugPath)
        {
            if (string.IsNullOrEmpty(releasePath) && string.IsNullOrEmpty(debugPath))
            {
                throw new ArgumentException("Must provide at least one valid path.");
            }

            ReleasePath = GetAbsolutePath(releasePath, debugPath);
            DebugPath = GetAbsolutePath(debugPath, releasePath);
        }

        public string ReleasePath
        {
            get
            {
                return EnsurePath(ref _releasePath, _debugPath);
            }

            set
            {
                _releasePath = GetAbsolutePath(value, _debugPath);
            }
        }

        public string DebugPath
        {
            get
            {
                return EnsurePath(ref _debugPath, _releasePath);
            }

            set
            {
                _debugPath = GetAbsolutePath(value, _releasePath);
            }
        }

        public string CurrentPath
        {
            get
            {
                if (Debugger.IsAttached)
                {
                    return DebugPath;
                }
                else
                {
                    return ReleasePath;
                }
            }
        }

        public int CompareTo(ScriptPath other)
        {
            return CurrentPath.CompareTo(other.CurrentPath);
        }

        private static string GetAbsolutePath(string path, string altPath)
        {
            string absolutePath;
            if (TryGetAbsolutePath(path, out absolutePath) || TryGetAbsolutePath(altPath, out absolutePath))
            {
                return absolutePath;
            }
            else
            {
                return string.Empty;
            }
        }

        private static bool TryGetAbsolutePath(string path, out string absolutePath)
        {
            if (string.IsNullOrEmpty(path))
            {
                absolutePath = string.Empty;
                return false;
            }
            else
            {
                absolutePath = VirtualPathUtility.ToAbsolute(path);
                return true;
            }
        }

        private static string EnsurePath(ref string path, string altPath)
        {
            if (string.IsNullOrEmpty(path) && !TryGetAbsolutePath(altPath, out path))
            {
                throw new InvalidOperationException("Must set at least one valid path before attempting to retrieve path.");
            }

            return path;
        }        
    }
}