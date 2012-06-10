namespace Creelio.Framework.Templating.Generators
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Core.Data;
    using Creelio.Framework.Core.Extensions.MaybeMonad;
    using Creelio.Framework.Templating.Interfaces;
    using Creelio.Framework.Templating.Templates;
    using Microsoft.SqlServer.Management.Smo;
    using Microsoft.VisualStudio.TextTemplating;
    using T4Toolbox;

    public class CrudGenerator : Generator, ITextTemplateHostProvider
    {
        private string _databaseName = null;

        private List<ISmoTemplate> _templates = null;

        private List<string> _excludedTables = null;

        public CrudGenerator(string connectionString, string databaseName)
        {
            DataProvider = new SmoDataProvider(connectionString);
            DatabaseName = databaseName;
        }

        public string ConnectionString
        {
            get { return DataProvider.ConnectionString; }
            set { DataProvider.ConnectionString = value; }
        }

        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }

            set
            {
                value.ThrowIfNullOrWhiteSpace(() => new ArgumentNullException("value", "Database name cannot be null."));
                _databaseName = value;
                Templates = null;
            }
        }

        public ITextTemplatingEngineHost Host { get; set; }

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

        private List<string> ExcludedTables
        {
            get
            {
                if (_excludedTables == null)
                {
                    // TODO: Retrieve from config.
                    _excludedTables = new List<string>
                    {
                        "sysdiagrams"
                    };
                }

                return _excludedTables;
            }
        }

        protected override void RunCore()
        {
            foreach (Table table in DataProvider.GetDatabase(DatabaseName).Tables)
            {
                if (ExcludedTables.Contains(table.Name))
                {
                    continue;
                }

                foreach (var template in Templates)
                {
                    // TODO: Move these rules to config?
                    if (table.Name.EndsWith("History") && template is DeleteProcedureTemplate)
                    {
                        continue;
                    }

                    template.TableName = table.Name;
                    template.Host = Host;
                    template.RenderToFile();
                }
            }
        }
    }
}