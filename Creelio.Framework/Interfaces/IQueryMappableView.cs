namespace Creelio.Framework.Interfaces
{
    using System.Collections.Specialized;

    public interface IQueryMappableView
    {
        void Map(NameValueCollection queryString);
    }
}