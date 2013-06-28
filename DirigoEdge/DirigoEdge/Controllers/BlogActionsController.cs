using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirigoEdge.Models;

namespace DirigoEdge.Controllers
{
    public class BlogActionsController : Controller
    {
		public JsonResult LoadMoreBlogs(int lastBlogId, int count)
		{
			var result = new JsonResult();

			List<Blog> blogs = BlogListModel.GetBlogs(lastBlogId, count);

			string html = RenderPartialViewToString("/Views/Shared/Partials/BlogArticlePartial.cshtml", blogs, ControllerContext, ViewData, TempData);

			result.Data = new { html = html, lastBlogId = blogs.LastOrDefault().BlogId };

			return result;
		}

        // ===================================//
		// ==== Utility / Helper Methods ==== //
		// ===================================//

		public static string RenderPartialViewToString(string viewName, object model, ControllerContext context, ViewDataDictionary viewData, TempDataDictionary tempData)
		{
			viewData.Model = model;

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
				ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, tempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}

    }
}
