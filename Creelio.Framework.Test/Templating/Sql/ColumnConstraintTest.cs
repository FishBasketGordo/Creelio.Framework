namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using Creelio.Framework.Templating.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ColumnConstraintTest : ValueConstraintTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public override void CannotCreateWithNullColumn()
        {
            var constraint = new ColumnConstraint(null, TestColumns.Normal.Int.Column);
            GC.KeepAlive(constraint);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateWithNullColumn2()
        {
            var constraint = new ColumnConstraint(TestColumns.Normal.Int.Column, null);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public override void CannotCreateWithColumnConstraintTypeMismatch()
        {
            var constraint = new ColumnConstraint(TestColumns.Normal.Int.Column, TestColumns.Normal.String.Column);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        public override void IntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Column.Normal.Int);
        }

        [TestMethod]
        public override void FullyQualifiedIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Column.FullyQualified.Int);
        }

        [TestMethod]
        public override void StringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Column.Normal.String);
        }

        [TestMethod]
        public override void FullyQualifiedStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.Column.FullyQualified.String);
        }

        [TestMethod]
        public override void ShouldEqualSelf()
        {
            TestEqualityToSelf(TestConstraints.Column.Normal.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldEqualClone()
        {
            TestEqualityToClone(TestConstraints.Column.Normal.Int.Constraint);
        }

        public override void ShouldMatchCloneFormat()
        {
            TestEqualityToCloneFormat(TestConstraints.Column.Normal.Int.Constraint);
        }
    }
}