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
		public List<BlogCatExtraData> Categories;
		public bool ReachedMaxBlogs;

		public BlogHomeViewModel()
		{
			using (var context = new DataContext())
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

				Blogs = context.Blogs.Where(x => x.IsActive == true).Take(MaxBlogCount).ToList();
				FeaturedBlog = context.Blogs.FirstOrDefault(x => x.IsFeatured == true);
				Categories = new List<BlogCatExtraData>();

				var cats = context.BlogCategories.Where(x => x.IsActive == true).ToList();
				foreach (var cat in cats)
				{
					int count = context.Blogs.Count(x => x.MainCategory == cat.CategoryName);
					Categories.Add(new BlogCatExtraData() { TheCategory = cat, BlogCount = count });
				}

			}
		}
	}

	public class BlogCatExtraData
	{
		public BlogCategory TheCategory;
		public int BlogCount;
	}
}