using System;
using System.Collections.Generic;
using System.Data;

namespace Creelio.Framework.Data.Model
{
    public partial class InformationSchema
    {
        [Obsolete("Use SMO objects instead.")]
        public class Column : Entity
        {
            #region Properties

            public string TABLE_CATALOG { get; private set; }
            public string TABLE_SCHEMA { get; private set; }
            public string TABLE_NAME { get; private set; }
            public string COLUMN_NAME { get; private set; }
            public int? ORDINAL_POSITION { get; private set; }
            public string COLUMN_DEFAULT { get; private set; }
            public string IS_NULLABLE { get; private set; }
            public string DATA_TYPE { get; private set; }
            public int? CHARACTER_MAXIMUM_LENGTH { get; private set; }
            public int? CHARACTER_OCTET_LENGTH { get; private set; }
            public int? NUMERIC_PRECISION { get; private set; }
            public int? NUMERIC_PRECISION_RADIX { get; private set; }
            public int? NUMERIC_SCALE { get; private set; }
            public int? DATETIME_PRECISION { get; private set; }
            public string CHARACTER_SET_CATALOG { get; private set; }
            public string CHARACTER_SET_SCHEMA { get; private set; }
            public string CHARACTER_SET_NAME { get; private set; }
            public string COLLATION_CATALOG { get; private set; }
            public string COLLATION_SCHEMA { get; private set; }
            public string COLLATION_NAME { get; private set; }
            public string DOMAIN_CATALOG { get; private set; }
            public string DOMAIN_SCHEMA { get; private set; }
            public string DOMAIN_NAME { get; private set; }

            #endregion

            #region Constructors

            public Column(DataRow dataRow)
                : base(dataRow) 
            {
            }

            #endregion

            #region Methods

            public static List<Column> CreateList(DataTable dataTable)
            {
                return CreateList(dataTable, dr => new Column(dr));
            }

            #endregion
        }
    }
}
