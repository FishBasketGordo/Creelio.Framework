using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Creelio.Framework.Data.Model
{
    [Obsolete("Use SMO objects instead.")]
    public partial class InformationSchema : IDisposable
    {
        #region Fields

        private DataTable _tableData = null;
        private DataTable _columnData = null;
        private DataTable _primaryKeyData = null;
        private DataTable _identityData = null;

        private List<Table> _tables = null;
        private List<Column> _columns = null;
        private List<PrimaryKey> _primaryKeys = null;
        private List<Identity> _identities = null;
        
        private SqlDataAdapter _tableDataAdapter = null;
        private SqlDataAdapter _columnDataAdapter = null;
        private SqlDataAdapter _primaryKeyDataAdapter = null;
        private SqlDataAdapter _identityDataAdapter = null;

        #endregion

        #region Properties

        public List<Table> Tables
        {
            get
            {
                if (_tables == null)
                {
                    _tables = Table.CreateList(TableData);
                }

                return _tables;
            }
        }

        public DataTable TableData
        {
            get
            {
                if (_tableData == null)
                {
                    _tableData = new DataTable("INFORMATION_SCHEMA.TABLES");
                    FillDataTable(_tableData, TableDataAdapter);
                }

                return _tableData;
            }
        }

        public List<Column> Columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = Column.CreateList(ColumnData);
                }

                return _columns;
            }
        }

        public DataTable ColumnData
        {
            get
            {
                if (_columnData == null)
                {
                    _columnData = new DataTable("INFORMATION_SCHEMA.COLUMNS");
                    FillDataTable(_columnData, ColumnDataAdapter);
                }

                return _columnData;
            }
        }

        public List<PrimaryKey> PrimaryKeys
        {
            get
            {
                if (_primaryKeys == null)
                {
                    _primaryKeys = PrimaryKey.CreateList(PrimaryKeyData);
                }

                return _primaryKeys;
            }
        }

        public DataTable PrimaryKeyData
        {
            get
            {
                if (_primaryKeyData == null)
                {
                    _primaryKeyData = new DataTable("PrimaryKeys");
                    FillDataTable(_primaryKeyData, PrimaryKeyDataAdapter);
                }

                return _primaryKeyData;
            }
        }

        public List<Identity> Identities
        {
            get
            {
                if (_identities == null)
                {
                    _identities = Identity.CreateList(IdentityData);
                }

                return _identities;
            }
        }

        public DataTable IdentityData
        {
            get
            {
                if (_identityData == null)
                {
                    _identityData = new DataTable("Identities");
                    FillDataTable(_identityData, IdentityDataAdapter);
                }

                return _identityData;
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

        public static InformationSchema CreateAndDisconnect(string connectionString)
        {
            InformationSchema info;
            using (info = new InformationSchema(connectionString))
            {
                // Force table adapters to fill DataTable objects.
                GC.KeepAlive(info.TableData);
                GC.KeepAlive(info.ColumnData);
                GC.KeepAlive(info.PrimaryKeyData);
                GC.KeepAlive(info.IdentityData);
            }

            return info;
        }

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

        private List<DataRow> ToRowList(DataTable dt)
        {
            return dt.Rows.Cast<DataRow>().ToList();
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
