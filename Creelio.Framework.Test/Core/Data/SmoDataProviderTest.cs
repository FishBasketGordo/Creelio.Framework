namespace Creelio.Framework.Test.Core.Data
{
    using System.Collections.Generic;
    using System.Configuration;
    using Creelio.Framework.Data;
    using Microsoft.SqlServer.Management.Smo;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class SmoDataProviderTest
    {
        private string _connectionString = null;
        private SmoDataProvider _dataProvider = null;

        public SmoDataProviderTest(string connectionStringName, string databaseName)
        {
            ConnectionStringName = connectionStringName;
            DatabaseName = databaseName;
        }

        protected string ConnectionStringName { get; set; }

        protected string DatabaseName { get; set; }

        protected string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }

                return _connectionString;
            }
        }

        protected SmoDataProvider DataProvider
        {
            get
            {
                if (_dataProvider == null)
                {
                    _dataProvider = new SmoDataProvider(ConnectionString);
                }

                return _dataProvider;
            }
        }

        [TestMethod]
        public void CanGetServer()
        {
            var server = DataProvider.GetServer();
            Assert.IsNotNull(server);
        }

        [TestMethod]
        public void CanTryGetServer()
        {
            Server server;
            Assert.IsTrue(DataProvider.TryGetServer(out server));
            Assert.IsNotNull(server, "TryGetServer returned true, but Server object is null.");
        }

        [TestMethod]
        public void CanGetDatabase()
        {
            var database = DataProvider.GetDatabase(DatabaseName);
            Assert.IsNotNull(database);
        }

        [TestMethod]
        public void CanTryGetDatabase()
        {
            Database database;
            Assert.IsTrue(DataProvider.TryGetDatabase(DatabaseName, out database));
            Assert.IsNotNull(database, "TryGetDatabase returned true, but Database object is null.");
        }

        protected void CanGetTable(string tableName)
        {
            var table = DataProvider.GetTable(DatabaseName, tableName);
            Assert.IsNotNull(table);
        }

        protected void CanTryGetTable(string tableName)
        {
            Table table;
            Assert.IsTrue(DataProvider.TryGetTable(DatabaseName, tableName, out table));
            Assert.IsNotNull(table, "TryGetTable returned true, but Table object is null.");
        }

        protected void CanGetColumn(string tableName, string columnName)
        {
            var column = DataProvider.GetColumn(DatabaseName, tableName, columnName);
            Assert.IsNotNull(column);
        }

        protected void CanTryGetColumn(string tableName, string columnName)
        {
            Column column;
            Assert.IsTrue(DataProvider.TryGetColumn(DatabaseName, tableName, columnName, out column));
            Assert.IsNotNull(column, "TryGetColumn returned true, but Column object is null.");
        }

        protected void CanGetPrimaryKeys(string tableName)
        {
            var primaryKeys = DataProvider.GetPrimaryKeys(DatabaseName, tableName);
            Assert.IsNotNull(primaryKeys);
        }

        protected void CanTryGetPrimaryKeys(string tableName)
        {
            IEnumerable<Column> primaryKeys;
            Assert.IsTrue(DataProvider.TryGetPrimaryKeys(DatabaseName, tableName, out primaryKeys));
            Assert.IsNotNull(primaryKeys, "TryGetPrimaryKeys returned true, but Column collection is null.");
        }
    }
}