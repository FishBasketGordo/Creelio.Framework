using System.Collections.Specialized;

namespace Creelio.Framework.Core.Interfaces
{
    public interface IQueryMappableView
    {
        void Map(NameValueCollection queryString);
    }
}