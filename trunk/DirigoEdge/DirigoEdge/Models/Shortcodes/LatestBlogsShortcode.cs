using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Models.ViewModels;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.Modules
{
    public class LatestBlogsModule : IShortcode
    {
        public string GetDisplayName()
        {
            return "Latest Blogs";
        }

        public string GetCSSClass()
        {
            return "latestBlogs";
        }

        public string GetHtml(Dictionary<string, string> parameters)
        {
            int blogCount = 10; // Default Blogs to Show

            if (parameters.ContainsKey("count"))
            {
                blogCount = Int32.Parse(parameters["count"]);
            }

            var model = new LatestBlogsViewModel(blogCount);            

            return DynamicModules.GetViewHtml("/Views/Shared/Partials/LatestBlogsPartial.cshtml", model);
        }
    }
}