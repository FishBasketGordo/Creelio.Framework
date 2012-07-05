namespace Creelio.Framework.Templating.Templates
{
    using System.Collections.Generic;
    using Creelio.Framework.Core.Data;
    using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
    using Creelio.Framework.Templating.FormatHelpers;
    using Microsoft.SqlServer.Management.Smo;

    public abstract class ProcedureTemplate : Template
    {
        private string _schemaName = "dbo";

        public ProcedureTemplate(SmoDataProvider dataProvider, string databaseName)
            : base(dataProvider, databaseName)
        {
        }

        public override string OutputFileName
        {
            get { return string.Format("{0}.sql", ProcedureName); }
        }

        protected abstract string ProcedureName { get; }

        protected virtual string SchemaName
        {
            get { return _schemaName; }
            set { _schemaName = value; }
        }

        protected virtual IEnumerable<Column> Parameters 
        { 
            get { return null; } 
        }
        
        protected string QualifiedProcedureName
        {
            get
            {
                return string.Format("[{0}].[{1}]", SchemaName, ProcedureName);
            }
        }

        public override string TransformText()
        {
            FormatHelper.WriteDisclaimer(Host);
            FormatHelper.WriteUseStatement(Database);
            this.WriteLine();

            WriteCreateDummyProcedure();
            this.WriteLine();

            FormatHelper.BeginWriteStoredProcedure(
                QualifiedProcedureName,
                Parameters,
                StoredProcedureWriteArgs.WriteAlter | StoredProcedureWriteArgs.DefaultParamsToNull);

            this.PushIndent();
            WriteProcedureBody();
            PopIndent();

            FormatHelper.EndWriteStoredProcedure();
            this.Write("GO");

            return this.GenerationEnvironment.ToString();
        }

        protected virtual void WriteCreateDummyProcedure()
        {
            this.WriteLine("IF NOT EXISTS");
            this.WriteLine("(");
            
            this.PushIndent();
            this.WriteLine("SELECT TOP 1 1 FROM INFORMATION_SCHEMA.ROUTINES r");
            this.WriteLine(" WHERE r.ROUTINE_TYPE   = 'PROCEDURE'");
            this.WriteLine("   AND r.ROUTINE_SCHEMA = '{0}'", SchemaName);
            this.WriteLine("   AND r.ROUTIME_NAME   = '{0}'", ProcedureName);
            this.PopIndent();
            
            this.WriteLine(")");
            this.WriteLine("BEGIN");
            
            this.PushIndent();
            this.WriteLine("EXEC sp_executesql N'");
            FormatHelper.BeginWriteStoredProcedure(QualifiedProcedureName);
            
            this.PushIndent();
            this.WriteLine("SELECT 1");
            this.PopIndent();
            
            FormatHelper.EndWriteStoredProcedure();
            this.WriteLine("'");
            this.PopIndent();
            
            this.WriteLine("END");
            this.WriteLine("GO");
        }

        protected abstract void WriteProcedureBody();
    }
}
