using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DirigoEdge.Entities;

namespace DirigoEdge.Utils
{
	public static class SiteSettingsUtils
	{
		private static readonly object EventsEnabledLock = new object();

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

		public static string GetGoogleAnalyticsId()
		{
			using (var context = new DataContext())
			{
				var siteSettings = context.SiteSettings.FirstOrDefault();
				return siteSettings != null ? siteSettings.GoogleAnalyticsId : "UA-XXXXX-X";
			}
		}

		public static bool PageRevisionsEnabled()
		{
			using (var context = new DataContext())
			{
				var siteSettings = context.SiteSettings.FirstOrDefault();
				return siteSettings != null && siteSettings.ContentPageRevisionsEnabled;
			}
		}

		public static bool EventsEnabled()
		{
			using (var context = new DataContext())
			{
				var featureSettings = context.FeatureSettings.FirstOrDefault();
				return featureSettings != null && featureSettings.EventsEnabled;
			}
		}

		/// <summary>
		/// Returns Whether or not the feature, "Events"  (i.e. Calendar Events) is enabled. This method is cached.
		/// </summary>
		/// <param name="recycleCache">Fetch content directly form database or check cache first</param>
		/// <returns>boolean</returns>
		public static bool EventsEnabled(bool recycleCache)
		{
			const string cacheName = "EventsEnabled"; // Must be unique

			var eventsAreEnabled = (bool?)HttpRuntime.Cache[cacheName];
			if (eventsAreEnabled == null || recycleCache)
			{
				// Lock cache so simultaneous requests don't also perform this query
				lock (EventsEnabledLock)
				{
					// If we were locked out, check the list again (double-checked locking - http://en.wikipedia.org/wiki/Double-checked_locking)
					eventsAreEnabled = (bool?)HttpRuntime.Cache[cacheName];

					if (eventsAreEnabled != null && !recycleCache)
					{
						return (bool)eventsAreEnabled;
					}

					using (var context = new DataContext())
					{
						// Otherwise get to caching the data.
						var featureSettings = context.FeatureSettings.FirstOrDefault();
						eventsAreEnabled = featureSettings != null && featureSettings.EventsEnabled;
					}

					HttpRuntime.Cache.Insert(cacheName, eventsAreEnabled, null, DateTime.Now.AddMinutes(120), Cache.NoSlidingExpiration);
				}
			}

			return (bool)eventsAreEnabled;
		}
	}
}