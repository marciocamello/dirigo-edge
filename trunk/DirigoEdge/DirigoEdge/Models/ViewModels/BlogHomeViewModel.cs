using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogHomeViewModel
	{
		public List<Blog> Blogs;
		public int MaxBlogCount;

		public Blog FeaturedBlog;
		public List<BlogCategory> Categories;
		public bool ReachedMaxBlogs;

		public BlogHomeViewModel()
		{
			using (DataContext context = new DataContext())
			{
				// Get the amount of blogs to load on homepage
				var blogSettings = context.BlogSettings.FirstOrDefault();
				if (blogSettings != null)
				{
					MaxBlogCount = blogSettings.MaxBlogsOnHomepageBeforeLoad;
				}
				else
				{
					// Set the default blog settings then come back and get data
					Utils.UserUtils.SetDefaultBlogSettings();
					blogSettings = context.BlogSettings.FirstOrDefault();
					MaxBlogCount = blogSettings.MaxBlogsOnHomepageBeforeLoad;
				}

				// Used to show the "Load More" link at the bottom to ajax in more content
				ReachedMaxBlogs = context.Blogs.Count() > MaxBlogCount;

				Blogs = context.Blogs.Where(x => x.IsActive == true).OrderByDescending(blog => blog.Date).Take(MaxBlogCount).ToList();
				FeaturedBlog = context.Blogs.FirstOrDefault(x => x.IsFeatured == true && x.IsActive == true);

				Categories = context.BlogCategories.Where(x => x.IsActive == true).ToList();
			}
		}
	}
}