﻿namespace Creelio.Framework.Templating.Templates
{
    using System.Collections.Generic;
    using Creelio.Framework.Data;
    using Creelio.Framework.Extensions;
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

        protected override IEnumerable<Column> Parameters
        {
            get
            {
                return Table.Columns.ToList<Column>();
            }
        }

        protected override void WriteProcedureBody()
        {
            WriteLine("-- TODO");
            WriteLine("SELECT 1");
        }
    }
}