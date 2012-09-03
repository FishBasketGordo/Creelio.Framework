namespace Creelio.Framework.Templating.Templates
{
    using Creelio.Framework.Data;

    public class CountProcedureTemplate : ProcedureTemplate
    {
        public CountProcedureTemplate(SmoDataProvider dataProvider, string databaseName)
            : base(dataProvider, databaseName)
        {
        }

        protected override string ProcedureName
        {
            get { return string.Format("{0}_COUNT", TableName); }
        }

        protected override void WriteProcedureBody()
        {
            WriteLine("SELECT COUNT(*) FROM {0}", TableName);
        }
    }
}