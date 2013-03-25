using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogSingleHomeViewModel
	{
		public Blog TheBlog;
		public BlogUser TheBlogUser;
		public bool ShowFacebookLikeButton;
		public bool ShowFacebookComments;
		public bool AllCommentsAreDisabled;
		public bool UseDisqusComments;
		public string DisqusShortName;

		public string BlogAbsoluteUrl;

		public BlogSingleHomeViewModel(string title)
		{
			using (var context = new DataContext())
			{
				// Try permalink first
				TheBlog = context.Blogs.FirstOrDefault(x => x.PermaLink == title);

				// If no go then try title as a final back up
				if (TheBlog == null)
				{
					title = title.Replace(Utils.ContentGlobals.BLOGDELIMMETER, " ");
					TheBlog = context.Blogs.FirstOrDefault(x => x.Title == title);
					
					// Go ahead and set the permalink for future reference
					TheBlog.PermaLink = Utils.ContentUtils.GetFormattedUrl(TheBlog.Title);
					context.SaveChanges();
				}

				// Get User based on authorid
				TheBlogUser = context.BlogUsers.FirstOrDefault(x => x.UserId == TheBlog.AuthorId);

				// Facebook Like button
				ShowFacebookLikeButton = Utils.UserUtils.ShowFbLikeButton();

				// Facebook Comments
				ShowFacebookComments = Utils.UserUtils.ShowFbComments();

				// Absolute Url for FB Like Button
				BlogAbsoluteUrl = HttpContext.Current.Request.Url.AbsoluteUri;

				// Disqus Comments
				UseDisqusComments = Utils.UserUtils.UseDisqusComments();				
				if (UseDisqusComments)
				{
					DisqusShortName = Utils.UserUtils.DisqusShortName();
				}
			}
		}
	}
}