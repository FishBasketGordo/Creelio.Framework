namespace Creelio.Framework.Templating.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Creelio.Framework.Data;
    using Creelio.Framework.Extensions;

    public sealed class Column : IComparable<Column>
    {
        private static readonly object[,] DatabaseTypeRelation =
        {
            { typeof(bool), DbType.Boolean },
            { typeof(byte), DbType.Byte },
            { typeof(byte[]), DbType.Binary },
            { typeof(DateTime), DbType.DateTime },
            { typeof(decimal), DbType.Decimal },
            { typeof(double), DbType.Double },
            { typeof(Guid), DbType.Guid },
            { typeof(short), DbType.Int16 },
            { typeof(int), DbType.Int32 },
            { typeof(long), DbType.Int64 },
            { typeof(object), DbType.Object },
            { typeof(string), DbType.String }
        };

        private static Dictionary<Type, DbType> _typeToDbType = null;

        private static Dictionary<DbType, Type> _dbTypeToType = null;

        private Table _table = null;

        private string _columnName;

        private Type _type;

        private DbType _dbType;

        static Column()
        {
            _typeToDbType = new Dictionary<Type, DbType>();
            _dbTypeToType = new Dictionary<DbType, Type>();

            for (int ii = 0; ii < DatabaseTypeRelation.GetLength(0); ii++)
            {
                Type type = (Type)DatabaseTypeRelation[ii, 0];
                DbType dbType = (DbType)DatabaseTypeRelation[ii, 1];

                _typeToDbType.Add(type, dbType);
                _dbTypeToType.Add(dbType, type);
            }
        }

        public Column(string columnName, string tableName, Type type)
        {
            Table.TableName = tableName;
            ColumnName = columnName;
            Type = type;
        }

        public Column(string columnName, string tableName, DbType dbType)
        {
            Table.TableName = tableName;
            ColumnName = columnName;
            DbType = dbType;
        }

        public Column(string fullyQualifiedColumnName, Type type)
        {
            Type = type;
            ParseFullyQualifiedColumnName(fullyQualifiedColumnName);
        }

        public Column(string fullyQualifiedColumnName, DbType dbType)
        {
            DbType = dbType;
            ParseFullyQualifiedColumnName(fullyQualifiedColumnName);
        }

        public Table Table
        {
            get
            {
                if (_table == null)
                {
                    _table = new Table();
                }

                return _table;
            }
        }
        
        public string ColumnName
        {
            get
            {
                return _columnName;
            }

            set
            {
                value.ThrowIfNullOrWhiteSpace(_ => new ArgumentNullException("Column name cannot be blank."));
                _columnName = value;
            }
        }

        public string FullQualifiedColumnName
        {
            get
            {
                return string.Format("{0}.[{1}]", Table.FullyQualifiedTableName, ColumnName);
            }
        }

        public Type Type
        {
            get
            {
                return _type;
            }

            private set
            {
                _type = value;
                _dbType = ConvertToDbType(value);
            }
        }

        public DbType DbType
        {
            get
            {
                return _dbType;
            }

            private set
            {
                _dbType = value;
                _type = ConvertToType(value);
            }
        }

        public static DbType ConvertToDbType(Type type)
        {
            return ConvertForDb<Type, DbType>(_typeToDbType, type);
        }

        public static Type ConvertToType(DbType dbType)
        {
            return ConvertForDb<DbType, Type>(_dbTypeToType, dbType);
        }

        public static bool NeedToSingleQuote(DbType dbType)
        {
            return NeedToSingleQuote(ConvertToType(dbType));
        }

        public static bool NeedToSingleQuote(Type type)
        {
            return type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(Guid);
        }

        public override string ToString()
        {
            return FullQualifiedColumnName;
        }

        public string ToString(string tableAlias)
        {
            tableAlias.ThrowIfNullOrWhiteSpace(_ => new ArgumentNullException("tableAlias"));
            return string.Format("{0}.[{1}]", tableAlias, ColumnName);
        }

        public int CompareTo(Column other)
        {
            return this.CompareToProperties(other, "ColumnName", "Table.TableName", "Type.FullName");
        }

        public int CompareTo(string columnName)
        {
            var other = new Column(columnName, Table.TableName, Type);
            return this.CompareToProperties(other, "ColumnName");
        }

        public int CompareTo(string columnName, string tableName)
        {
            var other = new Column(columnName, tableName, Type);
            return this.CompareToProperties(other, "ColumnName", "Table.TableName");
        }

        public bool Exists(string connectionString, string databaseName)
        {
            var dataProvider = new SmoDataProvider(connectionString);
            var column = dataProvider.GetColumn(Table.DatabaseName, Table.TableName, ColumnName);

            // TODO: Check on column type. May need to switch to Smo.SqlDataType
            return column != null;
        }

        private static TOut ConvertForDb<TIn, TOut>(Dictionary<TIn, TOut> dictionary, TIn key)
        {
            key.ThrowIf(
                k => k == null,
                _ => new ArgumentNullException("key"));

            dictionary.ThrowIf(
                d => !d.ContainsKey(key),
                _ => new InvalidOperationException(string.Format("Unable to convert {0} to {1}.", key, typeof(TOut))));

            return dictionary[key];
        }

        private void ParseFullyQualifiedColumnName(string fullyQualifiedColumnName)
        {
            fullyQualifiedColumnName.ThrowIfNullOrWhiteSpace(_ => new ArgumentNullException("fullyQualifiedColumnName"));

            var columnNameParts = (from cnp in fullyQualifiedColumnName.Split(StringSplitOptions.RemoveEmptyEntries, '.')
                                   select cnp.TrimStart('[').TrimEnd(']')).Reverse();

            ColumnName = columnNameParts.FirstOrDefault() ?? string.Empty;
            Table.TableName = columnNameParts.Skip(1).FirstOrDefault() ?? string.Empty;
            Table.SchemaName = columnNameParts.Skip(2).FirstOrDefault() ?? string.Empty;
            Table.DatabaseName = columnNameParts.Skip(3).FirstOrDefault() ?? string.Empty;
        }
    }
}