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
		public JsonResult LoadMoreBlogs(int lastBlogId, int count, List<string> idList, string user = "", string date = "")
		{
			var result = new JsonResult();

			List<Blog> blogs = BlogListModel.GetBlogs(lastBlogId, count, idList, user, date);

			string html = RenderPartialViewToString("/Views/Shared/Partials/BlogArticleSinglePartial.cshtml", blogs, ControllerContext, ViewData, TempData);

            lastBlogId = blogs.Count < 1 ? 0 : blogs.LastOrDefault().BlogId;
			result.Data = new { html = html, lastBlogId = lastBlogId };

			return result;
		}

        public JsonResult LoadMorePopularBlogs(int lastBlogId, int count, List<string> idList)
        {
            var result = new JsonResult();

            List<Blog> blogs = BlogListModel.GetBlogs(lastBlogId, count, idList);

            string html = RenderPartialViewToString("/Views/Shared/Partials/PopularBlogSinglePartial.cshtml", blogs, ControllerContext, ViewData, TempData);

            lastBlogId = blogs.Count < 1 ? 0 : blogs.LastOrDefault().BlogId;
            result.Data = new { html = html, lastBlogId = lastBlogId };

            return result;
        }

        public JsonResult LoadMoreRelatedBlogs(int lastBlogId, int count, List<string> idList)
        {
            var result = new JsonResult();

            List<Blog> blogs = BlogListModel.GetBlogs(lastBlogId, count, idList);

            string html = RenderPartialViewToString("/Views/Shared/Partials/RelatedBlogSinglePartial.cshtml", blogs, ControllerContext, ViewData, TempData);

            lastBlogId = blogs.Count < 1 ? 0 : blogs.LastOrDefault().BlogId;
            result.Data = new { html = html, lastBlogId = lastBlogId };

            return result;
        }

        public JsonResult LoadMoreArchives(string lastMonth, int count, List<string> idList, string user = "", string date = "")
        {
            var result = new JsonResult();

            IEnumerable<string> archives = BlogListModel.GetArchives(lastMonth, count, idList, user, date);

            string html = RenderPartialViewToString("/Views/Shared/Partials/BlogArchiveSinglePartial.cshtml", archives, ControllerContext, ViewData, TempData);

            lastMonth = !archives.Any() ? "0" : "";
            result.Data = new { html = html, lastMonth = lastMonth };

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
