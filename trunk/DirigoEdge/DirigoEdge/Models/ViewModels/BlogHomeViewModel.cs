using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogHomeViewModel
	{
        public List<Blog> BlogRoll;
	    public BlogsCategoriesViewModel BlogCats;

        public int BlogRollCount = 10;
        public int MaxBlogCount = 10;
        public int LastBlogId = 0;

		public Blog FeaturedBlog;
		public bool ReachedMaxBlogs;

        public string CurrentMonth;

	    public string BlogTitle;

		public BlogHomeViewModel(string date = "")
		{
			using (var context = new DataContext())
			{
			    MaxBlogCount = BlogListModel.GetBlogSettings().MaxBlogsOnHomepageBeforeLoad;
                BlogTitle = BlogListModel.GetBlogSettings().BlogTitle;

				FeaturedBlog = context.Blogs.FirstOrDefault(x => x.IsFeatured);

                CurrentMonth = "";

                BlogRoll = context.Blogs.Where(x => x.IsActive)
                            .OrderByDescending(blog => blog.Date)
                            .Take(MaxBlogCount)
                            .ToList();

			    BlogCats = new BlogsCategoriesViewModel("");


                if (!String.IsNullOrEmpty(date))
                {
                    DateTime startDate = Convert.ToDateTime(date);

                    CurrentMonth = startDate.ToString("MM/yyyy");

                    BlogRoll =
                        context.Blogs.Where(
                            x => x.IsActive
                                 && (x.Date.Month == startDate.Month)
                                 && (x.Date.Year == startDate.Year)
                            )
                               .OrderBy(x => x.Date)
                               .ToList();
                }

			    LastBlogId = 0;

			    if (BlogRoll.Count < 1)
			    {
			        LastBlogId = BlogRoll.LastOrDefault().BlogId;
			    }

			}
		}
	}
}