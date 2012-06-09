using System.Collections.Generic;

namespace Creelio.Framework.Core.Interfaces
{
    public interface IEntitySelector<T> where T : class, new()
    {
        List<T> Select(string identifyingNumber, string sourceType);
    }
}
