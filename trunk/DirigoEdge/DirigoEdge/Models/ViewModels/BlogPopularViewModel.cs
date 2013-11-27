using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogPopularViewModel
	{
        public List<Blog> BlogRoll;
        public int BlogRollCount = 10;
        public int MaxBlogCount = 10;
        public int LastBlogId;

		public bool ReachedMaxBlogs;

	    public string BlogTitle;

		public BlogPopularViewModel()
		{
			using (var context = new DataContext())
			{
                MaxBlogCount = BlogListModel.GetBlogSettings().MaxBlogsOnHomepageBeforeLoad;
                BlogTitle = BlogListModel.GetBlogSettings().BlogTitle;

                BlogRoll = context.Blogs.Where(x => x.IsActive)
                            .OrderByDescending(blog => blog.Date)
                            .Take(MaxBlogCount)
                            .ToList();

			    if (BlogRoll.Count > 0)
			    {
			        LastBlogId = BlogRoll.LastOrDefault().BlogId;
			    }

			}
		}
	}
}