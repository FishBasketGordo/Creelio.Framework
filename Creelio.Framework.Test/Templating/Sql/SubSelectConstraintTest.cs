namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Creelio.Framework.Templating.Sql;

    [TestClass]
    public class SubSelectConstraintTest : ValueConstraintTest
    {
        [TestMethod]
        public override void ShouldEqualSelf()
        {
            TestEqualityToSelf(TestConstraints.SubSelect.Normal.Inclusive.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldEqualClone()
        {
            TestEqualityToClone(TestConstraints.SubSelect.Normal.Inclusive.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldMatchCloneFormat()
        {
            TestEqualityToCloneFormat(TestConstraints.SubSelect.Normal.Inclusive.Int.Constraint);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SettingNullValueShouldThrow()
        {
            var test = TestColumns.Normal.Int;
            var constraint = new SubSelectConstraint(test.Column, test.SelectStatement, ListType.Inclusive);
            constraint.NullValue = true;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public override void CannotCreateWithNullColumn()
        {
            var test = TestColumns.Normal.Int;
            var constraint = new SubSelectConstraint(null, test.SelectStatement, ListType.Inclusive);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        public override void IntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.Normal.Inclusive.Int);
        }

        [TestMethod]
        public void InclusiveIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.Normal.Inclusive.Int);
        }

        [TestMethod]
        public void ExclusiveIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.Normal.Exclusive.Int);
        }

        [TestMethod]
        public override void FullyQualifiedIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.FullyQualified.Inclusive.Int);
        }

        [TestMethod]
        public void FullyQualifiedInclusiveIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.FullyQualified.Inclusive.Int);
        }

        [TestMethod]
        public void FullyQualifiedExclusiveIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.FullyQualified.Exclusive.Int);
        }

        [TestMethod]
        public override void StringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.Normal.Inclusive.String);
        }

        [TestMethod]
        public void InclusiveStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.Normal.Inclusive.String);
        }

        [TestMethod]
        public void ExclusiveStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.Normal.Exclusive.String);
        }

        [TestMethod]
        public override void FullyQualifiedStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.FullyQualified.Inclusive.String);
        }

        [TestMethod]
        public void FullyQualifiedInclusiveStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.FullyQualified.Inclusive.String);
        }

        [TestMethod]
        public void FullyQualifiedExclusiveStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.SubSelect.FullyQualified.Exclusive.String);
        }
    }
}
