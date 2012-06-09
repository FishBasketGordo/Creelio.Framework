using System;
using System.Collections.Generic;
using Creelio.Framework.Core.Data;
using Creelio.Framework.Core.Extensions.MaybeMonad;
using Creelio.Framework.Templating.Interfaces;
using Creelio.Framework.Templating.Templates;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TextTemplating;
using T4Toolbox;

namespace Creelio.Framework.Templating.Generators
{
    public class CrudGenerator : Generator, ITextTemplateHostProvider
    {
        #region Fields

        private string _databaseName = null;
        private List<ISmoTemplate> _templates = null;

        #endregion

        #region Properties

        public string ConnectionString
        {
            get { return DataProvider.ConnectionString; }
            set { DataProvider.ConnectionString = value; }
        }

        public string DatabaseName 
        {
            get { return _databaseName; }
            set
            {
                value.ThrowIfNullOrWhiteSpace(() => new ArgumentNullException("value", "Database name cannot be null."));
                _databaseName = value;
                Templates = null;
            }
        }

        private SmoDataProvider DataProvider { get; set; }

        private List<ISmoTemplate> Templates
        {
            get
            {
                if (_templates == null)
                {
                    _templates = new List<ISmoTemplate>
                    {
                        new DeleteProcedureTemplate(DataProvider, DatabaseName),
                        new CountProcedureTemplate(DataProvider, DatabaseName),
                    };
                }

                return _templates;
            }
            set
            {
                _templates = value;
            }
        }

        #endregion

        #region Constructors

        public CrudGenerator(string connectionString, string databaseName)
        {
            DataProvider = new SmoDataProvider(connectionString);
            DatabaseName = databaseName;
        }

        #endregion

        #region Methods

        protected override void RunCore()
        {
            foreach (Table table in DataProvider.GetDatabase(DatabaseName).Tables)
            {
                foreach (var template in Templates)
                {
                    template.TableName = table.Name;
                    template.Host = Host;
                    template.RenderToFile();
                }    
            }
        }

        #endregion

        #region ITextTemplateHostProvider

        public ITextTemplatingEngineHost Host { get; set; }

        #endregion
    }
}
