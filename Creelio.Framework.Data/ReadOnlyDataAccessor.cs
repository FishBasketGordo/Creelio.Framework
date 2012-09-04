namespace Creelio.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Data.Interfaces;
    using Creelio.Framework.Extensions;

    public class ReadOnlyDataAccessor<T> : IDataSelector<T> where T : new()
    {
        public ReadOnlyDataAccessor(IDataAccessor<T> accessor)
        {
            accessor.ThrowIfNull(() => new ArgumentNullException("accessor"));
            Accessor = accessor;
        }

        private IDataAccessor<T> Accessor { get; set; }

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
    }
}