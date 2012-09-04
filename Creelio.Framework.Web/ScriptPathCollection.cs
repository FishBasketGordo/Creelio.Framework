namespace Creelio.Framework.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.Extensions;

    [Serializable]
    public class ScriptPathCollection : ICollection<ScriptPath>
    {
        private static readonly object BaseSync = new object();

        private static readonly object PageSpecificSync = new object();

        private static List<ScriptPath> _baseScripts = null;

        private static Dictionary<string, List<ScriptPath>> _pageSpecificScripts = null;
                
        public int Count
        {
            get
            {
                lock (BaseSync)
                {
                    return BaseScripts.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        private static List<ScriptPath> BaseScripts
        {
            get
            {
                if (_baseScripts == null)
                {
                    _baseScripts = new List<ScriptPath>();
                }

                return _baseScripts;
            }
        }

        private static Dictionary<string, List<ScriptPath>> PageSpecificScripts
        {
            get
            {
                if (_pageSpecificScripts == null)
                {
                    _pageSpecificScripts = new Dictionary<string, List<ScriptPath>>();
                }

                return _pageSpecificScripts;
            }
        }

        public void Add(string path)
        {
            Add(new ScriptPath(path));
        }

        public void Add(string path, Uri uri)
        {
            Add(new ScriptPath(path), uri);
        }

        public void Add(ScriptPath path, Uri uri)
        {
            path.ThrowIfNull(() => new ArgumentNullException("path"));

            var key = GetPageKey(uri);

            lock (PageSpecificSync)
            {
                var pageScripts = PageSpecificScripts.GetOrAddDefault(key);
                pageScripts.DoIf(s => !BaseScripts.Contains(path), s => s.Add(path));
            }
        }

        public void Clear(Uri uri)
        {
            var key = GetPageKey(uri);

            lock (PageSpecificSync)
            {
                PageSpecificScripts.GetOrReturnDefault(key).Clear();
            }
        }

        public bool Contains(ScriptPath item, Uri uri)
        {
            var key = GetPageKey(uri);

            lock (PageSpecificSync)
            {
                return Contains(item) || PageSpecificScripts.GetOrReturnDefault(key).Contains(item);
            }
        }

        public void CopyTo(ScriptPath[] array, int arrayIndex, Uri uri)
        {
            GetAllScripts(uri).ToList().CopyTo(array, arrayIndex);
        }

        public bool Remove(ScriptPath item, Uri uri)
        {
            var key = GetPageKey(uri);

            lock (PageSpecificSync)
            {
                return Remove(item) || PageSpecificScripts.GetOrReturnDefault(key).Remove(item);
            }
        }

        public IEnumerator<ScriptPath> GetEnumerator(Uri uri)
        {
            var key = GetPageKey(uri);

            lock (BaseSync)
            {
                foreach (var item in BaseScripts)
                {
                    yield return item;
                }
            }

            lock (PageSpecificSync)
            {
                foreach (var item in PageSpecificScripts.GetOrReturnDefault(key))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<ScriptPath> GetAllScripts(Uri uri)
        {
            return GetBaseScripts().Concat(GetPageSpecificScripts(uri));
        }

        public IEnumerable<ScriptPath> GetBaseScripts()
        {
            lock (BaseSync)
            {
                return BaseScripts.Skip(0);
            }
        }

        public IEnumerable<ScriptPath> GetPageSpecificScripts(Uri uri)
        {
            var key = GetPageKey(uri);

            lock (PageSpecificSync)
            {
                return PageSpecificScripts.GetOrReturnDefault(key).Skip(0);
            }
        }

        public void Add(ScriptPath item)
        {
            item.ThrowIfNull(() => new ArgumentNullException("path"));

            lock (BaseSync)
            {
                BaseScripts.Add(item);
            }
        }

        public void Clear()
        {
            lock (BaseSync)
            {
                BaseScripts.Clear();
            }

            lock (PageSpecificSync)
            {
                PageSpecificScripts.Clear();
            }
        }

        public bool Contains(ScriptPath item)
        {
            lock (BaseSync)
            {
                return BaseScripts.Contains(item);
            }
        }

        public void CopyTo(ScriptPath[] array, int arrayIndex)
        {
            lock (BaseSync)
            {
                BaseScripts.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(ScriptPath item)
        {
            lock (BaseSync)
            {
                return BaseScripts.Remove(item);
            }
        }

        public IEnumerator<ScriptPath> GetEnumerator()
        {
            return BaseScripts.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static string GetPageKey(Uri uri)
        {
            uri.ThrowIfNull(() => new ArgumentNullException("uri"));
            return uri.AbsolutePath;
        }
    }
}