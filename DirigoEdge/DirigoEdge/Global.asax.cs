using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DirigoEdge.PluginFramework;

namespace DirigoEdge
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteCollection rc = RouteTable.Routes;

            // Load all Plugins from plugin Directory, adding any plugin routes to the collection
            PluginAssemblyLoader.Load(rc);

            // Registers Containing Area, such as /Admin
            AreaRegistration.RegisterAllAreas();

            // Register the stock routes plus the plugin routes
            RouteConfig.RegisterRoutes(rc);

            // Add the new View Engine for our plugins to use
            ViewEngines.Engines.Add(new PluginRazorViewEngine());
        }

        // May need to store host in distributed or multi-tenant applications
        protected void Application_BeginRequest()
        {
            //string host = Request.Url.Host;      
        }
	}
}