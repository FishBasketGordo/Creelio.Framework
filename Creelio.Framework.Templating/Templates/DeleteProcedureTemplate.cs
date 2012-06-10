namespace Creelio.Framework.Templating.Templates
{
    using Creelio.Framework.Core.Data;
    using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
    using Creelio.Framework.Templating.FormatHelpers;

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

       protected override void WriteProcedureBody()
        {
            FormatHelper.WriteDeleteStatement(Table, PrimaryKeys, false);
        }
    }
}