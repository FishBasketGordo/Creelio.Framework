namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using Creelio.Framework.Templating.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InequalityConstraintTest : ValueConstraintTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public override void CannotCreateWithNullColumn()
        {
            var constraint = new InequalityConstraint<int>(null, 0, InequalityType.GreaterThan);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public override void CannotCreateWithColumnConstraintTypeMismatch()
        {
            var constraint = new InequalityConstraint<int>(TestColumns.Normal.String.Column, 0, InequalityType.GreaterThan);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        public override void IntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.GreaterThan.Int);
        }

        [TestMethod]
        public void GreaterThanIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.GreaterThan.Int);
        }

        [TestMethod]
        public void GreaterThanOrEqualToIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.GreaterThanOrEqualTo.Int);
        }

        [TestMethod]
        public void LessThanIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.LessThan.Int);
        }

        [TestMethod]
        public void LessThanOrEqualToIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.LessThanOrEqualTo.Int);
        }

        [TestMethod]
        public override void FullyQualifiedIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.GreaterThan.Int);
        }

        [TestMethod]
        public void FullyQualifiedGreaterThanIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.GreaterThan.Int);
        }

        [TestMethod]
        public void FullyQualifiedGreaterThanOrEqualToIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.GreaterThanOrEqualTo.Int);
        }

        [TestMethod]
        public void FullyQualifiedLessThanIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.LessThan.Int);
        }

        [TestMethod]
        public void FullyQualifiedLessThanOrEqualToIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.LessThanOrEqualTo.Int);
        }

        [TestMethod]
        public override void StringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.GreaterThan.String);
        }

        [TestMethod]
        public void GreaterThanStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.GreaterThan.String);
        }

        [TestMethod]
        public void GreaterThanOrEqualToStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.GreaterThanOrEqualTo.String);
        }

        [TestMethod]
        public void LessThanStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.LessThan.String);
        }

        [TestMethod]
        public void LessThanOrEqualToStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.Normal.LessThanOrEqualTo.String);
        }

        [TestMethod]
        public override void FullyQualifiedStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.GreaterThan.String);
        }

        [TestMethod]
        public void FullyQualifiedGreaterThanStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.GreaterThan.String);
        }

        [TestMethod]
        public void FullyQualifiedGreaterThanOrEqualToStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.GreaterThanOrEqualTo.String);
        }

        [TestMethod]
        public void FullyQualifiedLessThanStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.LessThan.String);
        }

        [TestMethod]
        public void FullyQualifiedLessThanOrEqualToStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Inequality.FullyQualified.LessThanOrEqualTo.String);
        }

        [TestMethod]
        public override void ShouldEscapeSingleQuotesInSqlStrings()
        {
            var constraint = new InequalityConstraint<string>(TestColumns.Normal.String.Column, "Apostrophe'd Value", InequalityType.GreaterThan);
            var result = constraint.ToString();

            Assert.AreEqual(TestColumns.Normal.String.FormatExpected("{0} > 'Apostrophe''d Value'"), result);
        }

        [TestMethod]
        public override void ShouldEqualSelf()
        {
            TestEqualityToSelf(TestConstraints.Inequality.Normal.GreaterThan.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldEqualClone()
        {
            TestEqualityToClone(TestConstraints.Inequality.Normal.GreaterThan.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldMatchCloneFormat()
        {
            TestEqualityToCloneFormat(TestConstraints.Inequality.Normal.GreaterThan.Int.Constraint);
        }
    }
}