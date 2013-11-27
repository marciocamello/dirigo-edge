using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Utils;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageEventsViewModel
	{
		public List<Event> EventListing;

		public ManageEventsViewModel()
		{
			using (var context = new DataContext())
			{
				EventListing = context.Events.ToList();
                if (EventListing.Count == 0)
                {
                    EventListing = context.Events.ToList();
                }
			}
		}
	}
}