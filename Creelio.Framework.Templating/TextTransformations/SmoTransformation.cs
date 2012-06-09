using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Creelio.Framework.Extensions.MaybeMonad;
using Creelio.Framework.Extensions.IEnumerableEx;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TextTemplating;
using Creelio.Framework.Templating.FormatHelpers;

namespace Creelio.Framework.Templating.TextTransformations
{
    public abstract class SmoTransformation<TRenderArgs> : TextTransformation, IRenderTransformation<TRenderArgs>
        where TRenderArgs : SmoRenderArgs
    {
        #region Fields

        private SqlFormatHelper _sqlFormatter = null;

        #endregion

        #region Properties

        public SqlFormatHelper SqlFormatter
        {
            get
            {
                if (_sqlFormatter == null)
                {
                    _sqlFormatter = new SqlFormatHelper(this);
                }

                return _sqlFormatter;
            }
        }

        private string ConnectionString { get; set; }
        private Server Server { get; set; }

        #endregion

        #region Methods

        #region Rendering

        public abstract string Render(TRenderArgs args);

        #endregion

        #region SMO

        protected Server GetServer(string connectionString)
        {
            if (UseCachedServer(connectionString))
            {
                return Server;
            }
            else
            {
                ValidateConnectionString(connectionString);

                var sqlConnection = new SqlConnection(connectionString);
                var serverConnection = new ServerConnection(sqlConnection);

                Server = new Server(serverConnection);
                ConnectionString = connectionString;

                return Server;
            }
        }

        protected Database GetDatabase(string connectionString, string databaseName)
        {
            var server = GetServer(connectionString);
            return GetDatabase(server, databaseName);
        }

        protected Database GetDatabase(Server server, string databaseName)
        {
            server.ThrowIfNull(() => new ArgumentNullException("server"));
            ValidateDatabaseName(databaseName, server);

            var database = server.Databases[databaseName];
            return database;
        }

        protected Table GetTable(string connectionString, string databaseName, string tableName)
        {
            var database = GetDatabase(connectionString, databaseName);
            return GetTable(database, tableName);
        }

        protected Table GetTable(Database database, string tableName)
        {
            database.ThrowIfNull(() => new ArgumentNullException("database"));
            ValidateTableName(tableName, database);

            var table = database.Tables[tableName];
            return table;
        }

        protected IEnumerable<Column> GetPrimaryKeyColumns(string connectionString, string databaseName, string tableName)
        {
            var table = GetTable(connectionString, databaseName, tableName);
            return GetPrimaryKeyColumns(table);
        }

        protected IEnumerable<Column> GetPrimaryKeyColumns(Table table)
        {
            table.ThrowIfNull(() => new ArgumentNullException("table"));
            return from col in table.Columns.ToList<Column>()
                   where col.InPrimaryKey
                   select col;
        }

        #endregion

        #region Helpers

        private bool UseCachedServer(string connectionString)
        {
            return connectionString == ConnectionString;
        }

        private static void ValidateConnectionString(string connectionString)
        {
            connectionString.ThrowIfNull(() => new ArgumentNullException("connectionString"));
        }

        private static void ValidateDatabaseName(string databaseName, Server server)
        {
            databaseName.ThrowIfNull(() => new ArgumentNullException("databaseName"))
                        .ThrowIf(
                            db => !server.Databases.Contains(db),
                            () => new ArgumentException(
                                string.Format(
                                    "The database '{0}' could not be found.",
                                    databaseName
                                )
                            )
                        );
        }

        private static void ValidateTableName(string tableName, Database database)
        {
            tableName.ThrowIfNull(() => new ArgumentNullException("tableName"))
                     .ThrowIf(
                        tbl => !database.Tables.Contains(tbl),
                        () => new ArgumentException(
                            string.Format(
                                "The table '{0}' could not be found.",
                                tableName
                            )
                        )
                    );
        }

        #endregion

        #endregion
    }
}
