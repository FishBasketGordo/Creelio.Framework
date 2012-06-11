namespace Creelio.Framework.Templating.Templates
{
    using System.Collections.Generic;
    using Creelio.Framework.Core.Data;
    using Creelio.Framework.Core.Extensions.IEnumerableExtensions;
    using Microsoft.SqlServer.Management.Smo;

    public class SelectProcedureTemplate : ProcedureTemplate
    {
        public SelectProcedureTemplate(SmoDataProvider dataProvider, string databaseName)
            : base(dataProvider, databaseName)
        {
        }

        protected override string ProcedureName
        {
            get { return string.Format("{0}_SELECT", TableName); }
        }

        protected override List<Column> Parameters
        {
            get
            {
                return new List<Column>(Table.Columns.ToList<Column>());
            }
        }

        protected override void WriteProcedureBody()
        {
            WriteLine("-- TODO");
            WriteLine("SELECT 1");
        }
    }
}