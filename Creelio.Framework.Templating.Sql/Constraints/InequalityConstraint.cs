namespace Creelio.Framework.Templating.Sql
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Extensions;

    public class InequalityConstraint<T> : ValueConstraint<T>
    {
        private static readonly Dictionary<InequalityType, string> OperatorDictionary = null;

        static InequalityConstraint()
        {
            OperatorDictionary = new Dictionary<InequalityType, string>
            {
                { InequalityType.LessThan, "<" },
                { InequalityType.GreaterThan, ">" },
                { InequalityType.LessThanOrEqualTo, "<=" },
                { InequalityType.GreaterThanOrEqualTo, ">=" },
            };
        }

        public InequalityConstraint(Column column, T value, InequalityType inequalityType)
            : base(column, value, false)
        {
            InequalityType = inequalityType;
        }

        public override bool NullValue
        {
            get { return false; }
            set { }
        }

        public InequalityType InequalityType { get; set; }

        public override object Clone()
        {
            var clone = new InequalityConstraint<T>(Column, Value, InequalityType);
            return clone;
        }

        protected override string GetOperatorForValue(T value)
        {
            var op = OperatorDictionary.GetOrReturnDefault(InequalityType);
            op.ThrowIfNull(_ => new ArgumentNullException(string.Format("Unrecognized inequality type: {0}.", InequalityType)));

            return op;
        }
    }
}