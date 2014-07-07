using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.Shortcodes
{
    public class CurrentYearShortcode : IShortcode
    {
        public string GetDisplayName()
        {
            return "Current Year";
        }

        public string GetCSSClass()
        {
            return "currentYearModule";
        }

        public string GetHtml(Dictionary<string, string> parameters)
        {
            return DateTime.Now.Year.ToString();
        }
    }
}