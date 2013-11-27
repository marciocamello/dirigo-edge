using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DirigoEdge.Models.ViewModels
{
	public class EventSingleHomeViewModel
	{
		public Event TheEvent;

		public string EventAbsoluteUrl;

		public EventSingleHomeViewModel(string title)
		{
			using (var context = new DataContext())
			{
				// Try permalink first
				TheEvent = context.Events.FirstOrDefault(x => x.PermaLink == title);

				// If no go then try title as a final back up
				if (TheEvent == null)
				{
					title = title.Replace(Utils.ContentGlobals.BLOGDELIMMETER, " ");
					TheEvent = context.Events.FirstOrDefault(x => x.Title == title);
					
					// Go ahead and set the permalink for future reference
					TheEvent.PermaLink = Utils.ContentUtils.GetFormattedUrl(TheEvent.Title);
					context.SaveChanges();
				}

				// Absolute Url for FB Like Button
				EventAbsoluteUrl = HttpContext.Current.Request.Url.AbsoluteUri;
			}
		}
	}
}