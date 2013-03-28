using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DirigoEdge.Models.ViewModels;
using DirigoEdge.Utils;

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

		// Generate a sitemap on request
		public XmlSitemapResult SitemapXML()
		{
			string hostUrl = HTTPUtils.GetFullyQualifiedApplicationPath();

			var items = new List<SitemapItem>()
			{
				new SitemapItem(hostUrl + "about/"),
				new SitemapItem(hostUrl + "blog/"),
				new SitemapItem(hostUrl + "contact/")	
			};

			// Add generated blogs, public content pages, etc.
			items.AddRange(Utils.Sitemap.GetGeneratedSiteMapItems(hostUrl));

			return new XmlSitemapResult(items);
		}
	}
}
