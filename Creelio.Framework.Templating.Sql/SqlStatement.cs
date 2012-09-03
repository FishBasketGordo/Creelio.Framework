namespace Creelio.Framework.Templating.Sql
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Extensions;

    public abstract class SqlStatement
    {
        private static SqlStatementFormats _defaultFormat = SqlStatementFormats.SingleLine;

        private Dictionary<SqlStatementFormats, Func<string>> _formatters = null;

        public static SqlStatementFormats DefaultFormat
        {
            get
            {
                return _defaultFormat;
            }

            set
            {
                _defaultFormat = value;
            }
        }

        protected Dictionary<SqlStatementFormats, Func<string>> Formatters
        {
            get
            {
                if (_formatters == null)
                {
                    _formatters = new Dictionary<SqlStatementFormats, Func<string>>();
                }

                return _formatters;
            }
        }

        public override string ToString()
        {
            return ToString(DefaultFormat);
        }

        public virtual string ToString(SqlStatementFormats format)
        {
            var formatter = Formatters.GetOrReturnDefault(format);
            return formatter.InvokeOrReturnDefault(string.Empty);
        }
    }
}