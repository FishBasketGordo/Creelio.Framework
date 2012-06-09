using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TextTemplating;

namespace Creelio.Framework.Templating.FormatHelpers
{
    public class SqlFormatHelper : FormatHelper
    {
        #region Constructors

        public SqlFormatHelper(TextTransformation textTransformation)
            : base(textTransformation)
        {
        }

        #endregion

        #region Methods

        #region Write Disclaimer

        protected override IEnumerable<string> FormatDisclaimerLines(IEnumerable<string> disclaimerLines)
        {
            int maxLineLength = disclaimerLines.Max(dl => dl.Length);

            var flowerBox = new string('-', maxLineLength + 6);
            var lineFormat = string.Format("-- {{0,-{0}}} --", maxLineLength);

            var formattedLines = (from line in disclaimerLines
                                  select string.Format(lineFormat, line)).ToList();

            formattedLines.Insert(0, flowerBox);
            formattedLines.Add(flowerBox);

            return formattedLines;
        }

        #endregion

        #region WriteUseStatement

        public void WriteUseStatement(Database database)
        {
            WriteUseStatement(database.Name);
        }

        public void WriteUseStatement(string databaseName)
        {
            ProcessIdentifier(ref databaseName, "Database name");

            _tt.WriteLine("USE [{0}]", databaseName);
            _tt.WriteLine("GO");
        }

        #endregion

        #region WriteStoredProcedure

        public void BeginWriteStoredProcedure(string sprocName)
        {
            BeginWriteStoredProcedure(sprocName, null);
        }

        public void BeginWriteStoredProcedure(string sprocName, IEnumerable<Column> parameters)
        {
            BeginWriteStoredProcedure(sprocName, parameters, true);
        }

        public void BeginWriteStoredProcedure(string sprocName, IEnumerable<Column> parameters, bool defaultParamsToNull)
        {
            ProcessIdentifier(ref sprocName, "Stored procedure name");

            _tt.WriteLine("CREATE PROCEDURE {0}", sprocName);

            if (parameters != null && parameters.Count() > 0)
            {
                _tt.WriteLine("(");
                _tt.PushIndent();
                _tt.WriteLines(
                    from p in parameters
                    select string.Format(
                        "@{0} {1} {2}",
                        p.Name,
                        p.DataType.Name.ToUpper(),
                        defaultParamsToNull ? "= NULL" : string.Empty
                    ), 
                    l => string.Format(",{0}", l), 
                    l => string.Format(" {0}", l)
                );
                _tt.PopIndent();
                _tt.WriteLine(")");
            }

            _tt.WriteLine("AS");
            _tt.WriteLine("BEGIN");
        }

        public void EndWriteStoredProcedure()
        {
            _tt.WriteLine("END");
        }

        #endregion

        #region WriteDeleteStatement

        public void WriteDeleteStatement(Table table)
        {
            WriteDeleteStatement(table, null);
        }

        public void WriteDeleteStatement(Table table, IEnumerable<Column> parameters)
        {
            WriteDeleteStatement(table, parameters, true);
        }

        public void WriteDeleteStatement(Table table, IEnumerable<Column> parameters, bool overloaded)
        {
            _tt.WriteLine("DELETE FROM [{0}].[{1}]", table.Owner, table.Name);
            WriteWhereClause(parameters, overloaded);
        }

        #endregion

        #region WriteWhereClause

        public void WriteWhereClause(IEnumerable<Column> parameters)
        {
            WriteWhereClause(parameters, true);
        }

        public void WriteWhereClause(IEnumerable<Column> parameters, bool overloaded)
        {
            if (parameters.Count() > 0)
            {
                var parameterToString = overloaded
                                      ? (Func<Column, string>)ToOverloadedWhereClauseParameter
                                      : (Func<Column, string>)ToWhereClauseParameter;

                _tt.WriteLines(parameters, l => string.Format("  AND ({0})", parameterToString(l)), l => string.Format("WHERE ({0})", parameterToString(l)));
            }
        }

