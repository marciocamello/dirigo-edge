using System;
using System.Collections.Generic;
using System.Linq;
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

			using (DataContext context = new DataContext())
			{
				var tempList = context.ContentModules.Where(x => x.ModuleName == name).ToList();

				if (tempList.Count > 0)
				{
					output = tempList.FirstOrDefault().HTMLContent;

					// Give admins a shortcut to edit content
					if (Utils.UserUtils.UserIsAdmin())
					{
						output = String.Format("<a class='adminEdit' href='/admin/editmodule/{0}' target='_blank'>Edit</a>", tempList.FirstOrDefault().ContentModuleId) + output;
					}
				}
			}

			return output;
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
			else
			{
				return blogTitle.ToLower().Replace(" ", ContentGlobals.BLOGDELIMMETER);
			}
		}
	}
}