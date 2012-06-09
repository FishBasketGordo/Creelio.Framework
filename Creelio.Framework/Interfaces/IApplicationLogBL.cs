using System;

namespace Creelio.Framework.Core.Interfaces
{
    public interface IApplicationLogBL
    {
        int Save(string logMessage);
        int Save(ApplicationLogMessageType messageType, string source, string targetSite, string logMessage);
        int Save(Exception ex);
    }
}
