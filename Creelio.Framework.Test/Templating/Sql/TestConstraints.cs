namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Creelio.Framework.Templating.Sql;

    internal class TestConstraints
    {
        static TestConstraints()
        {
            InitializeTestEqualityConstraints();
            InitializeTestNonEqualityConstraints();
            InitializeTestInequalityConstraints();
            InitializeTestColumnConstraints();
            InitializeTestListConstraints();
            InitializeTestSubSelectConstraints();

            AllConstraints = new List<_TestConstraint>
            {
                Equality.Normal.NonNull.Int,
                Equality.Normal.NonNull.String,
                Equality.Normal.Null.Int,
                Equality.Normal.Null.String,
                Equality.FullyQualified.NonNull.Int,
                Equality.FullyQualified.NonNull.String,
                Equality.FullyQualified.Null.Int,
                Equality.FullyQualified.Null.String,
                NonEquality.Normal.NonNull.Int,
                NonEquality.Normal.NonNull.String,
                NonEquality.Normal.Null.Int,
                NonEquality.Normal.Null.String,
                NonEquality.FullyQualified.NonNull.Int,
                NonEquality.FullyQualified.NonNull.String,
                NonEquality.FullyQualified.Null.Int,
                NonEquality.FullyQualified.Null.String,
                Inequality.Normal.GreaterThan.Int,
                Inequality.Normal.GreaterThan.String,
                Inequality.Normal.GreaterThanOrEqualTo.Int,
                Inequality.Normal.GreaterThanOrEqualTo.String,
                Inequality.Normal.LessThan.Int,
                Inequality.Normal.LessThan.String,
                Inequality.Normal.LessThanOrEqualTo.Int,
                Inequality.Normal.LessThanOrEqualTo.String,
                Inequality.FullyQualified.GreaterThan.Int,
                Inequality.FullyQualified.GreaterThan.String,
                Inequality.FullyQualified.GreaterThanOrEqualTo.Int,
                Inequality.FullyQualified.GreaterThanOrEqualTo.String,
                Inequality.FullyQualified.LessThan.Int,
                Inequality.FullyQualified.LessThan.String,
                Inequality.FullyQualified.LessThanOrEqualTo.Int,
                Inequality.FullyQualified.LessThanOrEqualTo.String,
                Column.Normal.Int,
                Column.Normal.String,
                Column.FullyQualified.Int,
                Column.FullyQualified.String,
                List.Normal.Inclusive.Int,
                List.Normal.Inclusive.String,
                List.Normal.Exclusive.Int,
                List.Normal.Exclusive.String,
                List.FullyQualified.Inclusive.Int,
                List.FullyQualified.Inclusive.String,
                List.FullyQualified.Exclusive.Int,
                List.FullyQualified.Exclusive.String,
                SubSelect.Normal.Inclusive.Int,
                SubSelect.Normal.Inclusive.String,
                SubSelect.Normal.Exclusive.Int,
                SubSelect.Normal.Exclusive.String,
                SubSelect.FullyQualified.Inclusive.Int,
                SubSelect.FullyQualified.Inclusive.String,
                SubSelect.FullyQualified.Exclusive.Int,
                SubSelect.FullyQualified.Exclusive.String,
            };
        }

        internal static IEnumerable<_TestConstraint> AllConstraints { get; private set; }

        internal static _TestConstraintQualifiedGroup<_TestConstraintNullableGroup> Equality { get; private set; }

        internal static _TestConstraintQualifiedGroup<_TestConstraintNullableGroup> NonEquality { get; private set; }

        internal static _TestConstraintQualifiedGroup<_TestConstraintInequalityGroup> Inequality { get; private set; }

        internal static _TestConstraintQualifiedGroup<_TestConstraintTypeGroup> Column { get; private set; }

        internal static _TestConstraintQualifiedGroup<_TestConstraintInclusionGroup> List { get; private set; }

        internal static _TestConstraintQualifiedGroup<_TestConstraintInclusionGroup> SubSelect { get; private set; }

        internal static Constraint GetRandomConstraint()
        {
            var random = new Random();
            int index = random.Next() % AllConstraints.Count();
            return AllConstraints.ElementAt(index).Constraint;
        }

        private static void InitializeTestEqualityConstraints()
        {
            Equality = new _TestConstraintQualifiedGroup<_TestConstraintNullableGroup>(
                normalGroup: new _TestConstraintNullableGroup(
                    nullGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new EqualityConstraint<int>(TestColumns.Normal.Int.Column, DBNull.Value),
                            expected: TestColumns.Normal.Int.FormatExpected("{0} IS NULL")),
                        stringGroup: new _TestConstraint(
                            constraint: new EqualityConstraint<string>(TestColumns.Normal.String.Column, DBNull.Value),
                            expected: TestColumns.Normal.String.FormatExpected("{0} IS NULL"))),
                    nonNullGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new EqualityConstraint<int>(TestColumns.Normal.Int.Column, 0),
                            expected: TestColumns.Normal.Int.FormatExpected("{0} = 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new EqualityConstraint<string>(TestColumns.Normal.String.Column, "Value"),
                            expected: TestColumns.Normal.String.FormatExpected("{0} = 'Value'")))),
                fullyQualifiedGroup: new _TestConstraintNullableGroup(
                    nullGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new EqualityConstraint<int>(TestColumns.FullyQualified.Int.Column, DBNull.Value),
                            expected: TestColumns.FullyQualified.Int.FormatExpected("{0} IS NULL")),
                        stringGroup: new _TestConstraint(
                            constraint: new EqualityConstraint<string>(TestColumns.FullyQualified.String.Column, DBNull.Value),
                            expected: TestColumns.FullyQualified.String.FormatExpected("{0} IS NULL"))),
                    nonNullGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new EqualityConstraint<int>(TestColumns.FullyQualified.Int.Column, 0),
                            expected: TestColumns.FullyQualified.Int.FormatExpected("{0} = 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new EqualityConstraint<string>(TestColumns.FullyQualified.String.Column, "Value"),
                            expected: TestColumns.FullyQualified.String.FormatExpected("{0} = 'Value'")))));
        }

        private static void InitializeTestNonEqualityConstraints()
        {
            NonEquality = new _TestConstraintQualifiedGroup<_TestConstraintNullableGroup>(
                normalGroup: new _TestConstraintNullableGroup(
                    nullGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new NonEqualityConstraint<int>(TestColumns.Normal.Int.Column, DBNull.Value),
                            expected: TestColumns.Normal.Int.FormatExpected("{0} IS NOT NULL")),
                        stringGroup: new _TestConstraint(
                            constraint: new NonEqualityConstraint<string>(TestColumns.Normal.String.Column, DBNull.Value),
                            expected: TestColumns.Normal.String.FormatExpected("{0} IS NOT NULL"))),
                    nonNullGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new NonEqualityConstraint<int>(TestColumns.Normal.Int.Column, 0),
                            expected: TestColumns.Normal.Int.FormatExpected("{0} <> 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new NonEqualityConstraint<string>(TestColumns.Normal.String.Column, "Value"),
                            expected: TestColumns.Normal.String.FormatExpected("{0} <> 'Value'")))),
                fullyQualifiedGroup: new _TestConstraintNullableGroup(
                    nullGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new NonEqualityConstraint<int>(TestColumns.FullyQualified.Int.Column, DBNull.Value),
                            expected: TestColumns.FullyQualified.Int.FormatExpected("{0} IS NOT NULL")),
                        stringGroup: new _TestConstraint(
                            constraint: new NonEqualityConstraint<string>(TestColumns.FullyQualified.String.Column, DBNull.Value),
                            expected: TestColumns.FullyQualified.String.FormatExpected("{0} IS NOT NULL"))),
                    nonNullGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new NonEqualityConstraint<int>(TestColumns.FullyQualified.Int.Column, 0),
                            expected: TestColumns.FullyQualified.Int.FormatExpected("{0} <> 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new NonEqualityConstraint<string>(TestColumns.FullyQualified.String.Column, "Value"),
                            expected: TestColumns.FullyQualified.String.FormatExpected("{0} <> 'Value'")))));
        }

        private static void InitializeTestInequalityConstraints()
        {
            Inequality = new _TestConstraintQualifiedGroup<_TestConstraintInequalityGroup>(
                normalGroup: new _TestConstraintInequalityGroup(
                    greaterThanGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<int>(TestColumns.Normal.Int.Column, 0, InequalityType.GreaterThan),
                            expected: TestColumns.Normal.Int.FormatExpected("{0} > 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<string>(TestColumns.Normal.String.Column, "Value", InequalityType.GreaterThan),
                            expected: TestColumns.Normal.String.FormatExpected("{0} > 'Value'"))),
                    greaterThanOrEqualToGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<int>(TestColumns.Normal.Int.Column, 0, InequalityType.GreaterThanOrEqualTo),
                            expected: TestColumns.Normal.Int.FormatExpected("{0} >= 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<string>(TestColumns.Normal.String.Column, "Value", InequalityType.GreaterThanOrEqualTo),
                            expected: TestColumns.Normal.String.FormatExpected("{0} >= 'Value'"))),
                    lessThanGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<int>(TestColumns.Normal.Int.Column, 0, InequalityType.LessThan),
                            expected: TestColumns.Normal.Int.FormatExpected("{0} < 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<string>(TestColumns.Normal.String.Column, "Value", InequalityType.LessThan),
                            expected: TestColumns.Normal.String.FormatExpected("{0} < 'Value'"))),
                    lessThanOrEqualToGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<int>(TestColumns.Normal.Int.Column, 0, InequalityType.LessThanOrEqualTo),
                            expected: TestColumns.Normal.Int.FormatExpected("{0} <= 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<string>(TestColumns.Normal.String.Column, "Value", InequalityType.LessThanOrEqualTo),
                            expected: TestColumns.Normal.String.FormatExpected("{0} <= 'Value'")))),
                fullyQualifiedGroup: new _TestConstraintInequalityGroup(
                    greaterThanGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<int>(TestColumns.FullyQualified.Int.Column, 0, InequalityType.GreaterThan),
                            expected: TestColumns.FullyQualified.Int.FormatExpected("{0} > 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<string>(TestColumns.FullyQualified.String.Column, "Value", InequalityType.GreaterThan),
                            expected: TestColumns.FullyQualified.String.FormatExpected("{0} > 'Value'"))),
                    greaterThanOrEqualToGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<int>(TestColumns.FullyQualified.Int.Column, 0, InequalityType.GreaterThanOrEqualTo),
                            expected: TestColumns.FullyQualified.Int.FormatExpected("{0} >= 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<string>(TestColumns.FullyQualified.String.Column, "Value", InequalityType.GreaterThanOrEqualTo),
                            expected: TestColumns.FullyQualified.String.FormatExpected("{0} >= 'Value'"))),
                    lessThanGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<int>(TestColumns.FullyQualified.Int.Column, 0, InequalityType.LessThan),
                            expected: TestColumns.FullyQualified.Int.FormatExpected("{0} < 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<string>(TestColumns.FullyQualified.String.Column, "Value", InequalityType.LessThan),
                            expected: TestColumns.FullyQualified.String.FormatExpected("{0} < 'Value'"))),
                    lessThanOrEqualToGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<int>(TestColumns.FullyQualified.Int.Column, 0, InequalityType.LessThanOrEqualTo),
                            expected: TestColumns.FullyQualified.Int.FormatExpected("{0} <= 0")),
                        stringGroup: new _TestConstraint(
                            constraint: new InequalityConstraint<string>(TestColumns.FullyQualified.String.Column, "Value", InequalityType.LessThanOrEqualTo),
                            expected: TestColumns.FullyQualified.String.FormatExpected("{0} <= 'Value'")))));
        }

        private static void InitializeTestColumnConstraints()
        {
            Column = new _TestConstraintQualifiedGroup<_TestConstraintTypeGroup>(
                normalGroup: new _TestConstraintTypeGroup(
                    intGroup: new _TestConstraint(
                        constraint: new ColumnConstraint(TestColumns.Normal.Int.Column, TestColumns.Normal.Int2.Column),
                        expected: TestColumns.Normal.Int.FormatExpected("{0} = {1}", TestColumns.Normal.Int2.Expected)),
                    stringGroup: new _TestConstraint(
                        constraint: new ColumnConstraint(TestColumns.Normal.String.Column, TestColumns.Normal.String2.Column),
                        expected: TestColumns.Normal.String.FormatExpected("{0} = {1}", TestColumns.Normal.String2.Expected))),
                fullyQualifiedGroup: new _TestConstraintTypeGroup(
                    intGroup: new _TestConstraint(
                        constraint: new ColumnConstraint(TestColumns.FullyQualified.Int.Column, TestColumns.FullyQualified.Int2.Column),
                        expected: TestColumns.FullyQualified.Int.FormatExpected("{0} = {1}", TestColumns.FullyQualified.Int2.Expected)),
                    stringGroup: new _TestConstraint(
                        constraint: new ColumnConstraint(TestColumns.FullyQualified.String.Column, TestColumns.FullyQualified.String2.Column),
                        expected: TestColumns.FullyQualified.String.FormatExpected("{0} = {1}", TestColumns.FullyQualified.String2.Expected))));
        }

        private static void InitializeTestListConstraints()
        {
            List = new _TestConstraintQualifiedGroup<_TestConstraintInclusionGroup>(
                normalGroup: new _TestConstraintInclusionGroup(
                    inclusiveGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            new ListConstraint<int>(TestColumns.Normal.Int.Column, new List<int> { 0, 1, 2, 3, 4 }, ListType.Inclusive),
                            TestColumns.Normal.Int.FormatExpected("{0} IN (0, 1, 2, 3, 4)")),
                        stringGroup: new _TestConstraint(
                            new ListConstraint<string>(TestColumns.Normal.String.Column, new List<string> { "a", "b", "c", "d", "e" }, ListType.Inclusive),
                            TestColumns.Normal.String.FormatExpected("{0} IN ('a', 'b', 'c', 'd', 'e')"))),
                    exclusiveGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            new ListConstraint<int>(TestColumns.Normal.Int.Column, new List<int> { 0, 1, 2, 3, 4 }, ListType.Exclusive),
                            TestColumns.Normal.Int.FormatExpected("{0} NOT IN (0, 1, 2, 3, 4)")),
                        stringGroup: new _TestConstraint(
                            new ListConstraint<string>(TestColumns.Normal.String.Column, new List<string> { "a", "b", "c", "d", "e" }, ListType.Exclusive),
                            TestColumns.Normal.String.FormatExpected("{0} NOT IN ('a', 'b', 'c', 'd', 'e')")))),
                fullyQualifiedGroup: new _TestConstraintInclusionGroup(
                    inclusiveGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            new ListConstraint<int>(TestColumns.FullyQualified.Int.Column, new List<int> { 0, 1, 2, 3, 4 }, ListType.Inclusive),
                            TestColumns.FullyQualified.Int.FormatExpected("{0} IN (0, 1, 2, 3, 4)")),
                        stringGroup: new _TestConstraint(
                            new ListConstraint<string>(TestColumns.FullyQualified.String.Column, new List<string> { "a", "b", "c", "d", "e" }, ListType.Inclusive),
                            TestColumns.FullyQualified.String.FormatExpected("{0} IN ('a', 'b', 'c', 'd', 'e')"))),
                    exclusiveGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            new ListConstraint<int>(TestColumns.FullyQualified.Int.Column, new List<int> { 0, 1, 2, 3, 4 }, ListType.Exclusive),
                            TestColumns.FullyQualified.Int.FormatExpected("{0} NOT IN (0, 1, 2, 3, 4)")),
                        stringGroup: new _TestConstraint(
                            new ListConstraint<string>(TestColumns.FullyQualified.String.Column, new List<string> { "a", "b", "c", "d", "e" }, ListType.Exclusive),
                            TestColumns.FullyQualified.String.FormatExpected("{0} NOT IN ('a', 'b', 'c', 'd', 'e')")))));
        }

        private static void InitializeTestSubSelectConstraints()
        {
            // TODO: Finish initializing sub select constraints.
            SubSelect = new _TestConstraintQualifiedGroup<_TestConstraintInclusionGroup>(
                normalGroup: new _TestConstraintInclusionGroup(
                    inclusiveGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            new SubSelectConstraint(TestColumns.Normal.Int.Column, TestColumns.Normal.Int.SelectStatement, ListType.Inclusive), 
                            TestColumns.Normal.Int.FormatExpected("{0} IN ({1})", TestColumns.Normal.Int.SelectStatement)),
                        stringGroup: new _TestConstraint(
                            new SubSelectConstraint(TestColumns.Normal.String.Column, TestColumns.Normal.String.SelectStatement, ListType.Inclusive),
                            TestColumns.Normal.String.FormatExpected("{0} IN ({1})", TestColumns.Normal.String.SelectStatement))),
                    exclusiveGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            new SubSelectConstraint(TestColumns.Normal.Int.Column, TestColumns.Normal.Int.SelectStatement, ListType.Exclusive),
                            TestColumns.Normal.Int.FormatExpected("{0} NOT IN ({1})", TestColumns.Normal.Int.SelectStatement)),
                        stringGroup: new _TestConstraint(
                            new SubSelectConstraint(TestColumns.Normal.String.Column, TestColumns.Normal.String.SelectStatement, ListType.Exclusive),
                            TestColumns.Normal.String.FormatExpected("{0} NOT IN ({1})", TestColumns.Normal.String.SelectStatement)))),
                fullyQualifiedGroup: new _TestConstraintInclusionGroup(
                    inclusiveGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            new SubSelectConstraint(TestColumns.FullyQualified.Int.Column, TestColumns.FullyQualified.Int.SelectStatement, ListType.Inclusive),
                            TestColumns.FullyQualified.Int.FormatExpected("{0} IN ({1})", TestColumns.FullyQualified.Int.SelectStatement)),
                        stringGroup: new _TestConstraint(
                            new SubSelectConstraint(TestColumns.FullyQualified.String.Column, TestColumns.FullyQualified.String.SelectStatement, ListType.Inclusive),
                            TestColumns.FullyQualified.String.FormatExpected("{0} IN ({1})", TestColumns.FullyQualified.String.SelectStatement))),
                    exclusiveGroup: new _TestConstraintTypeGroup(
                        intGroup: new _TestConstraint(
                            new SubSelectConstraint(TestColumns.FullyQualified.Int.Column, TestColumns.FullyQualified.Int.SelectStatement, ListType.Exclusive),
                            TestColumns.FullyQualified.Int.FormatExpected("{0} NOT IN ({1})", TestColumns.FullyQualified.Int.SelectStatement)),
                        stringGroup: new _TestConstraint(
                            new SubSelectConstraint(TestColumns.FullyQualified.String.Column, TestColumns.FullyQualified.String.SelectStatement, ListType.Exclusive),
                            TestColumns.FullyQualified.String.FormatExpected("{0} NOT IN ({1})", TestColumns.FullyQualified.String.SelectStatement)))));
        }

        internal class _TestConstraintQualifiedGroup<TGroup>
        {
            internal _TestConstraintQualifiedGroup(TGroup normalGroup, TGroup fullyQualifiedGroup)
            {
                Normal = normalGroup;
                FullyQualified = fullyQualifiedGroup;
            }

            internal TGroup Normal { get; private set; }

            internal TGroup FullyQualified { get; private set; }
        }

        internal class _TestConstraintNullableGroup
        {
            internal _TestConstraintNullableGroup(
                _TestConstraintTypeGroup nullGroup,
                _TestConstraintTypeGroup nonNullGroup)
            {
                Null = nullGroup;
                NonNull = nonNullGroup;
            }

            internal _TestConstraintTypeGroup Null { get; private set; }

            internal _TestConstraintTypeGroup NonNull { get; private set; }
        }

        internal class _TestConstraintInequalityGroup
        {
            internal _TestConstraintInequalityGroup(
                _TestConstraintTypeGroup greaterThanGroup,
                _TestConstraintTypeGroup greaterThanOrEqualToGroup,
                _TestConstraintTypeGroup lessThanGroup,
                _TestConstraintTypeGroup lessThanOrEqualToGroup)
            {
                GreaterThan = greaterThanGroup;
                GreaterThanOrEqualTo = greaterThanOrEqualToGroup;
                LessThan = lessThanGroup;
                LessThanOrEqualTo = lessThanOrEqualToGroup;
            }

            internal _TestConstraintTypeGroup GreaterThan { get; private set; }

            internal _TestConstraintTypeGroup GreaterThanOrEqualTo { get; private set; }

            internal _TestConstraintTypeGroup LessThan { get; private set; }

            internal _TestConstraintTypeGroup LessThanOrEqualTo { get; private set; }
        }

        internal class _TestConstraintInclusionGroup
        {
            internal _TestConstraintInclusionGroup(
                _TestConstraintTypeGroup inclusiveGroup,
                _TestConstraintTypeGroup exclusiveGroup)
            {
                Inclusive = inclusiveGroup;
                Exclusive = exclusiveGroup;
            }

            internal _TestConstraintTypeGroup Inclusive { get; private set; }

            internal _TestConstraintTypeGroup Exclusive { get; private set; }
        }

        internal class _TestConstraintTypeGroup
        {
            internal _TestConstraintTypeGroup(_TestConstraint intGroup, _TestConstraint stringGroup)
            {
                Int = intGroup;
                String = stringGroup;
            }

            internal _TestConstraint Int { get; private set; }

            internal _TestConstraint String { get; private set; }
        }

        internal class _TestConstraint
        {
            internal _TestConstraint(Constraint constraint, string expected)
            {
                Constraint = constraint;
                Expected = expected;
            }

            internal Constraint Constraint { get; private set; }

            internal string Expected { get; private set; }
        }
    }
}