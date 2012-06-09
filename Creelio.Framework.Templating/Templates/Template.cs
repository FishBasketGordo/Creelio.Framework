using System;
using Creelio.Framework.Core.Data;
using Creelio.Framework.Core.Extensions.MaybeMonad;
using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
using Creelio.Framework.Templating.FormatHelpers;
using Creelio.Framework.Templating.Interfaces;
using Microsoft.VisualStudio.TextTemplating;
using T4Toolbox;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;

namespace Creelio.Framework.Templating.Templates
{
    public abstract class Template : T4Toolbox.Template, ISmoTemplate
    {
        #region Fields

        private string _databaseName = null;
        private string _tableName = null;
        private Database _database = null;
        private Table _table = null;
        private IEnumerable<Column> _primaryKeys = null;
        private SqlFormatHelper _formatHelper = null;

        #endregion

        #region Properties

        protected abstract string ProcedureName { get; }

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

        #endregion

        #region Constructors

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

        #endregion

        #region ISmoTemplate Members

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

                _database = null;
                _table = null;
                _primaryKeys = null;
            }
        }

        public string TableName
        {
            get { return _tableName; }
            set
            {
                value.ThrowIfNullOrWhiteSpace(() => new ArgumentNullException("tableName", "Table name cannot be null."));
                _tableName = value;

                _table = null;
                _primaryKeys = null;
            }
        }

        public string OutputFileName
        {
            get { return string.Format("{0}.sql", ProcedureName); }
        }
        
        public void RenderToFile()
        {
            RenderToFile(OutputFileName);
        }

        #endregion

        #region ITextTemplateHostProvider Members

        public ITextTemplatingEngineHost Host { get; set; }

        #endregion
    }
}