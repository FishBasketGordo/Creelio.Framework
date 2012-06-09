using Creelio.Framework.Core.Data;
using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
using Creelio.Framework.Templating.FormatHelpers;

namespace Creelio.Framework.Templating.Templates
{
    public class CountProcedureTemplate : Template
    {
        #region Properties

        protected override string ProcedureName
        {
            get { return string.Format("{0}_COUNT", TableName); }
        }
        
        #endregion

        #region Constructors

        public CountProcedureTemplate(string connectionString, string databaseName)
            : base(connectionString, databaseName)
        {
        }

        public CountProcedureTemplate(SmoDataProvider dataProvider, string databaseName)
            : base(dataProvider, databaseName)
        {
        }

        public CountProcedureTemplate(string connectionString, string databaseName, string tableName)
            : base(connectionString, databaseName, tableName)
        {
        }

        public CountProcedureTemplate(SmoDataProvider dataProvider, string databaseName, string tableName)
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
            FormatHelper.BeginWriteStoredProcedure(ProcedureName);

            this.PushIndent();
            WriteLine("SELECT COUNT(*) FROM {0}", TableName);
            PopIndent();

            FormatHelper.EndWriteStoredProcedure();

            return this.GenerationEnvironment.ToString();
        }

        #endregion
    }
}
