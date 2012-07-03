namespace Creelio.Framework.Templating.Sql
{
    using System.Collections.Generic;
    using Creelio.Framework.Core.Extensions.StringExtensions;
    using Creelio.Framework.Core.Extensions.DictionaryExtensions;
    using System.Text;
    
    public class InsertStatement
    {
        private Dictionary<Column, object> _values = null;

        public InsertStatement()
        {
        }

        public InsertStatement(string tableName)
            : this(tableName, null)
        {
        }

        public InsertStatement(string tableName, Dictionary<Column, object> values)
        {
            TableName = tableName;
            Values = values;
        }

        public string TableName { get; set; }

        public Dictionary<Column, object> Values
        {
            get
            {
                if (_values == null)
                {
                    _values = new Dictionary<Column, object>();
                }

                return _values;
            }

            private set
            {
                _values = value;
            }
        }

        public override string ToString()
        {
            if (TableName.IsNullOrWhiteSpace() || Values.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            // TODO: Finish INSERT ToString
            sb.AppendFormat("INSERT INTO [{0}]", TableName);

            return sb.ToString();
        }
    }
}