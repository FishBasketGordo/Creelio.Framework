using Creelio.Framework.Core.Interfaces;

namespace Creelio.Framework.Core.Presentation
{
    public class PagingDataProxy : IPagingDataProvider
    {
        #region IPagingDataProvider Members

        public int DataPage { get; set; }
        public int TotalDataPages { get; set; }
        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }
        
        #endregion
    }
}