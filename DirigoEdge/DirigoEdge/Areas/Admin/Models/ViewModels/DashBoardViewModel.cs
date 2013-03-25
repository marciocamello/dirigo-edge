using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class DashBoardViewModel
	{
		public int BlogCount = 0;
		public int ModuleCount = 0;
		public int PagesCount = 0;
		public int UsersCount = 0;
		public int FeaturedBlogsCount = 0;

		public DashBoardViewModel()
		{
			using (DataContext context = new DataContext())
			{
				BlogCount = context.Blogs.Count();
				ModuleCount = context.ContentModules.Count();
				PagesCount = context.ContentPages.Count();
				UsersCount = context.BlogUsers.Count();
				FeaturedBlogsCount = context.Blogs.Count(x => x.IsFeatured == true);
			}
		}

	}
}