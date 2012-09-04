namespace Creelio.Framework.Test.Templating.Sql
{
    using Creelio.Framework.Templating.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WhereClauseTest
    {
        [TestMethod]
        public void NewShouldMatchEmptyString()
        {
            var whereClause = new WhereClause();
            
            Assert.IsTrue(
                whereClause.ToString() == string.Empty, 
                "New WhereClause did not render empty string.");
        }

        [TestMethod]
        public void SingleConstraintShouldMatchFormat()
        {
            foreach (var test in TestConstraints.AllConstraints)
            {
                var whereClause = new WhereClause();
                whereClause.AddConstraint(test.Constraint);

                var result = whereClause.ToString();

                Assert.AreEqual(
                    string.Format("WHERE ({0})", test.Expected),
                    result);
            }
        }

        [TestMethod]
        public void SingleConstraintWithTableAliasShouldMatchFormat()
        {
            foreach (var test in TestConstraints.AllConstraints)
            {
                var columnBased = test.Constraint as IColumnBasedConstraint;
                if (columnBased == null)
                {
                    continue;
                }

                var alias = "t";

                var whereClause = new WhereClause();
                whereClause.AddConstraint(test.Constraint)
                           .AddTableAlias(columnBased.Column.Table.TableName, alias);

                var result = whereClause.ToString();

                var replaced = GetAliasedTableName(test, columnBased, alias);

                var expected = string.Format("WHERE ({0})", replaced);

                Assert.AreEqual(expected, result);
            } 
        }

        [TestMethod]
        public void MultipleConstraintsWithTableAliasesShouldMatchFormat()
        {
            foreach (var test1 in TestConstraints.AllConstraints)
            {
                foreach (var test2 in TestConstraints.AllConstraints)
                {
                    var columnBased1 = test1.Constraint as IColumnBasedConstraint;
                    var columnBased2 = test2.Constraint as IColumnBasedConstraint;

                    if (columnBased1 == null || columnBased2 == null)
                    {
                        continue;
                    }

                    string alias1;
                    string alias2;

                    if (columnBased1.Column.Table.TableName == columnBased2.Column.Table.TableName)
                    {
                        alias1 = alias2 = "t";
                    }
                    else
                    {
                        alias1 = "t1";
                        alias2 = "t2";
                    }

                    var whereClause = new WhereClause();
                    whereClause.AddConstraint(test1.Constraint)
                               .AddConstraint(test2.Constraint)
                               .AddTableAlias(columnBased1.Column.Table.TableName, alias1)
                               .AddTableAlias(columnBased2.Column.Table.TableName, alias2);

                    var result = whereClause.ToString();

                    var replaced1 = GetAliasedTableName(test1, columnBased1, alias1);

                    var replaced2 = GetAliasedTableName(test2, columnBased2, alias2);

                    var expected = string.Format(
                        "WHERE ({0}) AND ({1})",
                        replaced1,
                        replaced2);

                    Assert.AreEqual(expected, result);
                }
            }
        }

        [TestMethod]
        public void MultipleConstraintsShouldMatchFormat()
        {
            foreach (var test1 in TestConstraints.AllConstraints)
            {
                foreach (var test2 in TestConstraints.AllConstraints)
                {
                    var whereClause = new WhereClause();
                    whereClause.AddConstraint(test1.Constraint)
                               .AddConstraint(test2.Constraint);

                    var result = whereClause.ToString();

                    Assert.AreEqual(
                        string.Format("WHERE ({0}) AND ({1})", test1.Expected, test2.Expected),
                        result);
                }
            }
        }

        private static string GetAliasedTableName(TestConstraints._TestConstraint test, IColumnBasedConstraint columnBased, string alias)
        {
            var replaced = test.Expected.Replace(
                string.Format("[{0}].", columnBased.Column.Table.DatabaseName),
                string.Empty);

            replaced = replaced.Replace(
                string.Format("[{0}].", columnBased.Column.Table.SchemaName),
                string.Empty);

            replaced = replaced.Replace(
                string.Format("[{0}]", columnBased.Column.Table.TableName),
                alias);

            return replaced;
        }
    }
}
