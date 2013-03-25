using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DirigoEdge.Entities;

namespace DirigoEdge.Utils
{
	public static class UserUtils
	{

		public static bool UserIsAdmin()
		{
			MembershipUser u = Membership.GetUser(HttpContext.Current.User.Identity.Name);

			return Roles.IsUserInRole("Administrators");
		}

		public static bool UseDisqusComments()
		{
			using (var context = new DataContext())
			{
				var blogSettings = context.BlogSettings.FirstOrDefault();

				return blogSettings != null && blogSettings.ShowDisqusComents;
			}
		}

		public static bool ShowFbLikeButton()
		{
			using (var context = new DataContext())
			{
				var blogSettings = context.BlogSettings.FirstOrDefault();

				return blogSettings != null && blogSettings.ShowFacebookLikeButton;
			}
		}

		public static bool ShowFbComments()
		{
			using (var context = new DataContext())
			{
				var blogSettings = context.BlogSettings.FirstOrDefault();

				return blogSettings != null && blogSettings.ShowFacebookComments;
			}
		}

		public static string DisqusShortName()
		{
			using (var context = new DataContext())
			{
				var blogSettings = context.BlogSettings.FirstOrDefault();

				return blogSettings != null ? blogSettings.DisqusShortName : String.Empty;
			}
		}

		public static void SetDefaultBlogSettings()
		{

			using (var context = new DataContext())
			{
				var settings = new BlogSettings()
				{
					BlogTitle = "My Blog",
					DisableAllCommentsGlobal = false,
					MaxBlogsOnHomepageBeforeLoad = 20
				};

				context.BlogSettings.Add(settings);

				context.SaveChanges();
			}
		}
	}
}