﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData.Core.UriParser.Semantic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.OData.Routing.Conventions
{
    public class DefaultODataRoutingConvention : IODataRoutingConvention
    {
        private static readonly IDictionary<string, string> _actionNameMappings = new Dictionary<string, string>()
        {
            {"GET", "Get"},
            {"POST", "Post"},
            {"PUT", "Put"},
            {"DELETE", "Delete"}
        };

        public ActionDescriptor SelectAction(RouteContext routeContext)
        {
            var preflightFor =
                routeContext.HttpContext.Request.Headers["Access-Control-Request-Method"].FirstOrDefault();
            var odataPath = routeContext.HttpContext.Request.ODataProperties().NewPath;
            var controllerName = string.Empty;
            var methodName = routeContext.HttpContext.Request.Method;
            var routeTemplate = string.Empty;
            var keys = new List<KeyValuePair<string, object>>();

            if (odataPath.FirstSegment is MetadataSegment)
            {
                controllerName = "Metadata";
            }
            else
            {
                // TODO: we should use attribute routing to determine controller and action.
                var entitySetSegment = odataPath.FirstSegment as EntitySetSegment;
                if (entitySetSegment != null)
                {
                    controllerName = entitySetSegment.EntitySet.Name;
                }

                var keySegment = odataPath.FirstOrDefault(s => s is KeySegment) as KeySegment;
                if (keySegment != null)
                {
                    keys.AddRange(keySegment.Keys);
                }

                if (keys.Count == 1)
                {
                    routeTemplate = "{id}";
                }

                var structuralPropertySegment =
                    odataPath.FirstOrDefault((s => s is PropertySegment)) as PropertySegment;
                if (structuralPropertySegment != null)
                {
                    routeTemplate += "/" + structuralPropertySegment.Property.Name;
                }

                var navigationPropertySegment =
                    odataPath.FirstOrDefault(s => s is NavigationPropertySegment) as NavigationPropertySegment;
                if (navigationPropertySegment != null)
                {
                        routeTemplate += "/" + navigationPropertySegment.NavigationProperty.Name;
                }

                var operationSegment =
                    odataPath.FirstOrDefault(s => s is OperationSegment) as OperationSegment;
                if (operationSegment != null)
                {
                    routeTemplate += "/" + operationSegment.Operations.First().Name;
                }
            }

            if (string.IsNullOrEmpty(routeTemplate))
            {
                routeTemplate = controllerName;
            }
            else
            {
                routeTemplate = controllerName + "/" + routeTemplate.TrimStart('/');
            }
            
            var services = routeContext.HttpContext.RequestServices;
            var provider = services.GetRequiredService<IActionDescriptorCollectionProvider>();
            var actionDescriptor = provider.ActionDescriptors.Items.SingleOrDefault(d =>
            {
                var c = d as ControllerActionDescriptor;
                if (c == null)
                {
                    return false;
                }
                if (c.ControllerName != controllerName)
                {
                    return false;
                }
                if (controllerName == "Metadata")
                {
                    return true;
                }
                if (!c.AttributeRouteInfo.Template.EndsWith(routeTemplate))
                {
                    return false;
                }
                // If we find no action constraints, this isn't our method
                if (c.ActionConstraints == null || !c.ActionConstraints.Any())
                {
                    return false;
                }
                var httpMethodConstraint = ((HttpMethodActionConstraint)c.ActionConstraints.First());
                if (methodName.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
                {
                    return httpMethodConstraint.HttpMethods.Contains(preflightFor);
                }
                return httpMethodConstraint.HttpMethods.Contains(methodName);
            });

            if (actionDescriptor == null)
            {
                throw new NotSupportedException(string.Format("No action match template '{0}' in '{1}Controller'", routeTemplate, controllerName));
            }

            if (keys.Any())
            {
                WriteRouteData(routeContext, actionDescriptor.Parameters, keys);
            }

            return actionDescriptor;
        }

        private void WriteRouteData(RouteContext context, IList<ParameterDescriptor> parameters, IList<KeyValuePair<string, object>> keys)
        {
            for (int i = 0; i < keys.Count; ++i)
            {
                // TODO: check if parameters match keys.
                context.RouteData.Values[parameters[i].Name] = keys[i].Value;
            }
        }
    }
}