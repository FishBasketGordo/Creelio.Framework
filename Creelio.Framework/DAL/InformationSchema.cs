using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace Creelio.Framework.DAL
{
    public class InformationSchema : IDisposable
    {
        #region Fields

        private DataTable _tables = null;
        private DataTable _columns = null;
        private DataTable _primaryKeys = null;
        private DataTable _identities = null;

        private SqlDataAdapter _tableDataAdapter = null;
        private SqlDataAdapter _columnDataAdapter = null;
        private SqlDataAdapter _primaryKeyDataAdapter = null;
        private SqlDataAdapter _identityDataAdapter = null;

        #endregion

        #region Properties

        public DataTable Tables
        {
            get
            {
                if (_tables == null)
                {
                    _tables = new DataTable("INFORMATION_SCHEMA.TABLES");
                    FillDataTable(_tables, TableDataAdapter);
                }

                return _tables;
            }
        }

        public DataTable Columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = new DataTable("INFORMATION_SCHEMA.COLUMNS");
                    FillDataTable(_columns, ColumnDataAdapter);
                }

                return _columns;
            }
        }

        public DataTable PrimaryKeys
        {
            get
            {
                if (_primaryKeys == null)
                {
                    _primaryKeys = new DataTable("PrimaryKeys");
                    FillDataTable(_primaryKeys, PrimaryKeyDataAdapter);
                }

                return _primaryKeys;
            }
        }

        public DataTable Identities
        {
            get
            {
                if (_identities == null)
                {
                    _identities = new DataTable("Identities");
                    FillDataTable(_identities, IdentityDataAdapter);
                }

                return _identities;
            }
        }

        private SqlConnection Connection { get; set; }

        private SqlDataAdapter TableDataAdapter
        {
            get
            {
                if (_tableDataAdapter == null)
                {
                    SqlCommand cmd = Connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME <> 'sysdiagrams' ORDER BY TABLE_NAME";

                    _tableDataAdapter = new SqlDataAdapter(cmd);
                }

                return _tableDataAdapter;
            }
        }

        private SqlDataAdapter ColumnDataAdapter
        {
            get
            {
                if (_columnDataAdapter == null)
                {
                    SqlCommand cmd = Connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME <> 'sysdiagrams' ORDER BY COLUMN_NAME, ORDINAL_POSITION";

                    _columnDataAdapter = new SqlDataAdapter(cmd);
                }

                return _columnDataAdapter;
            }
        }

        private SqlDataAdapter PrimaryKeyDataAdapter
        {
            get
            {
                if (_primaryKeyDataAdapter == null)
                {
                    SqlCommand cmd = Connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = 
                          "SELECT "
                            + " i.name                    AS INDEX_NAME "
                            + ",ic.index_column_id        AS INDEX_COLUMN_ID "
                            + ",key_ordinal               AS ORDINAL_POSITION "
                            + ",i.object_id               AS TABLE_ID "
                            + ",t.name                    AS TABLE_NAME "
                            + ",c.name                    AS COLUMN_NAME "
                            + ",TYPE_NAME(c.user_type_id) AS COLUMN_TYPE "
                            + ",is_identity               AS IS_IDENTITY "
                        + "FROM "
                            + "sys.indexes AS i "
                        + "INNER JOIN "
                            + "sys.tables AS t "
                                + "ON i.object_id = t.object_id "
                        + "INNER JOIN "
                            + "sys.index_columns AS ic "
                                + "ON i.object_id = ic.object_id AND i.index_id = ic.index_id "
                        + "INNER JOIN "
                            + "sys.columns AS c "
                                + "ON ic.object_id = c.object_id AND c.column_id = ic.column_id "
                        + "WHERE "
                            + "i.is_primary_key = 1";

                    _primaryKeyDataAdapter = new SqlDataAdapter(cmd);
                }

                return _primaryKeyDataAdapter;
            }
        }

        private SqlDataAdapter IdentityDataAdapter
        {
            get
            {
                if (_identityDataAdapter == null)
                {
                    SqlCommand cmd = Connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText =
                        "SELECT "
                          + "COLUMN_NAME, "
                          + "TABLE_NAME "
                      + "FROM "
                          + "INFORMATION_SCHEMA.COLUMNS "
                      + "WHERE "
                          + "COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1";

                    _identityDataAdapter = new SqlDataAdapter(cmd);
                }

                return _identityDataAdapter;
            }
        }

        #endregion

        #region Constructors

        public InformationSchema(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            Connection = new SqlConnection(connectionString);            
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            TableDataAdapter.Dispose();
            ColumnDataAdapter.Dispose();

            CloseConnection();
            Connection.Dispose();
        }

        private void FillDataTable(DataTable dt, SqlDataAdapter adapter)
        {
            try
            {
                Connection.Open();
                adapter.Fill(dt);
            }
            finally
            {
                CloseConnection();
            }
        }

        private void CloseConnection()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        #endregion
    }
}
