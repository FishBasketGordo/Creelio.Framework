namespace Creelio.Framework.Templating.Sql
{
    using Creelio.Framework.Core;
    using System;

    public class SubSelectConstraint : ValueConstraint<string>
    {
        private Guard<Column> _columnGuard = null;

        public SubSelectConstraint(Column column, string subselectStatement, ListType listType)
            : base(column, subselectStatement, false)
        {
            ListType = listType;
        }

        public override bool NullValue
        {
            get { return false; }
            set { throw new InvalidOperationException("Cannot set NullValue on a SubSelectConstraint."); }
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
                            c => new ArgumentNullException("Column cannot be null."));
                }

                return _columnGuard;
            }
        }

        public override object Clone()
        {
            var clone = new SubSelectConstraint(Column, Value, ListType);
            return clone;
        }

        protected override string GetOperatorForValue(string value)
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

        protected override string ValueToString(string value)
        {
            return Encapsulate(value);
        }
    }
}