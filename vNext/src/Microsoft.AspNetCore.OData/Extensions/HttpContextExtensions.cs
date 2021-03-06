﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Common;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.OData.Extensions
{
	public static class HttpContextExtensions
    {
        public static ODataProperties ODataProperties(this HttpContext httpContext)
        {
			if (httpContext == null)
            {
                throw Error.ArgumentNull("httpContext");
            }

	        var oDataProperties = httpContext.RequestServices.GetRequiredService<ODataProperties>();
			oDataProperties.Configure(httpContext);
	        return oDataProperties;
        }

        public static IUrlHelper UrlHelper(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw Error.ArgumentNull("httpContext");
            }

            return httpContext.RequestServices.GetRequiredService<IUrlHelper>();
        }

        public static IETagHandler ETagHandler(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw Error.ArgumentNull("httpContext");
            }

            return httpContext.RequestServices.GetRequiredService<IETagHandler>();
        }

        public static IODataPathHandler ODataPathHandler(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw Error.ArgumentNull("httpContext");
            }

            return httpContext.RequestServices.GetRequiredService<IODataPathHandler>();
        }

        public static AssembliesResolver AssemblyProvider(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw Error.ArgumentNull("httpContext");
            }

            return httpContext.RequestServices.GetRequiredService<AssembliesResolver>();
        }
    }
}