namespace Creelio.Framework.Templating.Templates
{
    using System.Collections.Generic;
    using Creelio.Framework.Core.Data;
    using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
    using Creelio.Framework.Templating.FormatHelpers;
    using Microsoft.SqlServer.Management.Smo;

    public class DeleteProcedureTemplate : ProcedureTemplate
    {
        public DeleteProcedureTemplate(SmoDataProvider dataProvider, string databaseName)
            : base(dataProvider, databaseName)
        {
        }

        protected override string ProcedureName
        {
            get { return string.Format("{0}_DELETE", TableName); }
        }

        protected override IEnumerable<Column> Parameters
        {
            get
            {
                return PrimaryKeys;
            }
        }

        protected override void WriteProcedureBody()
        {
            FormatHelper.WriteDeleteStatement(Table, PrimaryKeys, false);
        }
    }
}