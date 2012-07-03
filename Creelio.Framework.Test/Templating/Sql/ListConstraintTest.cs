namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Creelio.Framework.Templating.Sql;
    using System.Collections.Generic;

    [TestClass]
    public class ListConstraintTest : ValueConstraintTest
    {
        private IEnumerable<int> _intList = new List<int> { 0, 1, 2 };

        //private IEnumerable<string> _stringList = new List<string> { "a", "b", "c" };

        [TestMethod]
        public override void ShouldEqualSelf()
        {
            TestEqualityToSelf(TestConstraints.List.Normal.Inclusive.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldEqualClone()
        {
            TestEqualityToClone(TestConstraints.List.Normal.Inclusive.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldMatchCloneFormat()
        {
            TestEqualityToCloneFormat(TestConstraints.List.Normal.Inclusive.Int.Constraint);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SettingNullValueShouldThrow()
        {
            var constraint = new ListConstraint<int>(TestColumns.Normal.Int.Column, _intList, ListType.Inclusive);
            constraint.NullValue = true;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public override void CannotCreateWithNullColumn()
        {
            var constraint = new ListConstraint<int>(null, _intList, ListType.Inclusive);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public override void CannotCreateWithColumnConstraintTypeMismatch()
        {
            var constraint = new ListConstraint<int>(TestColumns.Normal.String.Column, _intList, ListType.Inclusive);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        public override void IntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.Normal.Inclusive.Int);
        }

        [TestMethod]
        public void InclusiveIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.Normal.Inclusive.Int);
        }

        [TestMethod]
        public void ExclusiveIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.Normal.Exclusive.Int);
        }

        [TestMethod]
        public override void FullyQualifiedIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.FullyQualified.Inclusive.Int);
        }

        [TestMethod]
        public void FullyQualifiedInclusiveIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.FullyQualified.Inclusive.Int);
        }

        [TestMethod]
        public void FullyQualifiedExclusiveIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.FullyQualified.Exclusive.Int);
        }

        [TestMethod]
        public override void StringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.Normal.Inclusive.String);
        }

        [TestMethod]
        public void InclusiveStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.Normal.Inclusive.String);
        }

        [TestMethod]
        public void ExclusiveStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.Normal.Exclusive.String);
        }

        [TestMethod]
        public override void FullyQualifiedStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.FullyQualified.Inclusive.String);
        }

        [TestMethod]
        public void FullyQualifiedInclusiveStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.FullyQualified.Inclusive.String);
        }

        [TestMethod]
        public void FullyQualifiedExclusiveStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.List.FullyQualified.Exclusive.String);
        }

        [TestMethod]
        public override void ShouldEscapeSingleQuotesInSqlStrings()
        {
            var constraint = new ListConstraint<string>(
                TestColumns.Normal.String.Column, 
                new List<string> { "Apostrophe'd Value 1", "Apostrophe'd Value 2", "Apostrophe'd Value 3" }, 
                ListType.Inclusive);

            var result = constraint.ToString();

            Assert.AreEqual(TestColumns.Normal.String.FormatExpected("{0} IN ('Apostrophe''d Value 1', 'Apostrophe''d Value 2', 'Apostrophe''d Value 3')"), result);
        }
    }
}