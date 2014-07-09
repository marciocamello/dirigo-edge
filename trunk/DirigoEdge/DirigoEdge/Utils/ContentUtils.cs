using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DirigoEdge.Utils
{
    public static class ContentUtils
    {
        /// <summary>
        /// Retrieves a content module from the DB by name. 
        /// If user is logged in as admin while viewing the content, an 'edit' link is shown for quick navigation to the edit module page
        /// </summary>
        /// <param name="name">The name of the field in SQL / Admin interface</param>
        /// <returns>Raw HTML Content. If user is admin, an anchor is prepended for admin access.</returns>
        public static string GetContentModuleByName(string name)
        {
            string output = String.Empty;

            using (var context = new DataContext())
            {
                var tempList = context.ContentModules.Where(x => x.ModuleName == name).ToList();

                if (tempList.Count > 0)
                {
                    output = tempList.FirstOrDefault().HTMLContent;

                    // Give admins a shortcut to edit content
                    if (UserUtils.UserIsAdmin())
                    {
                        output = String.Format("<a class='adminEdit' href='/admin/editmodule/{0}' target='_blank'>Edit</a>", tempList.FirstOrDefault().ContentModuleId) + output;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Return the html with any included modules inserted into the html.
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns>Html with module html and scripts in place</returns>
        public static string GetFormattedPageContent(string pageContent)
        {
            if (String.IsNullOrEmpty(pageContent))
            {
                return pageContent;
            }

            // pull everything in-between brackets. Ex : [text] will extract "text".
            const string pattern = @"\[(.*?)\]";
            var matches = Regex.Matches(pageContent, pattern);

            using (var context = new DataContext())
            {
                // Run through each matched tag and replace with module html if found. Otherwise leave the tag alone
                foreach (Match m in matches)
                {
                    var tag = m.Groups[1].ToString();

                    var module = context.ContentModules.Where(x => x.ModuleName == tag).FirstOrDefault();

                    if (module != null)
                    {
                        string htmlContent = String.Format("<div class='shortCodeInsert sortableModule' data-name='{0}'>{1}</div>", module.ModuleName,
                                                           module.HTMLContent);

                        // May want to append edit button here if admin
                        //htmlContent += "<script class='moduleScript'>" + module.JSContent + "</script>";

                        pageContent = pageContent.Replace("[" + tag + "]", htmlContent);
                    }
                    // Otherwise check dynamic modules
                    else
                    {
                        ShortCodeObject shortCode = GetFormattedShortCodeObject(tag);
                        DynamicModules dm = DynamicModules.Instance;

                        if (dm.GetDynamicModuleList().ContainsKey(shortCode.Name))
                        {
                            var list = dm.GetDynamicModuleList();
                            if (list.ContainsKey(shortCode.Name))
                            {
                                string htmlContent = String.Format("<div class='dynamicCodeInsert sortableModule {2}' data-name='{0}'>{1}</div>", tag,
                                                           list[shortCode.Name].GetHtml(shortCode.Parameters), list[shortCode.Name].GetCSSClass());

                                pageContent = pageContent.Replace("[" + tag + "]", htmlContent);
                            }
                        }
                    }
                }
            }

            return pageContent;
        }


        /// <summary>
        /// Return the html with any included modules inserted into the html. Script data is not included in html.
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns>Formatted Html Content with Module Contents and a list of script html</returns>
        public static PageDataCollection GetFormattedPageContentAndScripts(string pageContent)
        {
            var collection = new PageDataCollection()
            {
                HtmlFormatted = pageContent,
                ScriptContents = new List<string>(),
                StylesContents = new List<string>()
            };

            // Save a lookup
            if (String.IsNullOrEmpty(pageContent))
            {
                return collection;
            }

            // pull everything in-between brackets. Ex : [text] will extract "text".
            const string pattern = @"\[(.*?)\]";
            var matches = Regex.Matches(pageContent, pattern);

            using (var context = new DataContext())
            {
                // Run through each matched tag and replace with module html if found. Otherwise leave the tag alone
                foreach (Match m in matches)
                {
                    string tag = m.Groups[1].ToString();

                    var module = context.ContentModules.Where(x => x.ModuleName == tag).FirstOrDefault();
                    if (module != null)
                    {
                        string htmlContent = String.Format("<div class='shortCodeInsert sortableModule' data-name='{0}'>{1}</div>", module.ModuleName, module.HTMLContent);

                        collection.ScriptContents.Add(module.JSContent);
                        collection.StylesContents.Add(module.CSSContent);

                        pageContent = pageContent.Replace("[" + tag + "]", htmlContent);
                    }
                    // Otherwise check dynamic modules
                    else
                    {

                        ShortCodeObject shortCode = GetFormattedShortCodeObject(tag);

                        DynamicModules dm = DynamicModules.Instance;
                        if (dm.GetDynamicModuleList().ContainsKey(shortCode.Name))
                        {
                            var list = dm.GetDynamicModuleList();
                            if (list.ContainsKey(shortCode.Name))
                            {
                                string htmlContent = String.Format("<div class='dynamicCodeInsert sortableModule {2}' data-name='{0}' data-tag='{3}'>{1}</div>", tag,
                                                           list[shortCode.Name].GetHtml(shortCode.Parameters), list[shortCode.Name].GetCSSClass(), tag);

                                pageContent = pageContent.Replace("[" + tag + "]", htmlContent);
                            }
                        }
                    }
                }
            }

            // update the collection's html entity once we're done parsing
            collection.HtmlFormatted = pageContent;

            return collection;
        }


        /// <summary>
        /// Parses a shortcode into an object. I.e. [BlogListing Count="1"] will return "BlogListing" for name and a KVP of ["Count", "1"].
        /// </summary>
        /// <param name="tag">The Shortcode in it's entirety. I.e. [BlogListing Count="1"]</param>
        /// <returns></returns>
        public static ShortCodeObject GetFormattedShortCodeObject(string tag)
        {
            var code = new ShortCodeObject();

            // parse the shortcode into the name and ordered key value pairs of parameters.

            // if there are no parameters, just return the tag into an object
            if (!tag.Contains("="))
            {
                // Remove the brackets and trim any white space
                code.Name = tag.Replace("[", String.Empty).Replace("]", String.Empty).Trim();
                return code;
            }

            // split the tag by spaces to get the name, then the parameters
            List<string> splitData = tag.Split(' ').ToList();

            if (splitData.Count > 0)
            {
                code.Name = splitData.FirstOrDefault();
                code.Parameters = new Dictionary<string, string>();

                // Get the parameters
                string tagNoName = tag.Replace(code.Name + " ", ""); // Remove the tag name so we only have parameters left

                // Get each key / value pair where a value is inside of quotes
                const string pattern = @"<=.*(?="")|[\w\s]+|""[\w\s./-]*""";
                var matches = Regex.Matches(tagNoName, pattern);

                // Now setup the kvp's
                int count = 0;
                foreach (Match match in matches)
                {
                    // Even means we're looking at a key
                    if (count % 2 == 0)
                    {
                        string key = match.Value;
                        string value = matches[count + 1].Value;
                        value = value.Replace("\"", "");

                        code.Parameters.Add(key, value);
                    }

                    count++;
                }
            }

            return code;
        }

        /// <summary>
        /// Returns a blog title as formatted with system delimmeter
        /// </summary>
        /// <param name="input">Title of the blog</param>
        /// <returns>Ex: 'My Blog' -> 'my-blog'</returns>
        public static string GetFormattedUrl(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return String.Empty;
            }

            // Replace spaces with dashes or other delimmeter
            input.ToLower().Replace(" ", ContentGlobals.BLOGDELIMMETER);

            // Only allow alpha numeric and dashes
            Regex rgx = new Regex("[^a-zA-Z0-9-]");
            input = rgx.Replace(input, "");

            return input;
        }
        
        public static string RenderPartialViewToString(string viewName, object model, ControllerContext context, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            viewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);

                if (viewResult == null || viewResult.View == null)
                {
                    return "";
                }

                ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }

    public class PageDataCollection
    {
        public string HtmlFormatted;
        public List<string> ScriptContents;
        public List<string> StylesContents;
    }

    public class ShortCodeObject
    {
        public string Name;
        public Dictionary<string, string> Parameters;
    }
}