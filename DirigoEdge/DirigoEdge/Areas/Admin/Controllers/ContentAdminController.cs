using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class ContentAdminController : Controller
    {
		[Authorize(Roles = "Administrators")]
		public JsonResult SetWordWrap(bool wordWrap)
		{
			var userName = Membership.GetUser().UserName;

			if (!String.IsNullOrEmpty(userName))
			{
				using (var context = new DataContext())
				{
					context.Users.FirstOrDefault(x => x.Username == userName).ContentAdminWordWrap = wordWrap;

					context.SaveChanges();
				}
			}

			return new JsonResult();
		}

	    [Authorize(Roles = "Administrators")]
		public JsonResult GetRevisionHtml(int revisionId)
	    {
			JsonResult result = new JsonResult();
		    string html = "";

			using (var context = new DataContext())
			{
				html = context.ContentPageRevisions.Where(x => x.ContentPageRevisionId == revisionId).FirstOrDefault().ContentHtml;
			}

			result.Data = new { html = html };

		    return result;
	    }

		[Authorize(Roles = "Administrators")]
		public JsonResult GetRevisionList(int id)
	    {
			JsonResult result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		    string html = "";

			using (var context = new DataContext())
			{
				List<ContentPageRevision> revisions = context.ContentPageRevisions.Where(x => x.ContentPageId == id).OrderByDescending(x => x.DateCreated).ToList();
				html = RenderPartialViewToString("/Areas/Admin/Views/Shared/Partials/RevisionsListInnerPartial.cshtml", revisions, ControllerContext, ViewData, TempData);
			}

			result.Data = new { html = html };

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