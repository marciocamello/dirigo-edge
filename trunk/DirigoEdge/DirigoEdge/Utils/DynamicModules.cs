using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DirigoEdge.Models.Modules;
using DirigoEdge.Models.Shortcodes;

namespace DirigoEdge.Utils
{
    public sealed class DynamicModules
    {
        static readonly DynamicModules _instance = new DynamicModules();

        public static DynamicModules Instance
        {
            get { return _instance; }
        }

        // private constructor for singleton pattern
        private DynamicModules()
        {

        }

        private Dictionary<string, IShortcode> _dynamicModuleList;

        // This is where dynamic module function definitions are stores. Accepts a string argument (module / shortcode name), returns string (html content)
        public Dictionary<string, IShortcode> GetDynamicModuleList()
        {
            if (_dynamicModuleList == null)
            {
                // Create the initial list and populate it with default dynamic modules.
                _dynamicModuleList = new Dictionary<string, IShortcode>();

                // Default Shortcodes for demo purposes
                _dynamicModuleList.Add("current_year", new CurrentYearShortcode());
                _dynamicModuleList.Add("latest_blogs", new LatestBlogsModule());

                // Add any additional shortcodes here, 
                // *or you can dynamically add them using "AddDynamicModule" from another function

            }

            return _dynamicModuleList;
        }

        /// <summary>
        /// Add a dynamic module programmatically during runtime.
        /// </summary>
        /// <param name="shortTag"></param>
        /// <param name="module"></param>
        /// <returns>True on success, false on error (tyopically if already added or key exists in dictionary</returns>
        public bool AddDynamicModule(string shortTag, IShortcode module)
        {
            // Pre-populate the list if necessary
            var moduleList = GetDynamicModuleList();

            // Replace the shortcode if it already exists
            if (moduleList.ContainsKey(shortTag))
            {
                _dynamicModuleList[shortTag] = module;
                return true;
            }

            // Add the module
            if (module != null)
            {
                _dynamicModuleList.Add(shortTag, module);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a dynamic module from the list
        /// </summary>
        /// <param name="shortTag"></param>
        /// <returns></returns>
        public bool RemoveDynamicModule(string shortTag)
        {
            // Pre-populate the list if necessary
            var moduleList = GetDynamicModuleList();

            // Replace the shortcode if it already exists
            if (moduleList.ContainsKey(shortTag))
            {
                _dynamicModuleList.Remove(shortTag);
                return true;
            }

            return false;
        }

        public static string GetViewHtml(string partialViewName, object model)
        {
            //Fake a context
            var context = new ControllerContext();
            context.RouteData = HttpContext.Current.Request.RequestContext.RouteData;
            context.RequestContext = HttpContext.Current.Request.RequestContext;

            ViewEngineResult result = ViewEngines.Engines.FindPartialView(context, partialViewName);

            // Try a regular view if partial not found
            if (result.View == null)
            {
                result = ViewEngines.Engines.FindView(context, partialViewName, "_layout");
            }

            var viewData = new ViewDataDictionary(model);
            var tempData = new TempDataDictionary();

            if (result.View != null)
            {
                var sb = new StringBuilder();
                using (var sw = new StringWriter(sb))
                {
                    using (var output = new HtmlTextWriter(sw))
                    {
                        var viewContext = new ViewContext(context, result.View, viewData, tempData, output);
                        result.View.Render(viewContext, output);
                    }
                }

                return sb.ToString();
            }
            return String.Empty;
        }
    }    

    public interface IShortcode
    {
        // The formal name of the object used for display purposes
        string GetDisplayName();

        // Appended to the wrapper div for stying purposes
        string GetCSSClass();

        // Return method for html to be consumed
        string GetHtml(Dictionary<string, string> parameters);
    }
}