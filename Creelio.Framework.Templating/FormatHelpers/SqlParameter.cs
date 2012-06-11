namespace Creelio.Framework.Templating.FormatHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SqlServer.Management.Smo;

    public class SqlParameter
    {
        public SqlParameter(Column column, string name)
            : this(column, name, null)
        {
        }

        public SqlParameter(Column column, string name, string defaultValue)
        {
            Column = column;
            Name = name;
            DefaultValue = defaultValue;
        }

        public Column Column { get; set; }

        public string Name { get; set; }

        public string DefaultValue { get; set; }
    }
}