using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Creelio.Framework.DAL
{
	public partial class InformationSchema
	{
        public class Table : Entity
        {
            #region Properties

            public string TABLE_CATALOG { get; private set; }
            public string TABLE_SCHEMA { get; private set; }
            public string TABLE_NAME { get; private set; }
            public string TABLE_TYPE { get; private set; }

            #endregion

            #region Constructors

            public Table(DataRow dataRow)
                : base(dataRow) 
            {
            }

            #endregion

            #region Methods

            public static List<Table> CreateList(DataTable dataTable)
            {
                return CreateList(dataTable, dr => new Table(dr));
            }

            #endregion
        }
	}
}
