using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Creelio.Framework.Core.Data.Model
{
    public partial class InformationSchema
    {
        [Obsolete("Use SMO objects instead.")]
        public abstract class Entity
        {
            #region Constructors

            protected Entity(DataRow dataRow)
            {
                PropertyInfo[] props = this.GetType().GetProperties();
                foreach (var prop in props)
                {
                    if (!prop.CanRead || !prop.CanWrite)
                        continue;

                    if (!dataRow.IsNull(prop.Name))
                        prop.SetValue(this, dataRow[prop.Name], null);
                }
            }

            #endregion

            #region Methods

            protected static List<T> CreateList<T>(DataTable dataTable, Func<DataRow, T> constructor) where T : Entity
            {
                List<T> list = new List<T>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    T entity = constructor(dataRow);
                    list.Add(entity);
                }
                return list;
            }

            #endregion
        }
    }
}
