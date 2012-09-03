namespace Creelio.Framework.WebServices
{
    using System;
    using System.Reflection;
    using Creelio.Framework.Interfaces;

    /// <summary>
    /// This class may be used to consolidate the Metadata 
    /// objects within various service reference namespaces.
    /// </summary>
    public class MetadataProxy : IServiceMetadata
    {
        public MetadataStatus Status { get; set; }

        public string Message { get; set; }

        public string UserName { get; set; }

        public static MetadataProxy Create(object metadata)
        {
            var metadataProxy = new MetadataProxy();
            var interfaceProperties = typeof(IServiceMetadata).GetProperties();

            foreach (var interfaceProperty in interfaceProperties)
            {
                var metadataProperty = metadata.GetType().GetProperty(interfaceProperty.Name);
                var proxyProperty = metadataProxy.GetType().GetProperty(interfaceProperty.Name);

                if (CanCopyToProxy(metadataProperty, proxyProperty))
                {
                    var value = metadataProperty.GetValue(metadata, null);
                    proxyProperty.SetValue(metadataProxy, value, null);
                }
            }

            return metadataProxy;
        }

        public void ThrowIfUnsuccessful<TException>(Func<IServiceMetadata, TException> createException)
            where TException : Exception
        {
            if (Status != MetadataStatus.Success)
            {
                throw createException(this);
            }
        }

        private static bool CanCopyToProxy(PropertyInfo metadataProperty, PropertyInfo proxyProperty)
        {
            return metadataProperty != null
                && proxyProperty != null
                && metadataProperty.PropertyType == proxyProperty.PropertyType
                && metadataProperty.CanRead
                && proxyProperty.CanWrite;
        }
    }
}
