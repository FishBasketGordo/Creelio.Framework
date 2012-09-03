namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.Templating.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConstraintGroupTest : ConstraintTest
    {
        [TestMethod]
        public override void ShouldEqualSelf()
        {
            var constraintGroup = new ConstraintGroup(ConstraintConjunction.And);
            constraintGroup.Add(TestConstraints.Equality.Normal.NonNull.Int.Constraint);
            constraintGroup.Add(TestConstraints.Equality.Normal.NonNull.String.Constraint);

            TestEqualityToSelf(constraintGroup);
        }

        [TestMethod]
        public override void ShouldEqualClone()
        {
            var constraintGroup = new ConstraintGroup(ConstraintConjunction.And);
            constraintGroup.Add(TestConstraints.Equality.Normal.NonNull.Int.Constraint);
            constraintGroup.Add(TestConstraints.Equality.Normal.NonNull.String.Constraint);

            TestEqualityToClone(constraintGroup);
        }

        [TestMethod]
        public override void ShouldMatchCloneFormat()
        {
            var constraintGroup = new ConstraintGroup(ConstraintConjunction.And);
            constraintGroup.Add(TestConstraints.Equality.Normal.NonNull.Int.Constraint);
            constraintGroup.Add(TestConstraints.Equality.Normal.NonNull.String.Constraint);

            TestEqualityToCloneFormat(constraintGroup);
        }

        [TestMethod]
        public void EmptyContraintGroupShouldBeEmpty()
        {
            var constraintGroup = new ConstraintGroup(ConstraintConjunction.And);
            var result = constraintGroup.ToString();

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void OneConstraintAndGroupShouldMatchFormat()
        {
            TestFormat(
                new ConstraintGroup(ConstraintConjunction.And),
                TestConstraints.Equality.Normal.NonNull.Int);
        }

        [TestMethod]
        public void TwoConstraintAndConstraintShouldMatchFormat()
        {
            TestFormat(
                new ConstraintGroup(ConstraintConjunction.And),
                TestConstraints.Equality.Normal.NonNull.Int,
                TestConstraints.Equality.Normal.NonNull.String);
        }

        [TestMethod]
        public void ThreeConstraintAndConstraintShouldMatchFormat()
        {
            TestFormat(
                new ConstraintGroup(ConstraintConjunction.And),
                TestConstraints.Equality.Normal.NonNull.Int,
                TestConstraints.Equality.Normal.NonNull.String,
                TestConstraints.Column.FullyQualified.Int);
        }

        [TestMethod]
        public void FourConstraintAndConstraintShouldMatchFormat()
        {
            TestFormat(
                new ConstraintGroup(ConstraintConjunction.And),
                TestConstraints.Equality.Normal.NonNull.Int,
                TestConstraints.Equality.Normal.NonNull.String,
                TestConstraints.Column.FullyQualified.Int,
                TestConstraints.Column.FullyQualified.String);
        }

        [TestMethod]
        public void OneConstraintOrGroupShouldMatchFormat()
        {
            TestFormat(
                new ConstraintGroup(ConstraintConjunction.Or),
                TestConstraints.Equality.Normal.NonNull.Int);
        }

        [TestMethod]
        public void TwoConstraintOrConstraintShouldMatchFormat()
        {
            TestFormat(
                new ConstraintGroup(ConstraintConjunction.Or),
                TestConstraints.Equality.Normal.NonNull.Int,
                TestConstraints.Equality.Normal.NonNull.String);
        }

        [TestMethod]
        public void ThreeConstraintOrConstraintShouldMatchFormat()
        {
            TestFormat(
                new ConstraintGroup(ConstraintConjunction.Or),
                TestConstraints.Equality.Normal.NonNull.Int,
                TestConstraints.Equality.Normal.NonNull.String,
                TestConstraints.Column.FullyQualified.Int);
        }

        [TestMethod]
        public void FourConstraintOrConstraintShouldMatchFormat()
        {
            TestFormat(
                new ConstraintGroup(ConstraintConjunction.Or),
                TestConstraints.Equality.Normal.NonNull.Int,
                TestConstraints.Equality.Normal.NonNull.String,
                TestConstraints.Column.FullyQualified.Int,
                TestConstraints.Column.FullyQualified.String);
        }

        [TestMethod]
        public void ComplexGroupShouldMatchFormat()
        {
            var subGroup1 = new ConstraintGroup(ConstraintConjunction.And);
            subGroup1.Add(TestConstraints.Equality.Normal.NonNull.Int.Constraint);
            subGroup1.Add(TestConstraints.Equality.Normal.NonNull.String.Constraint);

            var subGroup2 = new ConstraintGroup(ConstraintConjunction.And);
            subGroup2.Add(TestConstraints.Equality.Normal.Null.Int.Constraint);
            subGroup2.Add(TestConstraints.Equality.Normal.Null.String.Constraint);

            var constraintGroup = new ConstraintGroup(ConstraintConjunction.Or);
            constraintGroup.Add(subGroup1);
            constraintGroup.Add(subGroup2);

            var expected = string.Format(
                "({0}) OR ({1})",
                FormatExpected(ConstraintConjunction.And, TestConstraints.Equality.Normal.NonNull.Int, TestConstraints.Equality.Normal.NonNull.String),
                FormatExpected(ConstraintConjunction.And, TestConstraints.Equality.Normal.Null.Int, TestConstraints.Equality.Normal.Null.String));

            var result = constraintGroup.ToString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CountShouldEqualConstraintsAdded()
        {
            int expectedCount;
            var constraintGroup = GetRandomConstraintGroup(out expectedCount);

            Assert.AreEqual(expectedCount, constraintGroup.Members.Count);
        }

        [TestMethod]
        public void IndexOrderShouldEqualOrderAdded()
        {
            var constraintGroup = new ConstraintGroup(ConstraintConjunction.And);
            var constraints = GetRandomConstraints();

            constraintGroup.AddRange(constraints);

            0.UpTo(constraints.Count() - 1, ii => Assert.AreEqual(constraints.ElementAt(ii), constraintGroup.Members[ii]));
        }

        private static ConstraintGroup GetRandomConstraintGroup(out int expectedCount)
        {
            var constraintGroup = new ConstraintGroup(ConstraintConjunction.And);
            var random = new Random();

            expectedCount = random.Next(0, 100);
            constraintGroup.AddRange(GetRandomConstraints(expectedCount));

            return constraintGroup;
        }

        private static IEnumerable<Constraint> GetRandomConstraints()
        {
            var random = new Random();
            return GetRandomConstraints(random.Next(1, 101));
        }

        private static IEnumerable<Constraint> GetRandomConstraints(int count)
        {
            var constraints = new List<Constraint>();
            0.UpTo(count, ii => constraints.Add(TestConstraints.GetRandomConstraint()));
            return constraints;
        }

        private static string FormatExpected(ConstraintConjunction conjunction, params TestConstraints._TestConstraint[] tests)
        {
            var conjunctionString = conjunction.ToString().ToUpper();
            var expected = (from t in tests select string.Format("({0})", t.Expected))
                           .Aggregate((s1, s2) => string.Format("{1} {0} {2}", conjunctionString, s1, s2));

            return expected;
        }

        private void TestFormat(ConstraintGroup constraintGroup, params TestConstraints._TestConstraint[] tests)
        {
            foreach (var test in tests)
            {
                constraintGroup.Add(test.Constraint);
            }

            var expected = FormatExpected(constraintGroup.Conjunction, tests);
            var result = constraintGroup.ToString();

            Assert.AreEqual(expected, result);
        }
    }
}
