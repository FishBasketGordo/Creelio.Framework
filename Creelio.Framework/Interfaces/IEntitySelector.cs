namespace Creelio.Framework.Interfaces
{
    using System.Collections.Generic;

    public interface IEntitySelector<T> where T : class, new()
    {
        List<T> Select(string identifyingNumber, string sourceType);
    }
}