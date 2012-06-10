namespace Creelio.Framework.Templating.Templates
{
    using Creelio.Framework.Core.Data;

    public class UpdateProcedureTemplate : ProcedureTemplate
    {
        public UpdateProcedureTemplate(SmoDataProvider dataProvider, string databaseName)
            : base(dataProvider, databaseName)
        {
        }

        protected override string ProcedureName
        {
            get { return string.Format("{0}_UPDATE", TableName); }
        }

        protected override void WriteProcedureBody()
        {
            WriteLine("-- TODO");
            WriteLine("SELECT 1");
        }
    }
}