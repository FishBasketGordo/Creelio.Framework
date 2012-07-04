namespace Creelio.Framework.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Core.Extensions.MaybeMonad;

    public static class ICollectionExtensions
    {
        public static bool IsValidIndex<T>(this ICollection<T> collection, int index)
        {
            collection.ThrowIfNull(_ => new NullReferenceException("Collection is null."));
            return index >= 0 && index < collection.Count;
        }
    }
}