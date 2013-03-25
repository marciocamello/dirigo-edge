using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DirigoEdge.Models.ViewModels;

namespace DirigoEdge.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var model = new HomeViewModel();
			return View(model);
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Blog()
		{
			return View();
		}

		public ActionResult Contact()
		{
			return View();
		}
	}
}
