namespace Creelio.Framework.UnitTesting
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class ServiceTestHarness
    {
        protected ServiceTestHarness(string serviceName)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; protected set; }

        protected void PrintLine(string line)
        {
            var formatted = string.Format("[{1} {2}] {3}{0}", Environment.NewLine, ServiceName, DateTime.Now, line);
            Console.WriteLine(formatted);
        }

        protected void PrintLine(string format, params object[] paramters)
        {
            PrintLine(string.Format(format, paramters));
        }

        protected void PrintException(Exception ex)
        {
            PrintLine(ex.ToString());
        }

        protected abstract void InitializeClient();

        protected abstract void CloseClient();

        protected abstract void AbortClient();

        protected abstract Uri GetEndpointAddress();

        protected void RunTest(Action test)
        {
            InitializeClient();

            try
            {
                PrintLine("Beginning call to \"{0}\" ({1})...", ServiceName, GetEndpointAddress());

                test();

                PrintLine("Completed call to \"{0}\".", ServiceName);

                CloseClient();
            }
            catch (Exception ex)
            {
                PrintLine("Completed call to \"{0}\" with errors.", ServiceName);
                PrintException(ex);
                AbortClient();
                Assert.Fail(ex.Message);
            }
        }
    }
}