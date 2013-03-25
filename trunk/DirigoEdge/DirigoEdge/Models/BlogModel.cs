using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models
{
	public class BlogListModel
	{
		/// <summary>
		/// Gets a list of Blogs since after the date of the given blog id
		/// </summary>
		/// <param name="lastBlogId">The id of the blog to start from</param>
		/// <param name="count">Number of blogs to return</param>
		public static List<Blog> GetBlogs(int lastBlogId, int count)
		{
			using (var context = new DataContext())
			{
				var lastBlog = context.Blogs.FirstOrDefault(x => x.BlogId == lastBlogId);
				var lastDate = lastBlog.Date;

				return context.Blogs.Where(x => x.Date <= lastDate && x.BlogId != lastBlog.BlogId).OrderByDescending(blog => blog.Date).Take(count).ToList();
			}
		}
	}
}