namespace Creelio.Framework.Templating.Sql
{
    using System;

    public class NonEqualityConstraint<T> : ValueConstraint<T>
    {
        public NonEqualityConstraint(Column column, T value)
            : base(column, value)
        {
        }

        public NonEqualityConstraint(Column column, DBNull value)
            : base(column, value)
        {
        }

        public override object Clone()
        {
            var clone = new NonEqualityConstraint<T>(Column, Value);
            return clone;
        }

        protected override string GetOperatorForValue(T value)
        {
            if (NullValue || IsNull(value))
            {
                return "IS NOT";
            }
            else
            {
                return "<>";
            }
        }
    }
}