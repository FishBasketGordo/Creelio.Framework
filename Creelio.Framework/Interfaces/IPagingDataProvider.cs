﻿namespace Creelio.Framework.Core.Interfaces
{
    public interface IPagingDataProvider
    {
        int DataPage { get; }
        int TotalDataPages { get; }
        int TotalRecords { get; }
        int RecordsPerPage { get; }
    }
}