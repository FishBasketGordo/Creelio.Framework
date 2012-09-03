namespace Creelio.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using Creelio.Framework.Data.Interfaces;
    using Creelio.Framework.Data.Model;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.Extensions;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

    public class DataAccessor<T> : IDataAccessor<T> where T : class, new()
    {
        private string _insertSP = null;

        private string _selectSP = null;
        
        private string _updateSP = null;
        
        private string _deleteSP = null;
        
        private string _countSP = null;

        public DataAccessor(string connectionName, string table = null, Func<string, string, string> formatSPName = null)
        {
            connectionName.ThrowIfNull(() => new ArgumentNullException("connectionName"));

            Table = table ?? typeof(T).Name;
            Database = EnterpriseLibraryContainer.Current.GetInstance<SqlDatabase>(connectionName);
            DatabaseInfo = new InformationSchema(Database.ConnectionString);
            FormatStoredProcedureName = formatSPName ?? ((tbl, action) => string.Format("dbo.{0}_{1}", tbl, action));
        }

        private Database Database { get; set; }

        private InformationSchema DatabaseInfo { get; set; }

        private string Table { get; set; }

        private Func<string, string, string> FormatStoredProcedureName { get; set; }

        private string InsertSP
        {
            get
            {
                if (_insertSP == null)
                {
                    _insertSP = FormatStoredProcedureName(Table, "INSERT");
                }

                return _insertSP;
            }
        }

        private string SelectSP
        {
            get
            {
                if (_selectSP == null)
                {
                    _selectSP = FormatStoredProcedureName(Table, "SELECT");
                }

                return _selectSP;
            }
        }

        private string UpdateSP
        {
            get
            {
                if (_updateSP == null)
                {
                    _updateSP = FormatStoredProcedureName(Table, "UPDATE");
                }

                return _updateSP;
            }
        }

        private string DeleteSP
        {
            get
            {
                if (_deleteSP == null)
                {
                    _deleteSP = FormatStoredProcedureName(Table, "DELETE");
                }

                return _deleteSP;
            }
        }

        private string CountSP
        {
            get
            {
                if (_countSP == null)
                {
                    _countSP = FormatStoredProcedureName(Table, "COUNT");
                }

                return _countSP;
            }
        }

        public List<T> Select()
        {
            object[] values = ReflectSPValues(SelectSP, null);
            return ExecuteStoredProc(SelectSP, values);
        }

        public T Select(int id)
        {
            object[] values = GetSPValues(SelectSP, id);
            return ExecuteStoredProc(SelectSP, values).FirstOrDefault();
        }

        public List<T> Select(T match)
        {
            object[] values = ReflectSPValues(SelectSP, match);
            return ExecuteStoredProc(SelectSP, values);
        }

        public int Count()
        {
            return Count(null);
        }

        public int Count(T match)
        {
            object[] values = ReflectSPValues(CountSP, match);
            return Convert.ToInt32(ExecuteScalarStoredProc(CountSP, values));
        }

        public void Save(ref T entity)
        {
            entity.ThrowIfNull(() => new ArgumentNullException("entity"));

            if (HasPrimaryKey(entity))
            {
                object[] values = ReflectSPValues(UpdateSP, entity);
                ExecuteNonQuery(UpdateSP, values);
            }
            else
            {
                object[] values = ReflectSPValues(InsertSP, entity);
                ExecuteScalarStoredProc(InsertSP, values);
            }
        }

        public void Remove(T entity)
        {
            object[] values = ReflectSPValues(DeleteSP, entity);
            ExecuteNonQuery(DeleteSP, values);
        }

        public void ClearPrimaryKey(T entity)
        {
            var primaryKeyRows = GetPrimaryKeyDataRows();

            foreach (DataRow dr in primaryKeyRows)
            {
                var primaryKeyColumn = dr["COLUMN_NAME"].ToString();
                var prop = entity.GetType().GetProperty(primaryKeyColumn);

                if (prop != null)
                {
                    prop.SetValue(entity, GetDefault(prop.PropertyType), null);
                }
            }
        }

        private List<T> ExecuteStoredProc(string procName, object[] values)
        {
            return Database.ExecuteSprocAccessor<T>(SelectSP, values).ToList();
        }

        private object ExecuteScalarStoredProc(string procName, object[] values)
        {
            return Database.ExecuteScalar(procName, values);
        }

        private int ExecuteNonQuery(string procName, object[] values)
        {
            using (DbCommand command = Database.GetStoredProcCommand(procName, values))
            {
                return Database.ExecuteNonQuery(command);
            }
        }

        private object[] GetSPValues(string procName, int id)
        {
            SqlCommand cmd = GetCommand(procName);
            object[] values = new object[cmd.Parameters.Count];
            values[0] = id;
            return values;
        }

        private object[] ReflectSPValues(string procName, T entity)
        {
            SqlCommand cmd = GetCommand(procName);

            if (entity != null)
            {
                PropertyInfo[] props = entity.GetType().GetProperties();
                List<object> valueList = new List<object>();

                foreach (SqlParameter param in cmd.Parameters)
                {
                    var prop = (from p in props
                                where p.Name == param.ParameterName.Substring(1)
                                select p).FirstOrDefault();

                    if (prop != null)
                    {
                        valueList.Add(prop.GetValue(entity, null));
                    }
                }

                return valueList.ToArray();
            }
            else
            {
                int paramCount = cmd.Parameters
                                    .ToList<SqlParameter>()
                                    .Where(p => p.Direction != ParameterDirection.ReturnValue)
                                    .Count();
                return new object[paramCount];
            }
        }

        private SqlCommand GetCommand(string procName)
        {
            SqlCommand cmd = new SqlCommand(procName) { CommandType = CommandType.StoredProcedure };
            Database.DiscoverParameters(cmd);
            return cmd;
        }

        private bool HasPrimaryKey(T entity)
        {
            var primaryKeyRows = GetPrimaryKeyDataRows();

            foreach (DataRow dr in primaryKeyRows)
            {
                var primaryKeyColumn = dr["COLUMN_NAME"].ToString();
                var prop = entity.GetType().GetProperty(primaryKeyColumn);

                if (prop != null && prop.GetValue(entity, null) == GetDefault(prop.PropertyType))
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<DataRow> GetPrimaryKeyDataRows()
        {
            var dataRows = DatabaseInfo.PrimaryKeyData.Rows.ToList<DataRow>().Where(r => r["TABLE_NAME"].ToString() == Table);
            return dataRows;
        }

        private object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}