namespace Creelio.Framework.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Creelio.Framework.Core.Extensions;
    using Creelio.Framework.Core.Extensions.MaybeMonad;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    public class SmoDataProvider
    {
        private string _connectionString = null;
        
        public SmoDataProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString
        {
            get 
            { 
                return _connectionString; 
            }

            set
            {
                value.ThrowIfNullOrWhiteSpace(_ => new ArgumentNullException("value", "The connection string cannot be null."));
                _connectionString = value;
                Server = null;
            }
        }

        public Exception LastException { get; private set; }

        private Server Server { get; set; }

        public Server GetServer()
        {
            try
            {
                if (Server == null)
                {
                    var sqlConnection = new SqlConnection(ConnectionString);
                    var serverConnection = new ServerConnection(sqlConnection);

                    Server = new Server(serverConnection);
                }

                return Server;
            }
            catch (Exception ex)
            {
                LastException = ex;
                throw;
            }
        }

        public Database GetDatabase(string databaseName)
        {
            try
            {
                var server = GetServer();

                databaseName
                    .ThrowIfNull(
                        _ => new ArgumentNullException("databaseName"))
                    .ThrowIf(
                        d => !server.Databases.Contains(d),
                        d => new ArgumentException(string.Format("The database '{0}' could not be found.", d)));

                var database = server.Databases[databaseName];

                return database;
            }
            catch (Exception ex)
            {
                LastException = ex;
                throw;
            }
        }

        public Table GetTable(string databaseName, string tableName)
        {
            try
            {
                var database = GetDatabase(databaseName);

                tableName
                    .ThrowIfNull(
                        _ => new ArgumentNullException("tableName"))
                    .ThrowIf(
                        t => !database.Tables.Contains(t),
                        t => new ArgumentException(string.Format("The table '{0}' could not be found in database '{1}'.", t, databaseName)));

                var table = database.Tables[tableName];

                return table;
            }
            catch (Exception ex)
            {
                LastException = ex;
                throw;
            }
        }

        public Column GetColumn(string databaseName, string tableName, string columnName)
        {
            try
            {
                var table = GetTable(databaseName, tableName);

                columnName
                    .ThrowIfNullOrWhiteSpace(
                        _ => new ArgumentNullException("columnName"))
                    .ThrowIf(
                        c => !table.Columns.Contains(c),
                        c => new ArgumentException(string.Format("The column '{0}' could not be found in database '{1}', table '{2}'.", c, databaseName, tableName)));

                var column = table.Columns[columnName];

                return column;
            }
            catch (Exception ex)
            {
                LastException = ex;
                throw;
            }
        }

        public IEnumerable<Column> GetPrimaryKeys(string databaseName, string tableName)
        {
            try
            {
                var table = GetTable(databaseName, tableName);
                return from col in table.Columns.ToList<Column>()
                       where col.InPrimaryKey
                       select col;
            }
            catch (Exception ex)
            {
                LastException = ex;
                throw;
            }
        }

        public bool TryGetServer(out Server server)
        {
            try
            {
                server = GetServer();
                return true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                server = null;
                return false;
            }
        }

        public bool TryGetDatabase(string databaseName, out Database database)
        {
            try
            {
                database = GetDatabase(databaseName);
                return true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                database = null;
                return false;
            }
        }

        public bool TryGetTable(string databaseName, string tableName, out Table table)
        {
            try
            {
                table = GetTable(databaseName, tableName);
                return true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                table = null;
                return false;
            }
        }

        public bool TryGetColumn(string databaseName, string tableName, string columnName, out Column column)
        {
            try
            {
                column = GetColumn(databaseName, tableName, columnName);
                return true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                column = null;
                return false;
            }
        }

        public bool TryGetPrimaryKeys(string databaseName, string tableName, out IEnumerable<Column> primaryKeys)
        {
            try
            {
                primaryKeys = GetPrimaryKeys(databaseName, tableName);
                return true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                primaryKeys = null;
                return false;
            }
        }
    }
}