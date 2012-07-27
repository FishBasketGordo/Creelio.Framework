namespace Creelio.Framework.Templating.FormatHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Creelio.Framework.Core.Extensions;
    using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
    using Microsoft.SqlServer.Management.Smo;
    using Microsoft.VisualStudio.TextTemplating;

    public class SqlFormatHelper : FormatHelper
    {
        public SqlFormatHelper(TextTransformation textTransformation)
            : base(textTransformation)
        {
        }

        // TODO: Consolidate with SqlStringEscape in ValueConstraint
        public static string FormatSqlString(string unformatted)
        {
            return FormatSqlString(unformatted, false);
        }

        // TODO: Consolidate with SqlStringEscape in ValueConstraint
        public static string FormatSqlString(string unformatted, bool includeQuotes)
        {
            unformatted = (unformatted ?? string.Empty).Replace("'", "''");
            return includeQuotes ? unformatted.ToSingleQuotedString() : unformatted;           
        }

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

        public void BeginWriteStoredProcedure(string sprocName)
        {
            BeginWriteStoredProcedure(sprocName, null);
        }

        public void BeginWriteStoredProcedure(string sprocName, IEnumerable<Column> parameters)
        {
            BeginWriteStoredProcedure(sprocName, parameters, (StoredProcedureWriteArgs)0);
        }

        public void BeginWriteStoredProcedure(string sprocName, StoredProcedureWriteArgs args)
        {
            BeginWriteStoredProcedure(sprocName, null, args);
        }

        public void BeginWriteStoredProcedure(string sprocName, IEnumerable<Column> parameters, StoredProcedureWriteArgs args)
        {
            bool defaultParamsToNull = (args & StoredProcedureWriteArgs.DefaultParamsToNull) == StoredProcedureWriteArgs.DefaultParamsToNull;
            bool writeAlter = (args & StoredProcedureWriteArgs.WriteAlter) == StoredProcedureWriteArgs.WriteAlter;

            ProcessIdentifier(ref sprocName, "Stored procedure name");

            _tt.WriteLine("{0} PROCEDURE {1}", writeAlter ? "ALTER" : "CREATE", sprocName);

            if (parameters != null && parameters.Count() > 0)
            {
                _tt.WriteLine("(");
                _tt.PushIndent();
                _tt.WriteLines(
                    from p in parameters
                    select string.Format(
                        "@{0} {1} {2}",
                        p.Name,
                        GetFormattedDataType(p),
                        defaultParamsToNull ? "= NULL" : string.Empty), 
                    l => string.Format(",{0}", l), 
                    l => string.Format(" {0}", l));
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

        private string ToOverloadedWhereClauseParameter(Column column)
        {
            return string.Format("@{0} = NULL OR {1}", column.Name, ToWhereClauseParameter(column));
        }

        private string ToWhereClauseParameter(Column column)
        {
            return string.Format("[{0}] = @{0}", column.Name);
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

        private string GetFormattedDataType(Column column)
        {
            var sb = new StringBuilder(column.DataType.Name.ToUpper());

            if (column.DataType.Name == "varchar" || column.DataType.Name == "nvarchar")
            {
                if (column.DataType.MaximumLength == -1)
                {
                    sb.Append("(MAX)");
                }
                else if (column.DataType.MaximumLength > 1)
                {
                    sb.AppendFormat("({0})", column.DataType.MaximumLength);
                }
            }

            return sb.ToString();
        }
    }
}