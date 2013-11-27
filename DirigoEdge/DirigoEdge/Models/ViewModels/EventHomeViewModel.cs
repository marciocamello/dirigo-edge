using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.ViewModels
{
	public class EventHomeViewModel
	{
		public List<Event> Events;
		public int MaxEventCount;

		public Event FeaturedEvent;
		public List<EventCatExtraData> Categories;
		public bool ReachedMaxEvents;

		public EventHomeViewModel()
		{
			using (var context = new DataContext())
			{
			    var tomorrow = DateTime.Now.Date;
				Events = context.Events.Where(x => x.IsActive == true && DateTime.Compare(x.EndDate.Value, tomorrow) >= 0).ToList();
				FeaturedEvent = context.Events.FirstOrDefault(x => x.IsFeatured == true);
				Categories = new List<EventCatExtraData>();

				var cats = context.EventCategories.Where(x => x.IsActive == true).ToList();
				foreach (var cat in cats)
				{
					int count = context.Events.Count(x => x.MainCategory == cat.CategoryName);
					Categories.Add(new EventCatExtraData() { TheCategory = cat, EventCount = count });
				}

			}
		}
	}

	public class EventCatExtraData
	{
		public EventCategory TheCategory;
		public int EventCount;
	}
}