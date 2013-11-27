using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogSingleHomeViewModel
	{
		public Blog TheBlog;
		public BlogUser TheBlogUser;
	    public BlogRelatedViewModel RelatedPosts;

		public bool ShowFacebookLikeButton;
		public bool ShowFacebookComments;
		public bool AllCommentsAreDisabled;
		public bool UseDisqusComments;
		public string DisqusShortName;

	    public BlogAuthorViewModel BlogAuthorModel;

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

                // Set up the Related Posts Module
			    RelatedPosts = new BlogRelatedViewModel(TheBlog.Title);

				// Get User based on authorid
				TheBlogUser = context.BlogUsers.FirstOrDefault(x => x.UserId == TheBlog.AuthorId);

			    string username = TheBlogUser != null ? TheBlogUser.DisplayName : "Anonymous";
                BlogAuthorModel = new BlogAuthorViewModel(username);

				// Facebook Like button
				ShowFacebookLikeButton = SiteSettingsUtils.ShowFbLikeButton();

				// Facebook Comments
				ShowFacebookComments = SiteSettingsUtils.ShowFbComments();

				// Absolute Url for FB Like Button
				BlogAbsoluteUrl = HttpContext.Current.Request.Url.AbsoluteUri;

				// Disqus Comments
				UseDisqusComments = SiteSettingsUtils.UseDisqusComments();	
				if (UseDisqusComments)
				{
					DisqusShortName = SiteSettingsUtils.DisqusShortName();
				}
			}
		}

	}
}