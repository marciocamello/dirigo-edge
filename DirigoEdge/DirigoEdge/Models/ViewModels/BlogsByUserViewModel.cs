using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogsByUserViewModel
	{
        public Blog TheBlog;
        public List<Blog> BlogsByUser;
        public BlogUser TheBlogUser;
        public int BlogRollCount = 10;
        public int MaxBlogCount = 10;
        public int LastBlogId;
        public bool ReachedMaxBlogs;
		public string BlogUsername;

	    public string BlogTitle;

		public BlogsByUserViewModel(string username)
		{
			// Get back to the original name before url conversion
			BlogUsername = username.Replace(ContentGlobals.BLOGDELIMMETER, " ");

			using (var context = new DataContext())
			{

                // Get User based on authorid
                TheBlogUser = context.BlogUsers.FirstOrDefault(x => x.Username == BlogUsername);

                MaxBlogCount = BlogListModel.GetBlogSettings().MaxBlogsOnHomepageBeforeLoad;
                BlogTitle = BlogListModel.GetBlogSettings().BlogTitle;

                BlogsByUser = context.Blogs.Where(x => x.Author == BlogUsername && x.IsActive)
                            .OrderByDescending(blog => blog.Date)
                            .Take(MaxBlogCount)
                            .ToList();

                // Try permalink first
                TheBlog = BlogsByUser.FirstOrDefault(x => x.Author == BlogUsername);

			    if (BlogsByUser.Count > 0)
			    {
			        LastBlogId = BlogsByUser.LastOrDefault().BlogId;
			    }
			}
		}
	}
}