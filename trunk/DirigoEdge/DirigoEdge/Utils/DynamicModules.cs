using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        private Dictionary<string, IDynamicModule> _dynamicModuleList;

        // This is where dynamic module function definitions are stores. Accepts a string argument (module / shortcode name), returns string (html content)
        public Dictionary<string, IDynamicModule> GetDynamicModuleList()
        {
            if (_dynamicModuleList == null)
            {
                // Create the initial list and populate it with default dynamic modules.
                _dynamicModuleList = new Dictionary<string, IDynamicModule>();

                // Test Dynamic Module for demo purposes
                _dynamicModuleList.Add("test", new TestModule());
            }

            return _dynamicModuleList;
        }

        /// <summary>
        /// Add a dynamic module programmatically during runtime.
        /// </summary>
        /// <param name="shortTag"></param>
        /// <param name="module"></param>
        /// <returns>True on success, false on error (tyopically if already added or key exists in dictionary</returns>
        public bool AddDynamicModule(string shortTag, IDynamicModule module)
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
    }

    public interface IDynamicModule
    {
        // The formal name of the object used for display purposes
        string GetModuleName();

        // Appended to the wrapper div for stying purposes
        string GetCSSClass();

        // Return method for html to be consumed
        string GetHtml(params object[] parameters);
    }

    public class TestModule : IDynamicModule
    {
        public string GetModuleName()
        {
            return "Test Module";
        }

        public string GetCSSClass()
        {
            return "testModule";
        }

        public string GetHtml(params object[] parameters)
        {
            return DateTime.Now.ToShortDateString();
        }
    }
}