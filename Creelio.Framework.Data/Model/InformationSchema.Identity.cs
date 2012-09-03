using System;
using System.Collections.Generic;
using System.Data;

namespace Creelio.Framework.Data.Model
{
    public partial class InformationSchema
    {
        [Obsolete("Use SMO objects instead.")]
        public class Identity : Entity
        {
            #region Properties

            public string COLUMN_NAME { get; private set; }
            public string TABLE_NAME { get; private set; }

            #endregion

            #region Constructors

            public Identity(DataRow dataRow)
                : base(dataRow)
            {
            }

            #endregion

            #region Methods

            public static List<Identity> CreateList(DataTable dataTable)
            {
                return CreateList(dataTable, dr => new Identity(dr));
            }

            #endregion
        }
    }
}
