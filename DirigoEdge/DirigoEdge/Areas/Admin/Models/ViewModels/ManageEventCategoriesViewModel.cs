using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageEventCategoriesViewModel
	{

		public List<EventCategory> EventCategories;

		public ManageEventCategoriesViewModel()
		{
			using (DataContext context = new DataContext())
			{
				EventCategories = context.EventCategories.ToList();
			}
		}
	}
}