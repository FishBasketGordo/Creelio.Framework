namespace Creelio.Framework.Test.WebServices
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Creelio.Framework.WebServices;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebServiceProxyTests
    {
        private interface ITestContract
        {
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateWithNullUrl()
        {
            var proxy = new WebServiceProxy<ITestContract, BasicHttpBinding>(
                (Uri)null,
                ExecutionEnvironment.Local);

            GC.KeepAlive(proxy);
            Assert.Fail("Constructor should have failed.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateWithNullUrlDictionary()
        {
            var proxy = new WebServiceProxy<ITestContract, BasicHttpBinding>(
                (IDictionary<ExecutionEnvironment, Uri>)null,
                ExecutionEnvironment.Local);

            GC.KeepAlive(proxy);
            Assert.Fail("Constructor should have failed.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotCreateWithUrlDictionaryContainingNullUrl()
        {
            var proxy = new WebServiceProxy<ITestContract, BasicHttpBinding>(
                new Dictionary<ExecutionEnvironment, Uri>
                {
                    { ExecutionEnvironment.Local, null },
                },
                ExecutionEnvironment.Local);

            GC.KeepAlive(proxy);
            Assert.Fail("Constructor should have failed.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotCreateWithDefaultEnvironmentNotInUrlDictionary()
        {
            var proxy = new WebServiceProxy<ITestContract, BasicHttpBinding>(
                new Dictionary<ExecutionEnvironment, Uri>
                {
                    { ExecutionEnvironment.Local, new Uri("http://www.dummysite.com") },
                },
                ExecutionEnvironment.Development);

            GC.KeepAlive(proxy);
            Assert.Fail("Constructor should have failed.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotSetEnvironmentIfServiceUrlNotFound()
        {
            var proxy = new WebServiceProxy<ITestContract, BasicHttpBinding>(
                new Uri("http://www.dummysite.com"), 
                ExecutionEnvironment.Local);

            proxy.Environment = ExecutionEnvironment.Development;
            Assert.Fail("Environment property setter should have failed.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotSetServiceAddressWithNullUrl()
        {
            var proxy = new WebServiceProxy<ITestContract, BasicHttpBinding>(
                new Uri("http://www.dummysite.com"), 
                ExecutionEnvironment.Local);

            proxy.SetServiceAddress(ExecutionEnvironment.Development, null);
            Assert.Fail("SetServiceAddress method call should have failed.");
        }
    }
}
