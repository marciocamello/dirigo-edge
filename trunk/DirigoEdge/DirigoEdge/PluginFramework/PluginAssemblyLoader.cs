using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace DirigoEdge.PluginFramework
{
    public static class PluginAssemblyLoader
    {
        private const string _pluginLocation = "/Plugins";

        // Load all Plugins from plugin Directory
        public static void Load(RouteCollection rc)
        {
            string fullPluginPath = HttpContext.Current.Server.MapPath(string.Format("~{0}", _pluginLocation));

            if (!Directory.Exists(fullPluginPath))
            {
                return;
            }

            var pluginListing = Directory.EnumerateFiles(fullPluginPath);

            // Loop through each dll in the folder and add them to the collection
            foreach (string pluginFile in pluginListing)
            {
                if (pluginFile.EndsWith(".dll"))
                {
                    try
                    {
                        Assembly controllersAssembly = Assembly.LoadFrom(pluginFile);

                        // Now that we've loaded all dll's, register the actions and filters
                        //Loop through all opened assemblies in the current AppDomain
                        foreach (Type t in controllersAssembly.GetTypes())
                        {
                            if (t.GetInterface("IPlugin") != null)
                            {
                                try
                                {
                                    IPlugin pluginclass = Activator.CreateInstance(t) as IPlugin;

                                    if (pluginclass != null)
                                    {
                                        pluginclass.RegisterActions();
                                        pluginclass.RegisterFilters();

                                        // Add the routes
                                        var x = pluginclass.RegisterRoutes();
                                        foreach (var y in x)
                                        {
                                            rc.Add(y);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    // Log loading issue
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // log to file
                    }
                }
            }
        }
    }
}