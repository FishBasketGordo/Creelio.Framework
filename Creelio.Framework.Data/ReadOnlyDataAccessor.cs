using System;
using System.Collections.Generic;
using Creelio.Framework.Data.Interfaces;
using Creelio.Framework.Extensions;

namespace Creelio.Framework.Data
{
    public class ReadOnlyDataAccessor<T> : IDataSelector<T> where T : new()
    {
        #region Properties

        private IDataAccessor<T> Accessor { get; set; }

        #endregion

        #region Constructors

        public ReadOnlyDataAccessor(IDataAccessor<T> accessor)
        {
            accessor.ThrowIfNull(() => new ArgumentNullException("accessor"));
            Accessor = accessor;
        }

        #endregion

        #region Methods

        public List<T> Select()
        {
            return Accessor.Select();
        }

        public T Select(int id)
        {
            return Accessor.Select(id);
        }

        public List<T> Select(T match)
        {
            return Accessor.Select(match);
        }

        public int Count()
        {
            return Accessor.Count();
        }

        public int Count(T match)
        {
            return Accessor.Count(match);
        }

        #endregion
    }
}