﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using Creelio.Framework.Core.Extensions.DictionaryExtensions;

namespace Creelio.Framework.Core
{
    /// <summary>
    /// Base class for representing hierarchical, enumerated data.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the Value property of this class. 
    /// Instances of this class will be implicitly castable to this type.
    /// </typeparam>
    /// <typeparam name="TMember">
    /// The type of the children/members of this class to enumerate.
    /// </typeparam>
    public abstract class HierarchicalEnum<T, TMember> : IEnumerable<KeyValuePair<string, TMember>>
    {
        #region Fields

        private Dictionary<string, TMember> _members = null;

        #endregion

        #region Constructors

        protected HierarchicalEnum(T value)
        {
            Value = value;
        }

        protected HierarchicalEnum(Func<HierarchicalEnum<T, TMember>, T> getValue)
        {
            Value = getValue(this);
        }

        #endregion

        #region Properties

        public TMember this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }
                else if (!Members.ContainsKey(key))
                {
                    throw new KeyNotFoundException(string.Format("No \"{0}\" key found", key));
                }
                else
                {
                    return Members[key];
                }
            }
        }

        public Dictionary<string, TMember>.KeyCollection Keys
        {
            get
            {
                return Members.Keys;
            }
        }

        public Dictionary<string, TMember>.ValueCollection Values
        {
            get
            {
                return Members.Values;
            }
        }

        private T Value { get; set; }

        private Dictionary<string, TMember> Members
        {
            get
            {
                if (_members == null)
                {
                    _members = new Dictionary<string, TMember>();

                    PropertyInfo[] properties = this.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        var getMethod = property.GetGetMethod();
                        if (getMethod.IsStatic && property.PropertyType == typeof(TMember) && property.CanRead)
                        {
                            _members.Add(property.Name, (TMember)getMethod.Invoke(null, null));
                        }
                    }

                    FieldInfo[] fields = this.GetType().GetFields();
                    foreach (var field in fields)
                    {
                        if (field.IsStatic && field.FieldType == typeof(TMember) && field.IsPublic)
                        {
                            _members.Add(field.Name, (TMember)field.GetValue(null));
                        }
                    }
                }

                return _members;
            }
        }

        #endregion

        #region Methods

        public static implicit operator T(HierarchicalEnum<T, TMember> expandedEnum)
        {
            return expandedEnum.Value;
        }

        public bool IsKey(string candidateKey)
        {
            if (string.IsNullOrEmpty(candidateKey))
            {
                return false;
            }
            else
            {
                return Members.ContainsKey(candidateKey);
            }
        }

        public bool IsValue(TMember candidateValue)
        {
            return Members.ContainsValue(candidateValue);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,TMember>> Members

        public IEnumerator<KeyValuePair<string, TMember>> GetEnumerator()
        {
            return Members.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Members.GetEnumerator();
        }

        #endregion
    }
}