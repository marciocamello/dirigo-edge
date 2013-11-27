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
    public class EventAdminController : Controller
    {
		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult ModifyEvent(Event entity)
		{
			JsonResult result = new JsonResult();

			if (!String.IsNullOrEmpty(entity.Title))
			{
				using (var context = new DataContext())
				{
					Event editedEvent = context.Events.FirstOrDefault(x => x.EventId == entity.EventId);
					if (editedEvent != null)
					{
						editedEvent.HtmlContent = entity.HtmlContent;
						editedEvent.FeaturedImageUrl = scrubInput(entity.FeaturedImageUrl);
						editedEvent.IsActive = entity.IsActive;
						editedEvent.IsFeatured = entity.IsFeatured;
						editedEvent.Title = scrubInput(entity.Title);
						editedEvent.PermaLink = scrubInput(entity.PermaLink);
						editedEvent.MainCategory = scrubInput(entity.MainCategory);
                        editedEvent.EventCategoryId = entity.EventCategoryId;
						editedEvent.ShortDesc = entity.ShortDesc;						
                        editedEvent.StartDate = entity.StartDate;
                        editedEvent.EndDate = entity.EndDate;

						context.SaveChanges();
						
						result.Data = new { id = entity.EventId };
					}
				}
			}

			return result;
		}

	    [HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult DeleteEvent(string id)
		{
			JsonResult result = new JsonResult();

			if (String.IsNullOrEmpty(id))
			{
				return result;
			}

			int eventId = Int32.Parse(id);

			using (var context = new DataContext())
			{
				var thisEvent = context.Events.FirstOrDefault(x => x.EventId == eventId);

                context.Events.Remove(thisEvent);
				context.SaveChanges();
			}

			return result;
		}

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		public ActionResult UploadEventImage(IEnumerable<HttpPostedFileBase> files)
		{
			// The Name of the Upload component is "files"
			if (files != null)
			{
				foreach (var file in files)
				{
					// Some browsers send file names with full path.
					// We are only interested in the file name.
					var fileName = Path.GetFileName(file.FileName);
					var physicalPath = Path.Combine(Server.MapPath("~/Content/Uploaded/EventFeaturedImages/"), fileName);

					// The files are not actually saved in this demo
					file.SaveAs(physicalPath);
				}
			}

			// Return an empty string to signify success
			return Content("");
		}
		
		[Authorize(Roles = "Administrators")]
		public ActionResult LoadStockImages()
		{
			var sb = new StringBuilder();

			string directory = Server.MapPath("~" + Utils.ContentGlobals.STOCKEVENTIMAGESDIRECTORY);
			string[] images = Directory.GetFiles(directory, "*.jpg");
			foreach (string image in images)
			{
				string imgSrc = Utils.ContentGlobals.STOCKEVENTIMAGESDIRECTORY + Path.GetFileName(image);
				sb.Append(String.Format("<a href='javascript:void(0);' class='stockImage has-tip tip-top' title=\"<img src='{0}' />\" >", imgSrc));
				sb.Append(String.Format("<img src='{0}' alt='Stock Image' />", imgSrc));
				sb.Append("</a>");
			}

			return Content(sb.ToString());
		}

		#region Helper Methods

		private string scrubInput(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return String.Empty;
			}
			
			return input.Trim();
		}

		#endregion
    }

	public class EventEntity
	{
		public string Title { get; set; }
		public string HtmlContent { get; set; }
	}

	public class EventAdminModules
	{
		public List<EventAdminModule> EventAdminModulesColumn1 { get; set; }
		public List<EventAdminModule> EventAdminModulesColumn2 { get; set; }
	}
}