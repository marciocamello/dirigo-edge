using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using DirigoEdge.Models;
using DirigoEdge.Models.DataModels;
using DirigoEdge.Models.ViewModels;

namespace DirigoEdge.Controllers
{
    public class EventController : Controller
    {
        public ActionResult Index(string title)
        {
			// If Events aren't enabled we should 404
			if (!Utils.SiteSettingsUtils.EventsEnabled()) { throw new HttpException(404, "Page not Found"); }

			// Event Listing Homepage
			if (String.IsNullOrEmpty(title))
			{
				var model = new EventHomeViewModel();

				return View("~/Views/Home/Event.cshtml", model);
			}
			// Individual Event
			else
			{
				var model = new EventSingleHomeViewModel(title);

				return View("~/Views/Home/EventSingle.cshtml", model);
			}
        }

	    public ActionResult Categories(string category)
		{
			// If Events aren't enabled we should 404
			if (!Utils.SiteSettingsUtils.EventsEnabled()) { throw new HttpException(404, "Page not Found"); }

			// Event Listing Homepage
			if (String.IsNullOrEmpty(category))
			{
				var model = new EventCategoryHomeViewModel();

                return View("~/Views/Home/Event.cshtml", model);
			}
			// Individual Event
			else
			{
				var model = new EventCategorySingleViewModel(category, Server);

				return View("~/Views/Event/CategoriesSingle.cshtml", model);
			}
		}
    }
}
