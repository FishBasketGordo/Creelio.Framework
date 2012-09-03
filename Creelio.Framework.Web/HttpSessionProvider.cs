namespace Creelio.Framework.Web
{
    using System.Web.SessionState;
    using Creelio.Framework.Interfaces;    

    public class HttpSessionProvider : ISessionProvider
    {
        public HttpSessionProvider(HttpSessionState session)
        {
            Session = session;
        }

        private HttpSessionState Session { get; set; }

        public object this[string key]
        {
            get
            {
                return Session[key];
            }

            set
            {
                Session[key] = value;
            }
        }
    }
}
