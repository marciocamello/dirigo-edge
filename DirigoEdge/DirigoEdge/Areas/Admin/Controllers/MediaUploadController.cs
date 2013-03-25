using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class MediaUploadController : Controller
    {
	    [HttpPost]
	    [Authorize(Roles = "Administrators")]
	    [AcceptVerbs(HttpVerbs.Post)]
		public JsonResult UploadFile(HttpPostedFileBase media)
	    {
			var result = new JsonResult();

			if (media != null)
			{
				var fileName = Path.GetFileName(media.FileName);
				var physicalPath = Path.Combine(Server.MapPath("~/Content/Uploaded/Media/"), fileName);

				media.SaveAs(physicalPath);
			}

		    return result;
	    }

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult ViewDirectory(string dir)
		{
			var result = new JsonResult();

			string directory = Server.MapPath("~/Content/Uploaded/Media");
			var images = Directory.GetFiles(directory, "*.*")
								.Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("gif"))
								.ToList();





			//string[] images = Directory.GetFiles(directory, "*.jpg, *.gif, *.jpeg, *.png");
			
			StringBuilder sb = new StringBuilder();
			sb.Append("<ul id='UploadedMediaListing'>");
			
			
			foreach (string image in images)
			{
				string imgSrc = String.Format("/Content/Uploaded/Media/{0}", Path.GetFileName(image));
				sb.Append("<li>");
				sb.Append(String.Format("<a href='javascript:void(0);' class='stockImage th has-tip tip-top' title=\"<img class='constrained' src='{0}' />\" >", imgSrc));
				sb.Append(String.Format("<img src='{0}' alt='Stock Image' />", imgSrc));
				sb.Append("</a>");
				sb.Append("</li>");
			}
			sb.Append("</ul>");

			result.Data = new { html = sb.ToString()};
			return result;
		}
    }
}
