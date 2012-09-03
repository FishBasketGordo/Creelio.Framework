namespace Creelio.Framework.Templating.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.Extensions;

    public class Table
    {
        private string _databaseName = string.Empty;

        private string _ownerName = string.Empty;

        private string _tableName = string.Empty;
                
        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }

            set
            {
                _databaseName = value.TrimOrEmpty();
            }
        }

        public string SchemaName
        {
            get
            {
                return _ownerName;
            }

            set
            {
                _ownerName = value.TrimOrEmpty();
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
                value.ThrowIfNullOrWhiteSpace(_ => new ArgumentNullException("Table name cannot be blank."));
                _tableName = value;
            }
        }

        public string FullyQualifiedTableName
        {
            get
            {
                TableName.ThrowIfNullOrWhiteSpace(
                _ => new InvalidOperationException("Cannot call ToString on a table with no name."));

                var columnNameParts = new List<string>();

                DatabaseName.DoIf(
                    d => !string.IsNullOrEmpty(d),
                    d => columnNameParts.Add(string.Format("[{0}]", d)));

                SchemaName.DoIf(
                    s => !string.IsNullOrEmpty(s),
                    s => columnNameParts.Add(string.Format("[{0}]", s)));

                columnNameParts.Add(string.Format("[{0}]", TableName));

                return columnNameParts.Aggregate((s1, s2) => string.Format("{0}.{1}", s1, s2));
            }
        }

        public override string ToString()
        {
            return FullyQualifiedTableName;
        }
    }
}