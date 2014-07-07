using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DirigoEdge.Entities;
using DirigoEdge.Utils;

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
			var result = new JsonResult();
		    string html = "";

			using (var context = new DataContext())
			{
				html = context.ContentPages.Where(x => x.ContentPageId == revisionId).FirstOrDefault().HTMLContent;
			}

			result.Data = new { html = html ?? String.Empty };

		    return result;
	    }

		[Authorize(Roles = "Administrators")]
		public JsonResult GetRevisionList(int id)
	    {
			JsonResult result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		    string html = "";

			using (var context = new DataContext())
			{
				List<ContentPage> drafts = context.ContentPages.Where(x => x.ParentContentPageId == id || x.ContentPageId == id).OrderByDescending(x => x.PublishDate).ToList();
                html = RenderPartialViewToString("/Areas/Admin/Views/Shared/Partials/RevisionsListInnerPartial.cshtml", drafts, ControllerContext, ViewData, TempData);
			}

			result.Data = new { html = html };

		    return result;
	    }

		[Authorize(Roles = "Administrators")]
		public JsonResult UploadModuleThumb(HttpPostedFileBase file)
		{
			if (file != null)
			{
				var fileName = Path.GetFileName(file.FileName);
				var physicalPath = Path.Combine(Server.MapPath("~" + Utils.ContentGlobals.MODULETHUMBNAILIMAGEUPLOADDIRECTORY), fileName);

				file.SaveAs(physicalPath);
			}

			string imgPath = Utils.ContentGlobals.MODULETHUMBNAILIMAGEUPLOADDIRECTORY + file.FileName;

			return new JsonResult() { Data = new { path = imgPath } };
		}

        [Authorize(Roles = "Administrators")]
        public JsonResult SaveSchema(int id, string data, string name)
        {
            var result = new JsonResult();

            using (var context = new DataContext())
            {
                var schema = context.Schemas.FirstOrDefault(x => x.SchemaId == id);

                if (schema != null)
                {
                    schema.JSONData = data;
                    schema.DisplayName = name;
                }

                context.SaveChanges();
            }

            result.Data = new {  };

            return result;
        }

        [Authorize(Roles = "Administrators")]
        public JsonResult GetSchemaHtml(int schemaId, int moduleId, bool isPage = false)
        {
            var result = new JsonResult();
            Schema theSchema;
            
            string entryValues = String.Empty;

            using (var context = new DataContext())
            {
                theSchema = context.Schemas.FirstOrDefault(x => x.SchemaId == schemaId);

                if (isPage)
                {
                    ContentPage thePage = context.ContentPages.FirstOrDefault(x => x.ContentPageId == moduleId);
                    entryValues = thePage.SchemaEntryValues;
                }
                else
                {
                    ContentModule theModule = context.ContentModules.FirstOrDefault(x => x.ContentModuleId == moduleId);
                    entryValues = theModule.SchemaEntryValues;
                }                
            }

            if (theSchema != null)
            {
                result.Data = new { data = theSchema.JSONData, values = entryValues };    
            }

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