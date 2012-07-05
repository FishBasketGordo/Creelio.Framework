namespace Creelio.Framework.Test.Templating.Sql
{
    using Creelio.Framework.Templating.Sql;
    using System.Collections.Generic;

    internal static class TestColumns
    {
        static TestColumns()
        {
            var normal1 = "[Table1].[Column1]";
            var normal2 = "[Table2].[Column2]";

            Normal = new _TestColumnGroup(
                intColumn: new _TestColumn(
                    column: new Column(normal1, typeof(int)),
                    expected: normal1),
                int2Column: new _TestColumn(
                    column: new Column(normal2, typeof(int)),
                    expected: normal2),
                stringColumn: new _TestColumn(
                    column: new Column(normal1, typeof(string)),
                    expected: normal1),
                string2Column: new _TestColumn(
                    column: new Column(normal2, typeof(string)),
                    expected: normal2));

            var fullyQualified1 = "[Database1].[Owner1].[Table1].[Column1]";
            var fullyQualified2 = "[Database2].[Owner2].[Table2].[Column2]";

            FullyQualified = new _TestColumnGroup(
                intColumn: new _TestColumn(
                    column: new Column(fullyQualified1, typeof(int)),
                    expected: fullyQualified1),
                int2Column: new _TestColumn(
                    column: new Column(fullyQualified2, typeof(int)),
                    expected: fullyQualified2),
                stringColumn: new _TestColumn(
                    column: new Column(fullyQualified1, typeof(string)),
                    expected: fullyQualified1),
                string2Column: new _TestColumn(
                    column: new Column(fullyQualified2, typeof(string)),
                    expected: fullyQualified2));
        }

        internal static _TestColumnGroup Normal { get; private set; }

        internal static _TestColumnGroup FullyQualified { get; private set; }

        internal class _TestColumnGroup
        {
            internal _TestColumnGroup(
                _TestColumn intColumn, 
                _TestColumn int2Column, 
                _TestColumn stringColumn, 
                _TestColumn string2Column)
            {
                Int = intColumn;
                Int2 = int2Column;
                String = stringColumn;
                String2 = string2Column;
            }

            internal _TestColumn Int { get; private set; }

            internal _TestColumn Int2 { get; private set; }

            internal _TestColumn String { get; private set; }

            internal _TestColumn String2 { get; private set; }
        }

        internal class _TestColumn
        {
            internal _TestColumn(Column column, string expected)
            {
                Column = column;
                Expected = expected;
            }

            internal Column Column { get; private set; }

            internal string Expected { get; private set; }

            internal string SelectStatement
            {
                get
                {
                    return string.Format("SELECT {0} FROM {1}", Column.ColumnName, Column.Table.TableName);
                }
            }

            internal string FormatExpected(string format)
            {
                return string.Format(format, Expected);
            }

            internal string FormatExpected(string format, params object[] parameters)
            {
                var paramList = new List<object>(parameters);
                paramList.Insert(0, Expected);
                return string.Format(format, paramList.ToArray());
            }
        }
    }
}