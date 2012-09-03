using System.Collections.Specialized;

namespace Creelio.Framework.Interfaces
{
    public interface IQueryMappableView
    {
        void Map(NameValueCollection queryString);
    }
}