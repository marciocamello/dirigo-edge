using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Models
{
	public class BlogListModel
	{
		/// <summary>
		/// Gets a list of Blogs since after the date of the given blog id
		/// </summary>
		/// <param name="lastBlogId">The id of the blog to start from</param>
		/// <param name="count">Number of blogs to return</param>
        /// <param name="idList">List of blog ids already on page</param>
        /// <param name="user">Number of blogs to return</param>
        /// <param name="date">Date MM/yyyy </param>
		public static List<Blog> GetBlogs(int lastBlogId, int count, List<string> idList, string user = "", string date = "")
		{
			using (var context = new DataContext())
			{
				var lastBlog = context.Blogs.FirstOrDefault(x => x.BlogId == lastBlogId);
				var lastDate = lastBlog.Date;

                if (user != "")
                {
                    return context.Blogs.Where(x => x.Author == user && x.Date <= lastDate && x.BlogId != lastBlog.BlogId && x.MainCategory != "")
                        .OrderByDescending(blog => blog.Date)
                        .Take(count)
                        .ToList();
                }
                if (date != "")
                {
                    var dateConverted = Convert.ToDateTime(date);
                    return context.Blogs.Where(
                            x => x.IsActive
                                 && (idList.Contains(x.BlogId.ToString()))
                                 && (x.Date.Month == dateConverted.Month)
                                 && (x.Date.Year == dateConverted.Year)
                            )
                               .OrderBy(x => x.Date)
                               .ToList();
                }
                return context.Blogs.Where(x => x.Date <= lastDate && x.BlogId != lastBlog.BlogId && x.MainCategory != "")
                        .OrderByDescending(blog => blog.Date)
                        .Take(count)
                        .ToList();
			}
		}

        /// <summary>
        /// Gets a list of Blogs since after the date of the given blog id
        /// </summary>
        /// <param name="lastMonth">The last month shown.Start from month + 1 month</param>
        /// <param name="count">Number of blogs to return</param>
        /// <param name="idList">List of blog ids already on page, not used here yet</param>
        /// <param name="user">Number of blogs to return</param>
        /// <param name="date">Date MM/yyyy, not used here yet </param>
        public static IEnumerable<string> GetArchives(string lastMonth, int count, List<string> idList, string user = "", string date = "")
        {
            using (var context = new DataContext())
            {
                var lastDate = new DateTime(Convert.ToDateTime(lastMonth).Year, Convert.ToDateTime(lastMonth).Month, 1);
                var blogs = context.Blogs.Where(x => x.IsActive && x.Date < lastDate)
                            .OrderByDescending(blog => blog.Date)
                            .ToList();

                return (from p in blogs
                           group p by
                               new { month = p.Date.ToString("MMM"), year = p.Date.ToString("yyyy"), dateString = p.Date.ToString("MM/yyyy") }
                               into d
                               select
                                   String.Format(
                                       "<a href=\"/blog?date={3}\" class=\"archive\">" +
                                       "<span class=\"dateRef\" data-date=\"{3}\">{0} {1}</span>  ({2}) </a>",
                                       d.Key.month, d.Key.year, d.Count(), d.Key.dateString)
                ).Take(count);
            }
        }

        public static BlogSettings GetBlogSettings()
        {
            using (var context = new DataContext())
            {
                // Get the amount of blogs to load on homepage
                var blogSettings = context.BlogSettings.FirstOrDefault();
                if (blogSettings != null)
                {
                    return blogSettings;
                }
                else
                {
                    // Set the default blog settings then come back and get data
                    Utils.SiteSettingsUtils.SetDefaultBlogSettings();
                    blogSettings = context.BlogSettings.FirstOrDefault();
                    return blogSettings;
                }
            }
        }

	}
}