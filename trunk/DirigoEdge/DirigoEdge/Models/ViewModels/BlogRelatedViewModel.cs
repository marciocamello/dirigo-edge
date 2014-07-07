using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogRelatedViewModel
	{
        public List<Blog> RelatedPosts;
        public Blog TheBlog;
        public int BlogRollCount = 10;
        public int MaxBlogCount = 10;
        public int LastBlogId = 0;

		public Blog FeaturedBlog;
		public bool ReachedMaxBlogs;

        public BlogRelatedViewModel(string title)
		{

            using (var context = new DataContext())
            {
                // Try permalink first
                TheBlog = context.Blogs.FirstOrDefault(x => x.PermaLink == title);

                MaxBlogCount = BlogListModel.GetBlogSettings().MaxBlogsOnHomepageBeforeLoad;

                // If no go then try title as a final back up
                if (TheBlog == null)
                {
                    title = title.Replace(Utils.ContentGlobals.BLOGDELIMMETER, " ");
                    TheBlog = context.Blogs.FirstOrDefault(x => x.Title == title);
                }

                if (TheBlog != null && TheBlog.Tags != null)
                {
                    List<string> tags = TheBlog.Tags.Split(',').ToList();
                    RelatedPosts = context.Blogs.Where(x => x.BlogId != TheBlog.BlogId && tags.Contains(x.Tags) && x.MainCategory == TheBlog.MainCategory)
                                    .OrderByDescending(blog => blog.Date)
                                    .Take(MaxBlogCount)
                                    .ToList();

                    if (RelatedPosts.Count > 0)
                    {
                        LastBlogId = RelatedPosts.LastOrDefault().BlogId;
                    }
                }
            }
		}
	}
}