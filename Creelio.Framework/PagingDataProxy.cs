namespace Creelio.Framework
{
    using Creelio.Framework.Interfaces;

    public class PagingDataProxy : IPagingDataProvider
    {
        public int DataPage { get; set; }

        public int TotalDataPages { get; set; }

        public int TotalRecords { get; set; }

        public int RecordsPerPage { get; set; }
    }
}
