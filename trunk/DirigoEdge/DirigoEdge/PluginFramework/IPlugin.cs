using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace DirigoEdge.PluginFramework
{
    public interface IPlugin
    {
        void RegisterActions();

        void RegisterFilters();

        RouteCollection RegisterRoutes();
    }
}