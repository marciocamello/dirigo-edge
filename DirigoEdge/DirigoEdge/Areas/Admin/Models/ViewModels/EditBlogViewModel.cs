using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Utils;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class EditBlogViewModel
	{
		public Blog ThisBlog;
		public List<BlogUser> BlogUsers;
		public List<BlogCategory> Categories;
		public List<string> UsersSelectedCategories;
		public int BlogId;

		public EditBlogViewModel(string blogId)
		{
			BlogId = Int32.Parse(blogId);

			using (DataContext context = new DataContext())
			{
				ThisBlog = context.Blogs.Where(x => x.BlogId == BlogId).FirstOrDefault();

				// Make sure we have a permalink set
				if (String.IsNullOrEmpty(ThisBlog.PermaLink))
				{
					ThisBlog.PermaLink = ContentUtils.GetFormattedUrl(ThisBlog.Title);
				}

				// Get the list of Authors for the drop down select
				BlogUsers = context.BlogUsers.Where(x => x.IsActive == true).OrderBy(x => x.DisplayName).ToList();

				Categories = context.BlogCategories.Where(x => x.IsActive == true).ToList();

				UsersSelectedCategories = new List<string>();
			}
		}
	}
}