namespace Creelio.Framework.Templating.FormatHelpers
{
    using System.Collections.Generic;
    using Microsoft.SqlServer.Management.Smo;

    public class SqlParameterList : List<SqlParameter>
    {
        public static SqlParameterList Create()
        {
            return new SqlParameterList();
        }

        public SqlParameterList Add(Column column, string name)
        {
            var param = new SqlParameter(column, name);
            this.Add(param);
            return this;
        }

        public SqlParameterList Add(Column column, string name, string defaultValue)
        {
            var param = new SqlParameter(typeName, name, defaultValue);
            this.Add(param);
            return this;
        }
    }
}