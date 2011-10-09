using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Creelio.Framework.DAL
{
    public partial class InformationSchema
    {
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
