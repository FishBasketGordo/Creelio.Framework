namespace Creelio.Framework.Templating.Generators
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Data;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.Templating.Interfaces;
    using Creelio.Framework.Templating.Templates;
    using Microsoft.SqlServer.Management.Smo;
    using Microsoft.VisualStudio.TextTemplating;
    using T4Toolbox;

    public class CrudGenerator : Generator, ITextTemplateHostProvider
    {
        private string _databaseName = null;

        private List<ISmoTemplate> _templates = null;

        static CrudGenerator()
        {
            TableExemptions = tbl => tbl.Name == "sysdiagrams";

            TemplateExemptions = 
                new Func<Table, ISmoTemplate, bool>(
                    (tbl, tmpl) => tbl.Name.EndsWith("History") && tmpl is DeleteProcedureTemplate)
                .OrElse(
                    (tbl, tmpl) => tbl.Name.EndsWith("History") && tmpl is InsertProcedureTemplate)
                .OrElse(
                    (tbl, tmpl) => tbl.Name.EndsWith("History") && tmpl is UpdateProcedureTemplate);
        }

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

        private static Func<Table, bool> TableExemptions { get; set; }

        private static Func<Table, ISmoTemplate, bool> TemplateExemptions { get; set; }
        
        private SmoDataProvider DataProvider { get; set; }

        private List<ISmoTemplate> Templates
        {
            get
            {
                if (_templates == null)
                {
                    _templates = new List<ISmoTemplate>
                    {
                        new SelectProcedureTemplate(DataProvider, DatabaseName),
                        new InsertProcedureTemplate(DataProvider, DatabaseName),
                        new UpdateProcedureTemplate(DataProvider, DatabaseName),
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

        protected override void RunCore()
        {
            foreach (Table table in DataProvider.GetDatabase(DatabaseName).Tables)
            {
                if (IsTableExempted(table))
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("{0} exempted", table.Name));
                    continue;
                }

                foreach (var template in Templates)
                {
                    if (IsTemplateExempted(table, template))
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("{0} exempted for template {1}", table.Name, template.GetType().Name));
                        continue;
                    }

                    template.TableName = table.Name;
                    template.Host = Host;
                    template.RenderToFile();
                }
            }
        }

        private bool IsTableExempted(Table table)
        {
            if (TableExemptions != null)
            {
                return TableExemptions(table);
            }

            return false;
        }

        private bool IsTemplateExempted(Table table, ISmoTemplate template)
        {
            if (TemplateExemptions != null)
            {
                return TemplateExemptions(table, template);
            }

            return false;
        }
    }
}