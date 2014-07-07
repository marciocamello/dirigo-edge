using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class PluginsController : Controller
    {
        private const string pluginDir = "/Plugins/";

        public ActionResult InstallPlugin(Plugin plugin)
        {
            // Install or update plugin information in database
            using (var context = new DataContext())
            {
                var pluginToUpdate = context.Plugins.Where(x => x.AppStoreId == plugin.AppStoreId).FirstOrDefault();

                // Update version information if it's already installed
                if (pluginToUpdate != null)
                {
                    pluginToUpdate.Version = plugin.Version;
                    pluginToUpdate.Description = plugin.Description;
                    pluginToUpdate.PluginDisplayName = pluginToUpdate.PluginDisplayName;
                }
                // Otherwise insert record into database if new install
                else
                {
                    var pluginToInsert = new Plugin();
                    pluginToInsert.AppStoreId = plugin.AppStoreId;
                    pluginToInsert.FileLocation = plugin.FileLocation;
                    pluginToInsert.Version = plugin.Version;
                    pluginToInsert.Description = plugin.Description;
                    pluginToInsert.PluginDisplayName = plugin.PluginDisplayName;

                    context.Plugins.Add(pluginToInsert);
                }

                context.SaveChanges();
            }

            // Download the zip file, then extract it to the /Plugins directory
            using(var client = new WebClient() )
            {
                string filename = Path.GetFileName(plugin.FileLocation);
                string zipFileTemp = pluginDir + filename;
                string downloadZipTo = Server.MapPath(zipFileTemp);

                client.DownloadFile(plugin.FileLocation, downloadZipTo);

                // If the dll already exists, then we need to delete it before installing
                // This is tricky since the file is in use
                // We'll need to save the file to delete to the database, and the .zuo file to install should remain in the /Plugins directory
                // * the restart the app pool to delete the file, then install the plugin..                 
                string dllLocation = downloadZipTo.Replace(".zip", ".dll");

                // If the file exists, mark it for deletion upon next app pool restart
                if (System.IO.File.Exists(dllLocation))
                {
                    using (var context = new DataContext())
                    {
                        var siteSettings = context.SiteSettings.FirstOrDefault();
                        siteSettings.RMPluginDLLPath = dllLocation;

                        context.SaveChanges();
                    }
                }
                else
                {
                    // Extract Zip File
                    ZipFile.ExtractToDirectory(downloadZipTo, Server.MapPath(pluginDir));

                    // Delete the Zip file
                    System.IO.File.Delete(downloadZipTo);
                }
                
                // Reset app pool to install plugin or start removal process of existing plugin
                HttpRuntime.UnloadAppDomain();
            }

            var result = new JsonResult();
            return result;
        }

        public ActionResult RemovePlugin(Plugin plugin)
        {

            using (var context = new DataContext())
            {

                // Remove entry from database
                var pluginToRemove = context.Plugins.Where(x => x.AppStoreId == plugin.AppStoreId).FirstOrDefault();

                if (pluginToRemove != null)
                {
                    context.Plugins.Remove(pluginToRemove);
                    context.SaveChanges();
                }

                // Mark Plugin For Deletion
                string filename = Path.GetFileName(plugin.FileLocation);
                string zipFileTemp = pluginDir + filename;
                string dllLocation = Server.MapPath(zipFileTemp).Replace(".zip", ".dll");

                var siteSettings = context.SiteSettings.FirstOrDefault();
                siteSettings.RMPluginDLLPath = dllLocation;

                context.SaveChanges();

                // Reset app pool to istart removal process of existing plugin
                HttpRuntime.UnloadAppDomain();
            }

            return new JsonResult();;
        }
    }
}