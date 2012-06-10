namespace Creelio.Framework.Templating.Templates
{
    using Creelio.Framework.Core.Data;

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
            WriteLine("-- TODO");
            WriteLine("SELECT 1");
        }
    }
}