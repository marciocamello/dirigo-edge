using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DirigoEdge
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.LowercaseUrls = true;

			routes.MapRoute(
				name: "About",
				url: "about/{action}/{id}",
				defaults: new { controller = "Home", action = "About", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Contact",
				url: "contact/{action}/{id}",
				defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
			);

			// Blog Categories View
			// Ex: dirigodev.com/blog/categories/seo/
			routes.MapRoute(
				"Blog Categories View", // Route name
				"blog/categories/{category}", // URL with parameters
				new { controller = "Blog", action = "Categories", category = UrlParameter.Optional } // Parameter defaults
			);

			// Blog RSS Feed
			routes.MapRoute(
				"Blog RSS New Feeds", // Route name
				"Blog/NewPosts/", // URL with parameters
				new { controller = "Blog", action = "NewPosts", id = UrlParameter.Optional } // Parameter defaults
			);

			routes.MapRoute(
				name: "Blog",
				url: "blog/{title}",
				defaults: new { controller = "Blog", action = "Index", title = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Content",
				url: "content/{title}",
				defaults: new { controller = "Content", action = "Index", title = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Account",
				url: "account/{action}/{id}",
				defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
			);

			// Generated Site Map
			routes.MapRoute(
				name: "Sitemap",
				url: "sitemap.xml",
				defaults: new { controller = "Home", action = "SitemapXML" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}