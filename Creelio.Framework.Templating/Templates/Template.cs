namespace Creelio.Framework.Templating.Templates
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Core.Data;
    using Creelio.Framework.Core.Extensions.MaybeMonad;
    using Creelio.Framework.Templating.FormatHelpers;
    using Creelio.Framework.Templating.Interfaces;
    using Microsoft.SqlServer.Management.Smo;
    using Microsoft.VisualStudio.TextTemplating;

    public abstract class Template : T4Toolbox.Template, ISmoTemplate
    {
        private string _databaseName = null;
        
        private string _tableName = null;
        
        private Database _database = null;
        
        private Table _table = null;
        
        private IEnumerable<Column> _primaryKeys = null;
        
        private SqlFormatHelper _formatHelper = null;

        public Template(string connectionString, string databaseName)
            : this(new SmoDataProvider(connectionString), databaseName)
        {
        }

        public Template(SmoDataProvider dataProvider, string databaseName)
        {
            dataProvider.ThrowIfNull(() => new ArgumentNullException("dataProvider"));

            DataProvider = dataProvider;
            DatabaseName = databaseName;
        }

        public Template(string connectionString, string databaseName, string tableName)
            : this(new SmoDataProvider(connectionString), databaseName, tableName)
        {
        }

        public Template(SmoDataProvider dataProvider, string databaseName, string tableName)
            : this(dataProvider, databaseName)
        {
            TableName = tableName;
        }

        public abstract string OutputFileName { get; }

        public ITextTemplatingEngineHost Host { get; set; }

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

                _database = null;
                _table = null;
                _primaryKeys = null;
            }
        }

        public string TableName
        {
            get 
            { 
                return _tableName; 
            }
            
            set
            {
                value.ThrowIfNullOrWhiteSpace(() => new ArgumentNullException("tableName", "Table name cannot be null."));
                _tableName = value;

                _table = null;
                _primaryKeys = null;
            }
        }

        public IEnumerable<Column> PrimaryKeys
        {
            get
            {
                if (_primaryKeys == null)
                {
                    _primaryKeys = DataProvider.GetPrimaryKeys(DatabaseName, TableName);
                }

                return _primaryKeys;
            }
        }

        protected Database Database
        {
            get
            {
                if (_database == null)
                {
                    _database = DataProvider.GetDatabase(DatabaseName);
                }

                return _database;
            }
        }

        protected Table Table
        {
            get
            {
                if (_table == null)
                {
                    _table = DataProvider.GetTable(DatabaseName, TableName);
                }

                return _table;
            }
        }

        protected SmoDataProvider DataProvider { get; set; }

        protected SqlFormatHelper FormatHelper
        {
            get
            {
                if (_formatHelper == null)
                {
                    _formatHelper = new SqlFormatHelper(this);
                }

                return _formatHelper;
            }
        }

        public void RenderToFile()
        {
            RenderToFile(OutputFileName);
        }
    }
}