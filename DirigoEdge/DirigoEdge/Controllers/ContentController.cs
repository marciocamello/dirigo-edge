using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirigoEdge.Models.ViewModels;

namespace DirigoEdge.Controllers
{
    public class ContentController : Controller
    {
		public ActionResult Index()
		{
            //Remove query string
            Uri thisUri = new Uri(Request.Url.GetLeftPart(UriPartial.Path));

            // Check for content pages before returning a 404
            string title = thisUri.PathAndQuery;

            // remove beginning and ending slashes from uri
            if (title.StartsWith("/"))
            {
                title = title.Substring(1);
            }
            if (title.EndsWith("/"))
            {
                title = title.Substring(0, title.Length - 1);
            }

			var model = new ContentViewViewModel(title);

			if (model.ThePage != null)
			{
			    return View(model.TheTemplate.ViewLocation, model);
			}
			else
			{
                HttpContext.Response.StatusCode = 404;
                return View("~/Views/Home/Error404.cshtml");    
			}
		}
    }
}