        private string ToOverloadedWhereClauseParameter(Column column)
        {
            return string.Format("@{0} = NULL OR {1}", column.Name, ToWhereClauseParameter(column));
        }

        private string ToWhereClauseParameter(Column column)
        {
            return string.Format("[{0}] = @{0}", column.Name);
        }

        #endregion

        #region WriteVarDeclaration

        public void WriteVarDeclaration(string varName, string varType)
        {
            WriteVarDeclaration(varName, varType, null);
        }

        public void WriteVarDeclaration(string varName, string varType, string defaultValue)
        {
            ProcessVarName(ref varName);
            ProcessSqlDataType(ref varType, ref defaultValue);

            _tt.WriteLine("DECLARE {0} {1}", varName, varType);

            if (defaultValue != null)
            {
                _tt.WriteLine("SET {0} = {1}", varName, defaultValue);
            }
        }

        #endregion

        #region WriteInsertStatement

        public void WriteInsertStatement(string tableName, IEnumerable<Dictionary<string, string>> insertRows)
        {
            WriteInsertStatement(tableName, null, insertRows, null);
        }

        public void WriteInsertStatement(string tableName, string ownerName, IEnumerable<Dictionary<string, string>> insertRows)
        {
            WriteInsertStatement(tableName, ownerName, insertRows, null);
        }

        public void WriteInsertStatement(string tableName, IEnumerable<Dictionary<string, string>> insertRows, Dictionary<string, int> maxLengths)
        {
            WriteInsertStatement(tableName, null, insertRows, maxLengths);
        }

        public void WriteInsertStatement(string tableName, string ownerName, IEnumerable<Dictionary<string, string>> insertRows, Dictionary<string, int> maxFieldWidths)
        {
            ProcessTableName(ref tableName, ownerName);
            ValidateInsertRows(insertRows);

            var prefixFormat = "{0,-10}";
            var fieldSeparator = " ,    ";

            _tt.WriteLine("INSERT INTO {0}", tableName);
            _tt.Write(prefixFormat, "(");

            Dictionary<string, string> fieldFormats;
            WriteInsertColumns(insertRows.First(), fieldSeparator, maxFieldWidths, out fieldFormats);

            _tt.WriteLine(")");

            WriteInsertRows(insertRows, prefixFormat, fieldSeparator, fieldFormats);
        }

        public void WriteInsertStatement(string tableName, Dictionary<string, string> insertRow)
        {
            WriteInsertStatement(tableName, new List<Dictionary<string, string>> { insertRow });
        }

        public void WriteInsertStatement(string tableName, string ownerName, Dictionary<string, string> insertRow)
        {
            WriteInsertStatement(tableName, ownerName, new List<Dictionary<string, string>> { insertRow });
        }

        public void WriteInsertStatement(string tableName, Dictionary<string, string> insertRow, Dictionary<string, int> maxFieldWidths)
        {
            WriteInsertStatement(tableName, new List<Dictionary<string, string>> { insertRow }, maxFieldWidths);
        }

        public void WriteInsertStatement(string tableName, string ownerName, Dictionary<string, string> insertRow, Dictionary<string, int> maxFieldWidths)
        {
            WriteInsertStatement(tableName, ownerName, new List<Dictionary<string, string>> { insertRow }, maxFieldWidths);
        }

        private void WriteInsertColumns(Dictionary<string, string> exampleRow, string fieldSeparator, Dictionary<string, int> maxFieldWidths, out Dictionary<string, string> fieldFormats)
        {
            fieldFormats = new Dictionary<string, string>();

            foreach (var field in exampleRow.Take(exampleRow.Count - 1))
            {
                var fieldName = field.Key;
                var fieldFormat = GetFieldFormat(fieldName, maxFieldWidths);

                fieldFormats.Add(fieldName, fieldFormat);

                _tt.Write(fieldFormat + fieldSeparator, fieldName);
            }

            var lastField = exampleRow.Last();
            var lastFieldName = lastField.Key;
            var lastFieldFormat = GetFieldFormat(lastFieldName, maxFieldWidths);

            fieldFormats.Add(lastFieldName, lastFieldFormat);

            _tt.Write(lastFieldFormat + new string(' ', fieldSeparator.Length), lastFieldName);
        }

