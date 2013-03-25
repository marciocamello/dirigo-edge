using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageBlogsViewModel
	{
		public List<Blog> BlogListing;

		public ManageBlogsViewModel()
		{
			using (var context = new DataContext())
			{
				BlogListing = context.Blogs.ToList();
			}
		}
	}
}