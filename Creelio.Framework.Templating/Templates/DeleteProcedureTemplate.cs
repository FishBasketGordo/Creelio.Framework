using Creelio.Framework.Core.Data;
using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
using Creelio.Framework.Templating.FormatHelpers;

namespace Creelio.Framework.Templating.Templates
{
    public class DeleteProcedureTemplate : Template
    {
        #region Properties

        protected override string ProcedureName
        {
            get { return string.Format("{0}_DELETE", TableName); }
        }
        
        #endregion

        #region Constructors

        public DeleteProcedureTemplate(string connectionString, string databaseName)
            : base(connectionString, databaseName)
        {
        }

        public DeleteProcedureTemplate(SmoDataProvider dataProvider, string databaseName)
            : base(dataProvider, databaseName)
        {
        }

        public DeleteProcedureTemplate(string connectionString, string databaseName, string tableName)
            : base(connectionString, databaseName, tableName)
        {
        }

        public DeleteProcedureTemplate(SmoDataProvider dataProvider, string databaseName, string tableName)
            : base(dataProvider, databaseName, tableName)
        {
        }

        #endregion

        #region Methods

        public override string TransformText()
        {
            FormatHelper.WriteDisclaimer(Host);
            FormatHelper.WriteUseStatement(Database);
            this.WriteLine();
            FormatHelper.BeginWriteStoredProcedure(ProcedureName, PrimaryKeys, false);

            this.PushIndent();
            FormatHelper.WriteDeleteStatement(Table, PrimaryKeys, false);
            PopIndent();

            FormatHelper.EndWriteStoredProcedure();

            return this.GenerationEnvironment.ToString();
        }

        #endregion
    }
}
