using System;
using System.Collections.Generic;
using System.Data;

namespace Creelio.Framework.Data.Model
{
    public partial class InformationSchema
    {
        [Obsolete("Use SMO objects instead.")]
        public class PrimaryKey : Entity
        {
            #region Properties

            public string INDEX_NAME { get; private set; }
            public int INDEX_COLUMN_ID { get; private set; }
            public int ORDINAL_POSITION { get; private set; }
            public int TABLE_ID { get; private set; }
            public string TABLE_NAME { get; private set; }
            public string COLUMN_NAME { get; private set; }
            public string COLUMN_TYPE { get; private set; }
            public bool IS_IDENTITY { get; private set; }

            #endregion

            #region Constructors

            public PrimaryKey(DataRow dataRow)
                : base(dataRow)
            {
            }

            #endregion

            #region Methods

            public static List<PrimaryKey> CreateList(DataTable dataTable)
            {
                return CreateList(dataTable, dr => new PrimaryKey(dr));
            }

            #endregion
        }
    }
}
