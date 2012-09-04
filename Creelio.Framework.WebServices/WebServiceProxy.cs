namespace Creelio.Framework.WebServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.WebServices.Extensions;

    public class WebServiceProxy<TServiceContract, TBinding>
        where TServiceContract : class
        where TBinding : Binding, new()
    {
        private ExecutionEnvironment _environment = ExecutionEnvironment.Local;

        private TBinding _binding = null;

        private EndpointAddress _endpointAddress = null;

        public WebServiceProxy(Uri serviceUrl, ExecutionEnvironment environment)
            : this(new Dictionary<ExecutionEnvironment, Uri> { { environment, serviceUrl } }, environment)
        {            
        }

        public WebServiceProxy(IDictionary<ExecutionEnvironment, Uri> serviceUrls, ExecutionEnvironment defaultEnvironment)
        {
            serviceUrls
                .ThrowIfNull(
                    () => new ArgumentNullException("serviceUrls", "Cannot create with null service URL dictionary."))
                .ThrowIf(
                    urls => urls.Any(kvp => kvp.Value == null),
                    () => new ArgumentNullException("Cannot create with null service URL."))
                .ThrowIf(
                    urls => !urls.ContainsKey(defaultEnvironment),
                    () => new ArgumentException("Cannot create with default environment that is not contained in dictionary."));

            ServiceUrls = serviceUrls;
            Environment = defaultEnvironment;
        }

        public ExecutionEnvironment Environment
        {
            get
            {
                return _environment;
            }

            set
            {
                value.ThrowIf(
                    v => !ServiceUrls.ContainsKey(v),
                    () => new InvalidOperationException(
                        string.Format(
                            "No service URL exists for {0} environment. " +
                            "Please set the service address for this environment " +
                            "with the SetServiceAddress method before attempting " +
                            "to use this environment.", 
                            value)));

                _environment = value;
                _endpointAddress = null;
            }
        }

        public Uri ServiceUrl
        {
            get
            {
                return ServiceUrls[Environment];
            }
        }

        public TBinding Binding 
        {
            get
            {
                if (_binding == null)
                {
                    _binding = new TBinding();
                }

                return _binding;
            }

            set
            {
                _binding = value;
            }
        }

        public EndpointAddress EndpointAddress
        {
            get
            {
                if (_endpointAddress == null)
                {
                    _endpointAddress = new EndpointAddress(ServiceUrl);
                }

                return _endpointAddress;
            }

            set
            {
                _endpointAddress = value;
            }
        }

        private IDictionary<ExecutionEnvironment, Uri> ServiceUrls { get; set; }

        public void SetServiceAddress(ExecutionEnvironment environment, Uri url)
        {
            url.ThrowIfNull(() => new ArgumentNullException("url", "Cannot set service address with null Uri object."));

            ServiceUrls.AddOrSet(environment, url);
        }

        public TResponse RunServiceProxy<TResponse>(Func<TServiceContract, TResponse> runServiceProxy)
        {
            var proxy = CreateServiceProxy();
            return (proxy as ICommunicationObject).Using(p => runServiceProxy(p as TServiceContract));
        }

        public void RunServiceProxy(Action<TServiceContract> runServiceProxy)
        {
            var proxy = CreateServiceProxy();
            (proxy as ICommunicationObject).Using(p => runServiceProxy(p as TServiceContract));
        }

        private TServiceContract CreateServiceProxy()
        {
            var channelFactory = new ChannelFactory<TServiceContract>(Binding, EndpointAddress);
            return channelFactory.CreateChannel(EndpointAddress);
        }
    }
}
