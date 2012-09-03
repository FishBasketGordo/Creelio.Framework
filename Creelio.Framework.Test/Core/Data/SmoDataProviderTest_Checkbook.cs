namespace Creelio.Framework.Test.Core.Data
{
    using Creelio.Framework.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Microsoft.SqlServer.Management.Smo;
    using System.Configuration;   
    
    [TestClass]
    public class SmoDataProviderTest_Checkbook : SmoDataProviderTest
    {
        public SmoDataProviderTest_Checkbook()
            : base("CheckbookConnectionString", "Checkbook")
        {
        }

        [TestMethod]
        public void CanGetAccountTable()
        {
            CanGetTable("Account");
        }

        [TestMethod]
        public void CanTryGetAccountTable()
        {
            CanTryGetTable("Account");
        }

        [TestMethod]
        public void CanGetAccountIDColumnFromAccountTable()
        {
            CanGetColumn("Account", "AccountID");
        }

        [TestMethod]
        public void CanTryGetAccountIDColumnFromAccountTable()
        {
            CanTryGetColumn("Account", "AccountID");
        }

        [TestMethod]
        public void CanGetAccountTablePrimaryKeys()
        {
            CanGetPrimaryKeys("Account");
        }

        [TestMethod]
        public void CanTryGetAccountTablePrimaryKeys()
        {
            CanTryGetPrimaryKeys("Account");
        }
    }
}
