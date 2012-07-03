namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using Creelio.Framework.Templating.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EqualityConstraintTest : ValueConstraintTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public override void CannotCreateWithNullColumn()
        {
            var constraint = new EqualityConstraint<int>(null, 0);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public override void CannotCreateWithColumnConstraintTypeMismatch()
        {
            var constraint = new EqualityConstraint<int>(TestColumns.Normal.String.Column, 0);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        public override void CreatingWithNullValueShouldBeEqualToCreatingWithDBNull()
        {
            var constraint1 = new EqualityConstraint<string>(TestColumns.Normal.String.Column, (string)null);
            var constraint2 = new EqualityConstraint<string>(TestColumns.Normal.String.Column, DBNull.Value);

            Assert.IsTrue(constraint1.Equals(constraint2));
        }

        [TestMethod]
        public override void NullValueFormatShouldMatchDBNullFormat()
        {
            var constraint1 = new EqualityConstraint<string>(TestColumns.Normal.String.Column, (string)null);
            var constraint2 = new EqualityConstraint<string>(TestColumns.Normal.String.Column, DBNull.Value);

            Assert.AreEqual(constraint1.ToString(), constraint2.ToString());
        }

        [TestMethod]
        public override void IntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Equality.Normal.NonNull.Int);
        }

        [TestMethod]
        public override void FullyQualifiedIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Equality.FullyQualified.NonNull.Int);
        }

        [TestMethod]
        public override void StringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Equality.Normal.NonNull.String);
        }

        [TestMethod]
        public override void FullyQualifiedStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Equality.FullyQualified.NonNull.String);
        }

        [TestMethod]
        public override void NullIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Equality.Normal.Null.Int);
        }

        [TestMethod]
        public override void FullyQualifiedNullIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Equality.FullyQualified.Null.Int);
        }

        [TestMethod]
        public override void NullStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Equality.Normal.Null.String);
        }

        [TestMethod]
        public override void FullyQualifiedNullStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Equality.FullyQualified.Null.String);
        }

        [TestMethod]
        public override void ShouldEscapeSingleQuotesInSqlStrings()
        {
            var constraint = new EqualityConstraint<string>(TestColumns.Normal.String.Column, "Apostrophe'd Value");
            var result = constraint.ToString();

            Assert.AreEqual(TestColumns.Normal.String.FormatExpected("{0} = 'Apostrophe''d Value'"), result);
        }

        [TestMethod]
        public void ConstraintWithStartingWildcardShouldMatchFormat()
        {
            var constraint = new EqualityConstraint<string>(TestColumns.Normal.String.Column, "Value")
            {
                BeginWithWildcard = true
            };

            var result = constraint.ToString();

            Assert.AreEqual(TestColumns.Normal.String.FormatExpected("{0} LIKE '%' + 'Value'"), result);
        }

        [TestMethod]
        public void ConstraintWithEndingWildcardShouldMatchFormat()
        {
            var constraint = new EqualityConstraint<string>(TestColumns.Normal.String.Column, "Value")
            {
                EndWithWildcard = true
            };

            var result = constraint.ToString();

            Assert.AreEqual(TestColumns.Normal.String.FormatExpected("{0} LIKE 'Value' + '%'"), result);
        }

        [TestMethod]
        public void ConstraintWithStartingAndEndingWildcardShouldMatchFormat()
        {
            var constraint = new EqualityConstraint<string>(TestColumns.Normal.String.Column, "Value")
            {
                BeginWithWildcard = true,
                EndWithWildcard = true
            };

            var result = constraint.ToString();

            Assert.AreEqual(TestColumns.Normal.String.FormatExpected("{0} LIKE '%' + 'Value' + '%'"), result);
        }

        [TestMethod]
        public override void ShouldEqualSelf()
        {
            TestEqualityToSelf(TestConstraints.Equality.Normal.NonNull.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldEqualClone()
        {
            TestEqualityToClone(TestConstraints.Equality.Normal.NonNull.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldMatchCloneFormat()
        {
            TestEqualityToCloneFormat(TestConstraints.Equality.Normal.NonNull.Int.Constraint);
        }        
    }
}