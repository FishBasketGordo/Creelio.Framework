namespace Creelio.Framework.Templating.Templates
{
    using Creelio.Framework.Core.Data;
    using System.Collections.Generic;
    using Microsoft.SqlServer.Management.Smo;

    public class InsertProcedureTemplate : ProcedureTemplate
    {
        public InsertProcedureTemplate(SmoDataProvider dataProvider, string databaseName)
            : base(dataProvider, databaseName)
        {
        }

        protected override string ProcedureName
        {
            get { return string.Format("{0}_INSERT", TableName); }
        }

        protected override void WriteProcedureBody()
        {
            // TODO: Rewrite with InsertStatement class

            ////var values = new Dictionary<string, string>();
            ////foreach (Column column in Table.Columns)
            ////{
            ////    values.Add(column.Name, string.Format("@{0}", column.Name));
            ////}

            ////FormatHelper.WriteInsertStatement(TableName, SchemaName, values);
        }
    }
}