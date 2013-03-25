using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageCategoriesViewModel
	{

		public List<BlogCategory> BlogCategories;

		public ManageCategoriesViewModel()
		{
			using (DataContext context = new DataContext())
			{
				BlogCategories = context.BlogCategories.ToList();
			}
		}
	}
}