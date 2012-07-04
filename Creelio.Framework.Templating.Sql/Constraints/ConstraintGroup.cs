namespace Creelio.Framework.Templating.Sql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.Core.Extensions;

    public class ConstraintGroup : Constraint
    {
        private List<Constraint> _constraints = null;

        public ConstraintGroup(ConstraintConjunction conjunction)
        {
            Conjunction = conjunction;
        }

        public ConstraintGroup(ConstraintConjunction conjunction, params Constraint[] constraints)
            : this(conjunction)
        {
            Members.AddRange(constraints);
        }

        public ConstraintConjunction Conjunction { get; set; }

        public List<Constraint> Members
        {
            get
            {
                if (_constraints == null)
                {
                    _constraints = new List<Constraint>();
                }

                return _constraints;
            }
        }

        public override bool Equals(Constraint other)
        {
            if (!base.Equals(other))
            {
                return false;
            }

            ConstraintGroup otherAsGroup = other as ConstraintGroup;

            bool isEqual = true;
            0.UpTo(Members.Count, ii => isEqual = Members[ii].Equals(otherAsGroup.Members[ii]));

            return isEqual;
        }

        public override string ToString()
        {
            if (Members.Count > 0)
            {
                var constraintStrings = Members.Select(c => Encapsulate(c, Conjunction))
                                               .Where(s => !string.IsNullOrWhiteSpace(s));

                var conjunction = string.Format(" {0} ", Conjunction.ToString().ToUpper());

                return constraintStrings.ToString(conjunction);
            }
            else
            {
                return string.Empty;
            }
        }

        public void Add(params Constraint[] constraints)
        {
            Members.AddRange(constraints);
        }

        public void AddRange(IEnumerable<Constraint> constraints)
        {
            Members.AddRange(constraints);
        }

        public override object Clone()
        {
            ConstraintGroup clone = new ConstraintGroup(Conjunction);

            foreach (Constraint constraint in Members)
            {
                clone.Add(constraint.Clone() as Constraint);
            }

            return clone;
        }
    }
}