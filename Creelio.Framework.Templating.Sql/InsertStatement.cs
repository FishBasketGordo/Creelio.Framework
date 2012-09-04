namespace Creelio.Framework.Templating.Sql
{
    using System.Collections.Generic;
    using System.Text;
    using Creelio.Framework.Extensions;

    public class InsertStatement : SqlStatement
    {
        private Table _table = null;
     
        private Dictionary<Column, object> _values = null;

        public InsertStatement()
        {
            Formatters.Add(SqlStatementFormats.SingleLine, FormatSingleLine);
            Formatters.Add(SqlStatementFormats.Block, FormatBlock);
        }

        public Table Table
        {
            get
            {
                if (_table == null)
                {
                    _table = new Table();
                }

                return _table;
            }
        }

        public Dictionary<Column, object> Values
        {
            get
            {
                if (_values == null)
                {
                    _values = new Dictionary<Column, object>();
                }

                return _values;
            }

            private set
            {
                _values = value;
            }
        }

        private string FormatSingleLine()
        {
            if (Table.TableName.IsNullOrWhiteSpace() || Values.IsNullOrEmpty())
            {
                return string.Empty;
            }
            else
            {
                // TODO: Finish INSERT ToString - Need to quote values
                return string.Format(
                    "INSERT INTO [{0}] ({1}) VALUES ({2})",
                    Table.TableName,
                    Values.ToCsv(true, kvp => kvp.Key.ColumnName),
                    Values.ToCsv(true, kvp => kvp.Value.ToString()));
            }
        }

        private string FormatBlock()
        {
            if (Table.TableName.IsNullOrWhiteSpace() || Values.IsNullOrEmpty())
            {
                return string.Empty;
            }
            else
            {
                var sb = new StringBuilder();
                var prefixFormat = "{0,-10}";

                sb.AppendFormat("INSERT INTO {0}", Table.FullyQualifiedTableName);
                sb.AppendFormat(prefixFormat, "(");

                sb.AppendLine(")");

                return sb.ToString();
            }
        }

        ////public void WriteInsertStatement(string tableName, string ownerName, IEnumerable<Dictionary<string, string>> insertRows, Dictionary<string, int> maxFieldWidths)
        ////{
        ////    ProcessTableName(ref tableName, ownerName);
        ////    ValidateInsertRows(insertRows);

        ////    var prefixFormat = "{0,-10}";
        ////    var fieldSeparator = " ,    ";

        ////    _tt.WriteLine("INSERT INTO {0}", tableName);
        ////    _tt.Write(prefixFormat, "(");

        ////    Dictionary<string, string> fieldFormats;
        ////    WriteInsertColumns(insertRows.First(), fieldSeparator, maxFieldWidths, out fieldFormats);

        ////    _tt.WriteLine(")");

        ////    WriteInsertRows(insertRows, prefixFormat, fieldSeparator, fieldFormats);
        ////}

        ////private void WriteInsertColumns(Dictionary<string, string> exampleRow, string fieldSeparator, Dictionary<string, int> maxFieldWidths, out Dictionary<string, string> fieldFormats)
        ////{
        ////    fieldFormats = new Dictionary<string, string>();

        ////    foreach (var field in exampleRow.Take(exampleRow.Count - 1))
        ////    {
        ////        var fieldName = field.Key;
        ////        var fieldFormat = GetFieldFormat(fieldName, maxFieldWidths);

        ////        fieldFormats.Add(fieldName, fieldFormat);

        ////        _tt.Write(fieldFormat + fieldSeparator, fieldName);
        ////    }

        ////    var lastField = exampleRow.Last();
        ////    var lastFieldName = lastField.Key;
        ////    var lastFieldFormat = GetFieldFormat(lastFieldName, maxFieldWidths);

        ////    fieldFormats.Add(lastFieldName, lastFieldFormat);

        ////    _tt.Write(lastFieldFormat + new string(' ', fieldSeparator.Length), lastFieldName);
        ////}

        ////private void WriteInsertRows(IEnumerable<Dictionary<string, string>> insertRows, string prefixFormat, string fieldSeparator, Dictionary<string, string> fieldFormats)
        ////{
        ////    var rowCount = insertRows.Count();

        ////    _tt.WriteLines(
        ////        insertRows,
        ////        row => FormatInsertRecord(row, fieldFormats, prefixFormat, fieldSeparator),
        ////        frow => FormatInsertRecord(frow, fieldFormats, prefixFormat, fieldSeparator),
        ////        lrow => FormatLastInsertRecord(lrow, fieldFormats, prefixFormat, fieldSeparator),
        ////        true);
        ////}

        ////private string FormatInsertRecord(Dictionary<string, string> row, Dictionary<string, string> fieldFormats, string prefixFormat, string fieldSeparator)
        ////{
        ////    var sb = new StringBuilder(FormatLastInsertRecord(row, fieldFormats, prefixFormat, fieldSeparator));
        ////    sb.Append("UNION ALL");

        ////    return sb.ToString();
        ////}

        ////private string FormatLastInsertRecord(Dictionary<string, string> row, Dictionary<string, string> fieldFormats, string prefixFormat, string fieldSeparator)
        ////{
        ////    var sb = new StringBuilder();
        ////    sb.AppendFormat(prefixFormat, "SELECT");

        ////    foreach (var field in row.Take(row.Count - 1))
        ////    {
        ////        var fieldName = field.Key;
        ////        sb.AppendFormat(fieldFormats[fieldName] + fieldSeparator, field.Value);
        ////    }

        ////    var lastField = row.Last();
        ////    var lastFieldName = lastField.Key;
        ////    sb.AppendFormat(fieldFormats[lastFieldName] + new string(' ', fieldSeparator.Length), lastField.Value);

        ////    return sb.ToString();
        ////}

        ////private string GetFieldFormat(string fieldName, Dictionary<string, int> maxFieldWidths)
        ////{
        ////    if (maxFieldWidths == null || !maxFieldWidths.ContainsKey(fieldName))
        ////    {
        ////        return "{0}";
        ////    }
        ////    else
        ////    {
        ////        var fieldWidth = maxFieldWidths[fieldName];
        ////        return string.Format("{{0,-{0}}}", fieldWidth);
        ////    }
        ////}

        ////private void ProcessTableName(ref string tableName, string ownerName)
        ////{
        ////    ProcessIdentifier(ref tableName, "Table name");
        ////    ProcessNullableIdentifier(ref ownerName, "Owner name");

        ////    tableName = ownerName == null
        ////              ? string.Format("[{0}]", tableName)
        ////              : string.Format("[{0}].[{1}]", ownerName, tableName);
        ////}

        ////private void ValidateInsertRows(IEnumerable<Dictionary<string, string>> insertRows)
        ////{
        ////    if (insertRows == null)
        ////    {
        ////        throw new ArgumentNullException("insertRows");
        ////    }
        ////    else if (insertRows.Count() < 1)
        ////    {
        ////        throw new ArgumentException("There are no rows to insert.", "insertRows");
        ////    }
        ////}
    }
}