using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogAuthorViewModel
	{

        public Blog TheBlog;
		public BlogUser TheBlogUser;
        public string BlogUsername;

        public bool ShowFacebookLikeButton;
        public bool ShowFacebookComments;
        public bool AllCommentsAreDisabled;

        public string BlogAbsoluteUrl;

        public BlogAuthorViewModel(string username)
		{
			using (var context = new DataContext())
			{

                // Get back to the original name before url conversion
                BlogUsername = username.Replace(ContentGlobals.BLOGDELIMMETER, " ");

                TheBlog = context.Blogs.FirstOrDefault(x => x.Author == BlogUsername);

                // Get User based on authorid
                TheBlogUser = context.BlogUsers.FirstOrDefault(x => x.Username == BlogUsername);

                // Facebook Like button
                ShowFacebookLikeButton = SiteSettingsUtils.ShowFbLikeButton();

                // Facebook Comments
                ShowFacebookComments = SiteSettingsUtils.ShowFbComments();

                // Absolute Url for FB Like Button
                BlogAbsoluteUrl = HttpContext.Current.Request.Url.AbsoluteUri;
			}
		}
	}
}