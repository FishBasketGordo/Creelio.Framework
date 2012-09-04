namespace Creelio.Framework.Templating.Sql
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework;
    using Creelio.Framework.Extensions;

    public class ListConstraint<TItem> : ValueConstraint<IEnumerable<TItem>>
    {
        private Guard<Column> _columnGuard = null;

        public ListConstraint(Column column, IEnumerable<TItem> value, ListType listType)
            : base(column, value, false)
        {
            ListType = listType;
        }

        public override bool NullValue
        {
            get { return false; }
            set { throw new InvalidOperationException("Cannot set NullValue on a ListConstraint."); }
        }

        public ListType ListType { get; set; }

        protected override Guard<Column> ColumnGuard
        {
            get
            {
                if (_columnGuard == null)
                {
                    _columnGuard = new Guard<Column>()
                        .AddPredicate(
                            c => c != null,
                            c => new ArgumentNullException("Column cannot be null."))
                        .AddPredicate(
                            c => c.Type == typeof(TItem),
                            c => new ArgumentException(string.Format(
                                 "The type of the column '{0}' does not match the type of the constraint '{1}'.",
                                 c.Type,
                                 typeof(TItem))));
                }

                return _columnGuard;
            }
        }

        public override object Clone()
        {
            var clone = new ListConstraint<TItem>(Column, Value, ListType);
            return clone;
        }

        protected override string GetOperatorForValue(IEnumerable<TItem> value)
        {
            if (ListType == ListType.Inclusive)
            {
                return "IN";
            }
            else
            {
                return "NOT IN";
            }
        }

        protected override string ValueToString(IEnumerable<TItem> value)
        {
            string valueString = Value.ToCsv(true, base.ValueToString);
            return Encapsulate(valueString);
        }
    }
}