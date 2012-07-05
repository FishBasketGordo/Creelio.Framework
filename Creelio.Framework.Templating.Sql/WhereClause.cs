namespace Creelio.Framework.Templating.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using Creelio.Framework.Core.Extensions;
    using Creelio.Framework.Core.Extensions.MaybeMonad;

    public class WhereClause : ICloneable
    {
        private ConstraintGroup _constraints = null;

        private Dictionary<string, string> _tableNameAliases = null;

        public WhereClause()
        {
        }

        public ConstraintGroup Constraints
        {
            get
            {
                if (_constraints == null)
                {
                    _constraints = new ConstraintGroup(ConstraintConjunction.And);
                    _constraints.Parent = this;
                }

                return _constraints;
            }

            private set
            {
                _constraints = value;
            }
        }

        public Dictionary<string, string> TableAliases
        {
            get
            {
                if (_tableNameAliases == null)
                {
                    _tableNameAliases = new Dictionary<string, string>();
                }

                return _tableNameAliases;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Constraints.Members.Count > 0)
            {
                sb.AppendFormat("WHERE {0}", Constraints.ToString());
            }

            return sb.ToString();
        }

        public WhereClause AddConstraint(Constraint constraint)
        {
            constraint.ThrowIfNull(() => new ArgumentNullException("constraint"));
            constraint.Parent = this;
            Constraints.Add(constraint);
            return this;
        }

        public WhereClause AddTableAlias(string tableName, string alias)
        {
            tableName.ThrowIfNullOrWhiteSpace(() => new ArgumentNullException("tableName"));

            alias.ThrowIfNullOrWhiteSpace(
                    _ => new ArgumentNullException("alias"))
                 .ThrowIf(
                    a => TableAliases.ContainsValue(a) && TableAliases.GetOrReturnDefault(tableName) != a,
                    _ => new ArgumentException("Duplicate table alias.", "alias"));

            TableAliases.AddOrSet(tableName, alias);
            return this;
        }

        public object Clone()
        {
            WhereClause clone = new WhereClause();

            clone.Constraints = Constraints.Clone() as ConstraintGroup;

            foreach (var tableName in TableAliases.Keys)
            {
                clone.TableAliases.Add(tableName, TableAliases[tableName]);
            }

            return clone;
        }
    }
}