        private void WriteInsertRows(IEnumerable<Dictionary<string, string>> insertRows, string prefixFormat, string fieldSeparator, Dictionary<string, string> fieldFormats)
        {
            var rowCount = insertRows.Count();

            _tt.WriteLines(
                insertRows,
                row => FormatInsertRecord(row, fieldFormats, prefixFormat, fieldSeparator),
                frow => FormatInsertRecord(frow, fieldFormats, prefixFormat, fieldSeparator),
                lrow => FormatLastInsertRecord(lrow, fieldFormats, prefixFormat, fieldSeparator),
                true
            );
        }

        private string FormatInsertRecord(Dictionary<string, string> row, Dictionary<string, string> fieldFormats, string prefixFormat, string fieldSeparator)
        {
            var sb = new StringBuilder(FormatLastInsertRecord(row, fieldFormats, prefixFormat, fieldSeparator));
            sb.Append("UNION ALL");

            return sb.ToString();
        }

        private string FormatLastInsertRecord(Dictionary<string, string> row, Dictionary<string, string> fieldFormats, string prefixFormat, string fieldSeparator)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(prefixFormat, "SELECT");

            foreach (var field in row.Take(row.Count - 1))
            {
                var fieldName = field.Key;
                sb.AppendFormat(fieldFormats[fieldName] + fieldSeparator, field.Value);
            }

            var lastField = row.Last();
            var lastFieldName = lastField.Key;
            sb.AppendFormat(fieldFormats[lastFieldName] + new string(' ', fieldSeparator.Length), lastField.Value);

            return sb.ToString();
        }

        #endregion

        #region FormatSqlString

        public string FormatSqlString(string unformatted)
        {
            if (string.IsNullOrEmpty(unformatted))
            {
                return "''";
            }
            else
            {
                return string.Format("'{0}'", unformatted.Replace("'", "''"));
            }
        }

        #endregion

        #region Helpers

        private void ProcessTableName(ref string tableName, string ownerName)
        {
            ProcessIdentifier(ref tableName, "Table name");
            ProcessNullableIdentifier(ref ownerName, "Owner name");

            tableName = ownerName == null
                      ? string.Format("[{0}]", tableName)
                      : string.Format("[{0}].[{1}]", ownerName, tableName);
        }

        private void ProcessVarName(ref string varName)
        {
            ProcessIdentifier(ref varName, "Variable name");

            if (!varName.StartsWith("@"))
            {
                varName = string.Format("@{0}", varName);
            }
        }

        private void ProcessSqlDataType(ref string sqlDataType, ref string defaultValue)
        {
            ProcessIdentifier(ref sqlDataType, "SQL data type");
        }

        private void ValidateInsertRows(IEnumerable<Dictionary<string, string>> insertRows)
        {
            if (insertRows == null)
            {
                throw new ArgumentNullException("insertRows");
            }
            else if (insertRows.Count() < 1)
            {
                throw new ArgumentException("There are no rows to insert.", "insertRows");
            }
        }

        private string GetFieldFormat(string fieldName, Dictionary<string, int> maxFieldWidths)
        {
            if (maxFieldWidths == null || !maxFieldWidths.ContainsKey(fieldName))
            {
                return "{0}";
            }
            else
            {
                var fieldWidth = maxFieldWidths[fieldName];
                return string.Format("{{0,-{0}}}", fieldWidth);
            }
        }

        #endregion

        #endregion
    }
}
