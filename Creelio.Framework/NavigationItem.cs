namespace Creelio.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract(Namespace = "AWD.Framework", Name = "NavigationItem", IsReference = true)]
    public class NavigationItem : IList<NavigationItem>, IComparable<NavigationItem>
    {
        private List<NavigationItem> _childrenInternal = null;

        public NavigationItem()
        {
        }

        public NavigationItem(string text, string url)
        {
            Text = text;
            Url = url;
        }

        [DataMember(Name = "Text")]
        public string Text { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Url")]
        public string Url { get; set; }

        [DataMember(Name = "IconUrl")]
        public string IconUrl { get; set; }

        [DataMember(Name = "Parent")]
        public NavigationItem Parent { get; set; }

        [DataMember(Name = "Children")]
        public IEnumerable<NavigationItem> Children
        {
            get
            {
                return ChildrenInternal;
            }

            set
            {
                Clear();

                if (value != null)
                {
                    foreach (var item in value)
                    {
                        Add(item);
                    }
                }
            }
        }

        protected List<NavigationItem> ChildrenInternal
        {
            get
            {
                if (_childrenInternal == null)
                {
                    _childrenInternal = new List<NavigationItem>();
                }

                return _childrenInternal;
            }

            set
            {
                _childrenInternal = value;
            }
        }

        public int Count
        {
            get
            {
                return ChildrenInternal.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public NavigationItem this[int index]
        {
            get
            {
                return ChildrenInternal[index];
            }

            set
            {
                ChildrenInternal[index] = value;
            }
        }

        public int IndexOf(NavigationItem item)
        {
            return ChildrenInternal.IndexOf(item);
        }

        public void Insert(int index, NavigationItem item)
        {
            ChildrenInternal.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ChildrenInternal.RemoveAt(index);
        }

        public void Add(NavigationItem item)
        {
            item.Parent = this;
            ChildrenInternal.Add(item);
        }

        public void Clear()
        {
            ChildrenInternal.Clear();
        }

        public bool Contains(NavigationItem item)
        {
            return ChildrenInternal.Contains(item);
        }

        public void CopyTo(NavigationItem[] array, int arrayIndex)
        {
            ChildrenInternal.CopyTo(array, arrayIndex);
        }

        public bool Remove(NavigationItem item)
        {
            item.Parent = null;
            return ChildrenInternal.Remove(item);
        }

        public IEnumerator<NavigationItem> GetEnumerator()
        {
            return ChildrenInternal.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ChildrenInternal.GetEnumerator();
        }

        public int CompareTo(NavigationItem other)
        {
            if (other == null)
            {
                return (int)ComparableResult.GreaterThanOther;
            }
            else if (Text == null)
            {
                return other.Text == null
                     ? (int)ComparableResult.EqualToOther
                     : (int)ComparableResult.LessThanOther;
            }
            else
            {
                return Text.CompareTo(other.Text);
            }
        }
    }
}