namespace Creelio.Framework.Test.Templating.Sql
{
    using System;
    using Creelio.Framework.Core.Extensions;
    using Creelio.Framework.Templating.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ColumnTest
    {
        [TestMethod]
        public void SameTableNameColumnNameAndTypeShouldBeEqual()
        {
            var col1 = new Column("Column1", "Table1", typeof(int));
            var col2 = new Column("Column1", "Table1", typeof(int));

            Assert.IsTrue(
                col1.ValueEquals(col2),
                "Two columns with the same column name, table name, and type should be equal.");
        }

        [TestMethod]
        public void DifferentColumnNamesShouldNotBeEqual()
        {
            var col1 = new Column("Column1", "Table1", typeof(int));
            var col2 = new Column("Column2", "Table1", typeof(int));

            Assert.IsTrue(
                !col1.ValueEquals(col2),
                "Two columns with different column names should not be equal."); 
        }

        [TestMethod]
        public void DifferentTableNamesShouldNotBeEqual()
        {
            var col1 = new Column("Column1", "Table1", typeof(int));
            var col2 = new Column("Column1", "Table2", typeof(int));

            Assert.IsTrue(
                !col1.ValueEquals(col2),
                "Two columns with different table names should not be equal.");
        }

        [TestMethod]
        public void DifferentTypesShouldNotBeEqual()
        {
            var col1 = new Column("Column1", "Table1", typeof(int));
            var col2 = new Column("Column1", "Table1", typeof(double));

            Assert.IsTrue(
                !col1.ValueEquals(col2),
                "Two columns with different types should not be equal.");
        }

        [TestMethod]
        public void ComparingWithSameColumnNameShouldBeEqual()
        {
            var column = new Column("Column1", "Table1", typeof(int));

            Assert.IsTrue(
                column.CompareTo(column.ColumnName) == 0,
                "A column compared with a column name that matches its own should be equal.");
        }

        [TestMethod]
        public void ComparingWithSameTableAndColumnNameShouldBeEqual()
        {
            var column = new Column("Column1", "Table1", typeof(int));

            Assert.IsTrue(
                column.CompareTo(column.ColumnName, column.TableName) == 0,
                "A column compared with a table name and column name that matches its own should be equal.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithNullColumnName()
        {
            var col = new Column(null, "Table1", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithEmptyColumnName()
        {
            var col = new Column(string.Empty, "Table1", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithWhiteSpaceColumnName()
        {
            var col = new Column(" ", "Table1", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithNullTableName()
        {
            var col = new Column("Column1", null, typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithEmptyTableName()
        {
            var col = new Column("Column1", string.Empty, typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithWhiteSpaceTableName()
        {
            var col = new Column("Column1", " ", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithNullType()
        {
            var col = new Column("Column1", "Table1", null);
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithNullFullyQualifiedColumnName()
        {
            var col = new Column(null, typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithEmptyFullyQualifiedColumnName()
        {
            var col = new Column(string.Empty, typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithWhiteSpaceFullyQualifiedColumnName()
        {
            var col = new Column(" ", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateColumnWithValidFullyQualifiedColumnNameButNullType()
        {
            var col = new Column("Database1.Owner1.Table1.Column1", null);
            GC.KeepAlive(col);
        }

        [TestMethod]
        public void CanCreateColumnWithQualifiedColumnName()
        {
            var col = new Column("Table1.Column1", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        public void CanCreateColumnWithQualifiedAndBracketedColumnName()
        {
            var col = new Column("[Table1].[Column1]", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        public void CanCreateColumnWithFullyQualifiedColumnName()
        {
            var col = new Column("Database1.Owner1.Table1.Column1", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        public void CanCreateColumnWithFullyQualifiedAndBracketedColumnName()
        {
            var col = new Column("[Database1].[Owner1].[Table1].[Column1]", typeof(int));
            GC.KeepAlive(col);
        }

        [TestMethod]
        public void CreatingColumnFromQualifiedColumnNameShouldHaveEmptyDatabaseName()
        {
            var col = new Column("Table1.Column1", typeof(int));
            Assert.AreEqual(string.Empty, col.DatabaseName);
        }

        [TestMethod]
        public void CreatingColumnFromQualifiedColumnNameShouldHaveEmptyOwnerName()
        {
            var col = new Column("Table1.Column1", typeof(int));
            Assert.AreEqual(string.Empty, col.OwnerName);
        }

        [TestMethod]
        public void CreatingColumnFromQualifiedColumnNameShouldHaveCorrectTableName()
        {
            var col = new Column("Table1.Column1", typeof(int));
            Assert.AreEqual("Table1", col.TableName);                
        }

        [TestMethod]
        public void CreatingColumnFromQualifiedColumnNameShouldHaveCorrectColumnName()
        {
            var col = new Column("Table1.Column1", typeof(int));
            Assert.AreEqual("Column1", col.ColumnName);
        }

        [TestMethod]
        public void CreatingColumnFromQualifiedAndBracketedColumnNameShouldHaveEmptyDatabaseName()
        {
            var col = new Column("[Table1].[Column1]", typeof(int));
            Assert.AreEqual(string.Empty, col.DatabaseName);
        }

        [TestMethod]
        public void CreatingColumnFromQualifiedAndBracketedColumnNameShouldHaveEmptyOwnerName()
        {
            var col = new Column("[Table1].[Column1]", typeof(int));
            Assert.AreEqual(string.Empty, col.OwnerName);
        }

        [TestMethod]
        public void CreatingColumnFromQualifiedAndBracketedColumnNameShouldHaveCorrectTableName()
        {
            var col = new Column("[Table1].[Column1]", typeof(int));
            Assert.AreEqual("Table1", col.TableName);
        }

        [TestMethod]
        public void CreatingColumnFromQualifiedAndBracketedColumnNameShouldHaveCorrectColumnName()
        {
            var col = new Column("[Table1].[Column1]", typeof(int));
            Assert.AreEqual("Column1", col.ColumnName);
        }

        [TestMethod]
        public void CreatingColumnFromFullyQualifiedColumnNameShouldHaveCorrectDatabaseName()
        {
            var col = new Column("Database1.Owner1.Table1.Column1", typeof(int));
            Assert.AreEqual("Database1", col.DatabaseName);
        }

        [TestMethod]
        public void CreatingColumnFromFullyQualifiedColumnNameShouldHaveCorrectOwnerName()
        {
            var col = new Column("Database1.Owner1.Table1.Column1", typeof(int));
            Assert.AreEqual("Owner1", col.OwnerName);
        }

        [TestMethod]
        public void CreatingColumnFromFullyQualifiedColumnNameShouldHaveCorrectTableName()
        {
            var col = new Column("Database1.Owner1.Table1.Column1", typeof(int));
            Assert.AreEqual("Table1", col.TableName);
        }

        [TestMethod]
        public void CreatingColumnFromFullyQualifiedColumnNameShouldHaveCorrectColumnName()
        {
            var col = new Column("Database1.Owner1.Table1.Column1", typeof(int));
            Assert.AreEqual("Column1", col.ColumnName);
        }

        [TestMethod]
        public void CreatingColumnFromFullyQualifiedAndBracketedColumnNameShouldHaveCorrectDatabaseName()
        {
            var col = new Column("[Database1].[Owner1].[Table1].[Column1]", typeof(int));
            Assert.AreEqual("Database1", col.DatabaseName);
        }

        [TestMethod]
        public void CreatingColumnFromFullyQualifiedAndBracketedColumnNameShouldHaveCorrectOwnerName()
        {
            var col = new Column("[Database1].[Owner1].[Table1].[Column1]", typeof(int));
            Assert.AreEqual("Owner1", col.OwnerName);
        }

        [TestMethod]
        public void CreatingColumnFromFullyQualifiedAndBracketedColumnNameShouldHaveCorrectTableName()
        {
            var col = new Column("[Database1].[Owner1].[Table1].[Column1]", typeof(int));
            Assert.AreEqual("Table1", col.TableName);
        }

        [TestMethod]
        public void CreatingColumnFromFullyQualifiedAndBracketedColumnNameShouldHaveCorrectColumnName()
        {
            var col = new Column("[Database1].[Owner1].[Table1].[Column1]", typeof(int));
            Assert.AreEqual("Column1", col.ColumnName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToStringWithNullTableAliasShouldThrow()
        {
            var column = new Column("Table1.Column1", typeof(int));
            GC.KeepAlive(column.ToString(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToStringWithEmptyTableAliasShouldThrow()
        {
            var column = new Column("Table1.Column1", typeof(int));
            GC.KeepAlive(column.ToString(string.Empty));
        }

        [TestMethod]
        public void QualifiedColumnToStringShouldBeBracketed()
        {
            var column = new Column("Table1.Column1", typeof(int));
            Assert.AreEqual("[Table1].[Column1]", column.ToString());
        }

        [TestMethod]
        public void QualifiedAndBracketedColumnToStringShouldBeBracketed()
        {
            var column = new Column("[Table1].[Column1]", typeof(int));
            Assert.AreEqual("[Table1].[Column1]", column.ToString());
        }

        [TestMethod]
        public void QualifiedColumnToStringWithTableAliasShouldUseAlias()
        {
            var column = new Column("Table1.Column1", typeof(int));
            Assert.AreEqual("t.[Column1]", column.ToString("t"));
        }

        [TestMethod]
        public void FullyQualifiedColumnToStringShouldBeBracketed()
        {
            var column = new Column("Database1.Owner1.Table1.Column1", typeof(int));
            Assert.AreEqual("[Database1].[Owner1].[Table1].[Column1]", column.ToString());
        }

        [TestMethod]
        public void FullyQualifiedAndBracketedColumnToStringShouldBeBracketed()
        {
            var column = new Column("[Database1].[Owner1].[Table1].[Column1]", typeof(int));
            Assert.AreEqual("[Database1].[Owner1].[Table1].[Column1]", column.ToString());
        }

        [TestMethod]
        public void FullyQualifiedColumnToStringWithTableAliasShouldUseAlias()
        {
            var column = new Column("Database1.Owner1.Table1.Column1", typeof(int));
            Assert.AreEqual("t.[Column1]", column.ToString("t"));
        }
    }
}