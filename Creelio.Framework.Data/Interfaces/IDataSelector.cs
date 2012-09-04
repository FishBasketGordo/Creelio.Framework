namespace Creelio.Framework.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IDataSelector<T> where T : new()
    {
        List<T> Select();

        T Select(int id);
        
        List<T> Select(T match);
        
        int Count();
        
        int Count(T match);
    }
}