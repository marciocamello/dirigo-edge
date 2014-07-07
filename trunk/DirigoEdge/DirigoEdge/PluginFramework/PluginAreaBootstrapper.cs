using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using DirigoEdge.Entities;

namespace DirigoEdge.PluginFramework
{
    public class PluginAreaBootstrapper
    {
        public static readonly List<Assembly> PluginAssemblies = new List<Assembly>();

        public static List<string> PluginNames()
        {
            return PluginAssemblies.Select(pluginAssembly => pluginAssembly.GetName().Name).ToList();
        }

        public static void Init()
        {
            string fullPluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

            // If no Plugins Directory if found then there is no need to continue
            if (!Directory.Exists(fullPluginPath))
            {
                return;
            }

            // First check to see if we need to remove any dll's so we can install a new one
            using (var context = new DataContext())
            {
                SiteSettings siteSettings = null;
                try
                {
                    siteSettings = context.SiteSettings.FirstOrDefault();
                }                   
                catch (Exception e)
                {
                    // Catch Model Update issue
                    // If model can be updated without data loss, go for it
                    var migratorConfig = new Migrations.Configuration();
                    var dbMigrator = new DbMigrator(migratorConfig);
                    dbMigrator.Update();
                }

                if (siteSettings != null && !String.IsNullOrEmpty(siteSettings.RMPluginDLLPath))
                {
                    // Remove the dll
                    if (File.Exists(siteSettings.RMPluginDLLPath))
                    {
                        try
                        {
                            File.Delete(siteSettings.RMPluginDLLPath);

                            // Reset the path then reset the app pool again
                            siteSettings.RMPluginDLLPath = String.Empty;
                            context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            // Probably had a file lock - should log to notificaitons api
                        }
                    }

                    // Remove the dll's folder if it exists
                    string pluginDir = siteSettings.RMPluginDLLPath.Replace(".dll", "");
                    if (Directory.Exists(pluginDir))
                    {
                        Directory.Delete(pluginDir, true);
                    }
                }
            }

            // Now check to see if we need to extract any plugins
            foreach (var file in Directory.EnumerateFiles(fullPluginPath, "*.zip"))
            {
                try
                {
                    // Extract Zip File
                    ZipFile.ExtractToDirectory(file, fullPluginPath);

                    // Delete the Zip file now that it has been extracted
                    File.Delete(file);
                }
                catch (Exception e)
                {
                    // Gotta.. catch..
                }
            }

            // Finally, load all plugins
            foreach (var file in Directory.EnumerateFiles(fullPluginPath, "*.dll"))
            {
                PluginAssemblies.Add(Assembly.LoadFile(file));
            }

            PluginAssemblies.ForEach(BuildManager.AddReferencedAssembly);
        }
    }
}