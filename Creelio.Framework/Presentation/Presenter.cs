using System;
using System.ServiceModel;
using Creelio.Framework.Core.Interfaces;

namespace Creelio.Framework.Core.Presentation
{
    public abstract class Presenter<TView>
    {
        #region Constructors

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

        #endregion

        #region Properties

        protected TView View { get; private set; }
        protected ISessionProvider Session { get; private set; }

        #endregion

        #region Methods

		public static TResponse CallWebService<TSvcInterface, TClient, TResponse>(Func<TClient, TResponse> webMethod)
			where TSvcInterface : class
			where TClient : ClientBase<TSvcInterface>, new()
		{
			TClient client = new TClient();
			TResponse response = webMethod(client);
			client.Close();

			return response;
		}

        #endregion
    }
}