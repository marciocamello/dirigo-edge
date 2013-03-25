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
    public class BlogController : Controller
    {
        public ActionResult Index(string title)
        {
			// Blog Listing Homepage
			if (String.IsNullOrEmpty(title))
			{
				var model = new BlogHomeViewModel();

				return View("~/Views/Home/Blog.cshtml", model);
			}
			// Individual Blog
			else
			{
				var model = new BlogSingleHomeViewModel(title);

				return View("~/Views/Home/BlogSingle.cshtml", model);
			}
        }

		public ActionResult Categories(string category)
		{
			// Blog Listing Homepage
			if (String.IsNullOrEmpty(category))
			{
				var model = new CategoryHomeViewModel();

				return View("~/Views/Blog/CategoriesHome.cshtml", model);
			}
			// Individual Blog
			else
			{
				var model = new CategorySingleViewModel(category, Server);

				return View("~/Views/Blog/CategoriesSingle.cshtml", model);
			}
		}

		public ActionResult NewPosts()
		{
			using (DataContext context = new DataContext())
			{
				var blog = context.Blogs.FirstOrDefault(x => x.IsActive == true);
				string blogUrl = "http://" + HttpContext.Request.Url.Host + "/blog/";

				var postItems = context.Blogs.Where(p => p.IsActive == true).OrderBy(p => p.Date).Take(25).ToList()
					.Select(p => new SyndicationItem(p.Title, p.HtmlContent, new Uri(blogUrl + p.Title)));


				var feed = new SyndicationFeed(blog.Title, blog.Title, new Uri(blogUrl + blog.Title), postItems)
				{
					Language = "en-US",
					Title = new TextSyndicationContent(blog.Title)
				};
				
				
				return new FeedResult(new Rss20FeedFormatter(feed));
			}
		}
    }
}
