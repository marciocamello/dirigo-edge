using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DirigoEdge.Utils
{
	public static class Sitemap
	{
		// Courtesy of http://joelabrahamsson.com/xml-sitemap-with-aspnet-mvc/

		public static List<SitemapItem> GetGeneratedSiteMapItems(string host)
		{
			var theList = new List<SitemapItem>();

			using (var context = new DataContext())
			{
				// Add blogs
				var activeBlogs = context.Blogs.Where(x => x.IsActive);
				theList.AddRange(activeBlogs.Select(blog => new SitemapItem(host + "blog/" + blog.PermaLink + "/")));

				// Add content pages
				var pages = context.ContentPages;
				theList.AddRange(pages.Select(page => new SitemapItem(host + "content/" + page.DisplayName + "/")));
			}

			return theList;
		}
	}

	public enum ChangeFrequency
	{
		Always,
		Hourly,
		Daily,
		Weekly,
		Monthly,
		Yearly,
		Never
	}

	public interface ISitemapItem
	{
		string Url { get; }
		DateTime? LastModified { get; }
		ChangeFrequency? ChangeFrequency { get; }
		float? Priority { get; }
	}

	public class XmlSitemapResult : ActionResult
	{
		private readonly IEnumerable<ISitemapItem> _items;

		public XmlSitemapResult(IEnumerable<ISitemapItem> items)
		{
			_items = items;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			string encoding = context.HttpContext.Response.ContentEncoding.WebName;
			var sitemap = new XDocument(new XDeclaration("1.0", encoding, "yes"),
				 new XElement("urlset", XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9"),
					  from item in _items
					  select CreateItemElement(item)
					  )
				 );

			context.HttpContext.Response.ContentType = "application/rss+xml";
			context.HttpContext.Response.Flush();
			context.HttpContext.Response.Write(sitemap.Declaration + sitemap.ToString());
		}

		private XElement CreateItemElement(ISitemapItem item)
		{
			var itemElement = new XElement("url", new XElement("loc", item.Url.ToLower()));

			if (item.LastModified.HasValue)
				itemElement.Add(new XElement("lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

			if (item.ChangeFrequency.HasValue)
				itemElement.Add(new XElement("changefreq", item.ChangeFrequency.Value.ToString().ToLower()));

			if (item.Priority.HasValue)
				itemElement.Add(new XElement("priority", item.Priority.Value.ToString(CultureInfo.InvariantCulture)));

			return itemElement;
		}
	}

	public class SitemapItem : ISitemapItem
	{
		public SitemapItem(string url)
		{
			Url = url;
		}

		public string Url { get; set; }

		public DateTime? LastModified { get; set; }

		public ChangeFrequency? ChangeFrequency { get; set; }

		public float? Priority { get; set; }
	}	
}