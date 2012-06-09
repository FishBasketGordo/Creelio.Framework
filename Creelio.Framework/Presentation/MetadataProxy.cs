using System.Reflection;
using Creelio.Framework.Core.Interfaces;

namespace Creelio.Framework.Core.Presentation
{
    /// <summary>
    /// This class may be used to consolidate the Metadata objects within various service reference namespaces.
    /// </summary>
    public class MetadataProxy : IServiceMetadata
    {
        #region Methods

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

        private static bool CanCopyToProxy(PropertyInfo metadataProperty, PropertyInfo proxyProperty)
        {
            return metadataProperty != null
                && proxyProperty != null
                && metadataProperty.PropertyType == proxyProperty.PropertyType
                && metadataProperty.CanRead
                && proxyProperty.CanWrite;
        }

        #endregion

        #region IServiceMetadata Members

        public MetadataStatus Status { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }

        #endregion
    }
}