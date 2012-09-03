using System.Collections.Generic;

namespace Creelio.Framework.Interfaces
{
    public interface IEntitySelector<T> where T : class, new()
    {
        List<T> Select(string identifyingNumber, string sourceType);
    }
}
