namespace Creelio.Framework.Core.Presentation
{
    using System;
    using System.ServiceModel;
    using Creelio.Framework.Interfaces;
    using Creelio.Framework.WebServices.Extensions;

    public abstract class Presenter<TView>
    {
        public Presenter(TView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view", "The view object cannot be null.");
            }

            View = view;
        }

        public Presenter(TView view, ISessionProvider session)
            : this(view)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session", "The session provider object cannot be null.");
            }

            Session = session;
        }

        protected TView View { get; private set; }

        protected ISessionProvider Session { get; private set; }

        public static TResponse CallWebService<TSvcInterface, TClient, TResponse>(Func<TClient, TResponse> webMethod)
            where TSvcInterface : class
            where TClient : ClientBase<TSvcInterface>, new()
        {
            var client = new TClient();
            var response = client.Using(webMethod);
            return response;
        }
    }
}