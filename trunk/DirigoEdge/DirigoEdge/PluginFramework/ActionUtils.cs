using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using DirigoEdge.Utils;

namespace DirigoEdge.PluginFramework
{
    public static class ActionUtils
    {
        private static readonly object ActionCollectionLock = new object();

        public static void RegisterAction(string actionName, Func<string> function)
        {
            var actionList = GetActions();
            if (actionList.ContainsKey(actionName))
            {
                actionList[actionName].Add(function);
            }
            else
            {
                actionList.Add(actionName, new List<Func<string>>() { function });
            }

            HttpRuntime.Cache["ActionList"] = actionList;
        }

        public static string DoAction(string action)
        {
            var output = new StringBuilder();
            var actionList = GetActions();

            if (actionList.ContainsKey(action))
            {
                foreach (var fAction in actionList[action])
                {
                    output.Append(fAction());
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// View Helper to Render a Plugin Action
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static MvcHtmlString DoAction(this HtmlHelper helper, string action)
        {
            return new MvcHtmlString(DoAction(action));
        }

        public static Dictionary<string, List<Func<string>>> GetActions()
        {
            return GetActions(false);
        }

        public static Dictionary<string, List<Func<string>>> GetActions(bool recycleCache)
        {
            var actionList = (Dictionary<string, List<Func<string>>>)HttpRuntime.Cache["ActionList"];
            if (actionList == null || recycleCache)
            {
                // Lock cache so simultaneous requests don't also perform this query
                lock (ActionCollectionLock)
                {
                    // If we were locked out, check the list again (double-checked locking - http://en.wikipedia.org/wiki/Double-checked_locking)
                    actionList = (Dictionary<string, List<Func<string>>>)HttpRuntime.Cache["ActionList"];

                    if (actionList != null && !recycleCache && actionList.Count != 0)
                    {
                        return actionList;
                    }

                    // If no actionlist found, create a new one
                    actionList = new Dictionary<string, List<Func<string>>>();

                    HttpRuntime.Cache.Insert("ActionList", actionList, null, DateTime.Now.AddYears(5), Cache.NoSlidingExpiration);
                }
            }

            return actionList;
        }

        public static void RegisterModule(string shortcode, IShortcode module)
        {
            DynamicModules.Instance.AddDynamicModule(shortcode, module);
        }
    }
}