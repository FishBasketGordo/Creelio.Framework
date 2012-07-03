namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using Creelio.Framework.Templating.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NonEqualityTest : ValueConstraintTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public override void CannotCreateWithNullColumn()
        {
            var constraint = new NonEqualityConstraint<int>(null, 0);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public override void CannotCreateWithColumnConstraintTypeMismatch()
        {
            var constraint = new NonEqualityConstraint<int>(TestColumns.Normal.String.Column, 0);
            GC.KeepAlive(constraint);
        }

        [TestMethod]
        public override void CreatingWithNullValueShouldBeEqualToCreatingWithDBNull()
        {
            var constraint1 = new NonEqualityConstraint<string>(TestColumns.Normal.String.Column, (string)null);
            var constraint2 = new NonEqualityConstraint<string>(TestColumns.Normal.String.Column, DBNull.Value);

            Assert.IsTrue(constraint1.Equals(constraint2));
        }

        [TestMethod]
        public override void NullValueFormatShouldMatchDBNullFormat()
        {
            var constraint1 = new NonEqualityConstraint<string>(TestColumns.Normal.String.Column, (string)null);
            var constraint2 = new NonEqualityConstraint<string>(TestColumns.Normal.String.Column, DBNull.Value);

            Assert.AreEqual(constraint1.ToString(), constraint2.ToString());
        }

        [TestMethod]
        public override void IntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.NonEquality.Normal.NonNull.Int);
        }

        [TestMethod]
        public override void FullyQualifiedIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.NonEquality.FullyQualified.NonNull.Int);
        }

        [TestMethod]
        public override void StringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.NonEquality.Normal.NonNull.String);
        }

        [TestMethod]
        public override void FullyQualifiedStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.NonEquality.FullyQualified.NonNull.String);
        }

        [TestMethod]
        public override void NullIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.NonEquality.Normal.Null.Int);
        }

        [TestMethod]
        public override void FullyQualifiedNullIntConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.NonEquality.FullyQualified.Null.Int);
        }

        [TestMethod]
        public override void NullStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.NonEquality.Normal.Null.String);
        }

        [TestMethod]
        public override void FullyQualifiedNullStringConstraintShouldMatchFormat()
        {
            TestFormat(TestConstraints.NonEquality.FullyQualified.Null.String);
        }

        [TestMethod]
        public override void ShouldEscapeSingleQuotesInSqlStrings()
        {
            var constraint = new NonEqualityConstraint<string>(TestColumns.Normal.String.Column, "Apostrophe'd Value");
            var result = constraint.ToString();

            Assert.AreEqual(TestColumns.Normal.String.FormatExpected("{0} <> 'Apostrophe''d Value'"), result);
        }

        [TestMethod]
        public override void ShouldEqualSelf()
        {
            TestEqualityToSelf(TestConstraints.NonEquality.Normal.NonNull.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldEqualClone()
        {
            TestEqualityToClone(TestConstraints.NonEquality.Normal.NonNull.Int.Constraint);
        }

        [TestMethod]
        public override void ShouldMatchCloneFormat()
        {
            TestEqualityToCloneFormat(TestConstraints.NonEquality.Normal.NonNull.Int.Constraint);
        }
    }
}
