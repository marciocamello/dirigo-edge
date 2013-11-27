using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

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
        /// Return the html with any included static or dynamic modules inserted into the html.
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
                        string htmlContent = String.Format("<div class='shortCodeInsert' data-name='{0}'>{1}</div>", module.ModuleName,
                                                           module.HTMLContent);

                        // May want to append edit button here if admin
                        htmlContent += "<script class='moduleScript'>" + module.JSContent + "</script>";

                        pageContent = pageContent.Replace("[" + tag + "]", htmlContent);
                    }
                    // Otherwise check dynamic modules
                    else
                    {
                        DynamicModules dm = DynamicModules.Instance;

                        if (dm.GetDynamicModuleList().ContainsKey(tag))
                        {
                            var list = dm.GetDynamicModuleList();
                            if (list.ContainsKey(tag))
                            {
                                string htmlContent = String.Format("<div class='dynamicCodeInsert editor-removable {2}' data-name='{0}'>{1}</div>", list[tag].GetModuleName(),
                                                           list[tag].GetHtml(), list[tag].GetCSSClass());

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
					var tag = m.Groups[1].ToString();
					var module = context.ContentModules.Where(x => x.ModuleName == tag).FirstOrDefault();

					if (module != null)
					{
						string htmlContent = String.Format("<div class='shortCodeInsert' data-name='{0}'>{1}</div>", module.ModuleName, module.HTMLContent);

						collection.ScriptContents.Add(module.JSContent);
						collection.StylesContents.Add(module.CSSContent);

						pageContent = pageContent.Replace("[" + tag + "]", htmlContent);
					}
                    // Otherwise check dynamic modules
                    else
                    {
                        DynamicModules dm = Utils.DynamicModules.Instance;
                        if (dm.GetDynamicModuleList().ContainsKey(tag))
                        {
                            var list = dm.GetDynamicModuleList();
                            if (list.ContainsKey(tag))
                            {
                                string htmlContent = String.Format("<div class='dynamicCodeInsert editor-removable {2}' data-name='{0}' data-tag='{3}'>{1}</div>", list[tag].GetModuleName(),
                                                           list[tag].GetHtml(), list[tag].GetCSSClass(), tag);

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
		/// Returns a blog title as formatted with system delimmeter
		/// </summary>
		/// <param name="blogTitle">Title of the blog</param>
		/// <returns>Ex: 'My Blog' -> 'my-blog'</returns>
		public static string GetFormattedUrl(string blogTitle)
		{
			if (String.IsNullOrEmpty(blogTitle))
			{
				return String.Empty;
			}
			
			return blogTitle.ToLower().Replace(" ", ContentGlobals.BLOGDELIMMETER);
		}
	}

	public class PageDataCollection
	{
		public string HtmlFormatted;
		public List<string> ScriptContents;
		public List<string> StylesContents;

	}
}