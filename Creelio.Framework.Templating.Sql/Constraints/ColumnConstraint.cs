namespace Creelio.Framework.Templating.Sql
{
    using System;
    using Creelio.Framework.Core.Extensions.MaybeMonad;
    using Creelio.Framework.Core;

    public class ColumnConstraint : EqualityConstraint<Column>
    {
        private Guard<Column> _columnGuard = null;

        private Guard<Column> _valueGuard = null;

        public ColumnConstraint(Column column, Column column2)
            : base(column, column2, false)
        {
        }

        protected override Guard<Column> ColumnGuard
        {
            get
            {
                if (_columnGuard == null)
                {
                    _columnGuard = new Guard<Column>()
                        .ClearPredicates()
                        .AddPredicate(
                            c => c != null,
                            c => new ArgumentNullException("Column cannot be null.")); 
                }

                return _columnGuard;
            }
        }

        protected override Guard<Column> ValueGuard
        {
            get
            {
                if (_valueGuard == null)
                {
                    _valueGuard = new Guard<Column>()
                        .ClearPredicates()
                        .AddPredicate(
                            val => val != null,
                            val => new ArgumentNullException("Value cannot be null."))
                        .AddPredicate(
                            val => Column.Type == val.With(v => v.Type),
                            val => new ArgumentException("Cannot compare columns with different types."));
                }

                return _valueGuard;
            }
        }

        public override object Clone()
        {
            var clone = new ColumnConstraint(Column, Value);
            return clone;
        }

        protected override string ValueToString(Column value)
        {
            return value.ToString();
        }
    }
}