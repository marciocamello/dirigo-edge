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
		public ActionResult Index(string title)
		{
			var model = new ContentViewViewModel(title);

			return View(model.TheTemplate.ViewLocation, model);
		}
    }
}
