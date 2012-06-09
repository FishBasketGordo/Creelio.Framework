namespace Creelio.Framework.Templating.Sql
{
    using System;

    public abstract class Constraint : IEquatable<Constraint>, ICloneable
    {
        public Constraint()
        {
        }
                
        public WhereClause Parent { get; set; }

        public abstract object Clone();

        public override bool Equals(object obj)
        {
            var constraint = obj as Constraint;
            if (constraint == null)
            {
                return false;
            }
            else
            {
                return Equals(constraint);
            }
        }

        public virtual bool Equals(Constraint other)
        {
            if (other == null)
            {
                return false;
            }

            if (!other.GetType().Equals(GetType()))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return 1;
        }

        protected static string Encapsulate(Constraint constraint, ConstraintConjunction conjunction)
        {
            if (constraint == null)
            {
                if (conjunction == ConstraintConjunction.And)
                {
                    return "(1=1)";
                }
                else if (conjunction == ConstraintConjunction.Or)
                {
                    return "(1=0)";
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return Encapsulate(constraint.ToString());
            }
        }

        protected static string Encapsulate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            else
            {
                return string.Format("({0})", value);
            }
        }
    }
}