namespace Creelio.Framework.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.Extensions;

    public static class UriBuilderExtensions
    {
        public static UriBuilder AddOrSetQueryParameter(this UriBuilder uriBuilder, string key, string value)
        {
            uriBuilder.ThrowIfNull(
                _ => new NullReferenceException("The uriBuilder object is null."));

            key.ThrowIf(
                k => string.IsNullOrEmpty(k),
                _ => new ArgumentNullException("key"));

            value.ThrowIf(
                v => string.IsNullOrEmpty(v),
                _ => new ArgumentNullException("value"));

            var queryParams = new Dictionary<string, string>()
                .FillFromQueryString(uriBuilder.Query)
                .AddOrSet(key, value);

            uriBuilder.Query = queryParams.ToQueryString(false);
            return uriBuilder;
        }
    }
}