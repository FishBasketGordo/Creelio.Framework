using System;
using System.Collections.Generic;

namespace Creelio.Framework.DAL.Interfaces
{
    public interface IDataSelector<T> where T : new()
    {
        List<T> Select();
        T Select(int id);
        List<T> Select(T match);
        int Count();
        int Count(T match);
    }
}
