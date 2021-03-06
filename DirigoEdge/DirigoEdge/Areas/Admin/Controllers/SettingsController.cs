﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class SettingsController : Controller
    {
		[Authorize(Roles = "Administrators")]
		public JsonResult BlogSettings(BlogSettings entity)
		{
			var result = new JsonResult();

			using (var context = new DataContext())
			{
				var blogSettings = context.BlogSettings.FirstOrDefault();
				if (blogSettings != null)
				{
					blogSettings.BlogTitle = entity.BlogTitle;
					blogSettings.DisableAllCommentsGlobal = entity.DisableAllCommentsGlobal;
					blogSettings.DisqusShortName = entity.DisqusShortName;
					blogSettings.FacebookAppId = entity.FacebookAppId;
					blogSettings.MaxBlogsOnHomepageBeforeLoad = entity.MaxBlogsOnHomepageBeforeLoad;
					blogSettings.ShowDisqusComents = entity.ShowDisqusComents;
					blogSettings.ShowFacebookComments = entity.ShowFacebookComments;
					blogSettings.ShowFacebookLikeButton = entity.ShowFacebookLikeButton;

					context.SaveChanges();
				}
			}

			return result;
		}

		[Authorize(Roles = "Administrators")]
		public JsonResult SiteSettings(SiteSettings entity)
		{
			var result = new JsonResult();

			using (var context = new DataContext())
			{
				var siteSettings = context.SiteSettings.FirstOrDefault();
				if (siteSettings != null)
				{
					siteSettings.ContactEmail = entity.ContactEmail;
					siteSettings.SearchIndex = entity.SearchIndex;
					siteSettings.GoogleAnalyticsId = entity.GoogleAnalyticsId;
					siteSettings.ContentPageRevisionsEnabled = entity.ContentPageRevisionsEnabled;
					siteSettings.ContentPageRevisionsRetensionCount = entity.ContentPageRevisionsRetensionCount;
                    siteSettings.DefaultUserRole = entity.DefaultUserRole;

					context.SaveChanges();
				}
			}

			return result;
		}

		[Authorize(Roles = "Administrators")]
		public JsonResult FeatureSettings(FeatureSettings entity)
		{
			var result = new JsonResult();

			using (var context = new DataContext())
			{
				var featureSettings = context.FeatureSettings.FirstOrDefault();
				if (featureSettings != null)
				{
					featureSettings.EventsEnabled = entity.EventsEnabled;

					context.SaveChanges();

					// Bust the site settings cache for events since we modified it's value
					Utils.SiteSettingsUtils.EventsEnabled(true);
				}
			}

			return result;
		}
    }
}