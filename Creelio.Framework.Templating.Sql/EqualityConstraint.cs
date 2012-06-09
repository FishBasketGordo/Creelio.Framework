namespace Creelio.Framework.Templating.Sql
{
    using System;
    using System.Text;

    public class EqualityConstraint<T> : ValueConstraint<T>
    {
        public EqualityConstraint(Column column, T value)
            : base(column, value)
        {
        }

        public EqualityConstraint(Column column, DBNull value)
            : base(column, value)
        {
        }

        protected EqualityConstraint(Column column, T value, bool allowNullValue)
            : base(column, value, allowNullValue)
        {
        }

        public bool BeginWithWildcard { get; set; }

        public bool EndWithWildcard { get; set; }

        public override object Clone()
        {
            var clone = new EqualityConstraint<T>(Column, Value);
            clone.BeginWithWildcard = this.BeginWithWildcard;
            clone.EndWithWildcard = this.EndWithWildcard;

            return clone;
        }

        protected override string GetOperatorForValue(T value)
        {
            if (BeginWithWildcard || EndWithWildcard)
            {
                return "LIKE";
            }
            else if (NullValue || IsNull(value))
            {
                return "IS";
            }
            else
            {
                return "=";
            }
        }

        protected override string ValueToString(T value)
        {
            if (NullValue || IsNull(value))
            {
                return base.ValueToString(value);
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                if (BeginWithWildcard && !IsNull(Value))
                {
                    sb.Append("'%' + ");
                }

                sb.Append(base.ValueToString(value));

                if (EndWithWildcard && !IsNull(Value))
                {
                    sb.Append(" + '%'");
                }

                return sb.ToString();
            }
        }
    }
}