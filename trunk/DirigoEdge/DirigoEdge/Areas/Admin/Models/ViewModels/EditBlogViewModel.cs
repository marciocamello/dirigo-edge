using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DirigoEdge.Areas.Admin.Models.DataModels;
using DirigoEdge.Utils;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class EditBlogViewModel
	{
		public Blog ThisBlog;
		public List<BlogUser> BlogUsers;
		public List<BlogCategory> Categories;
		public List<string> UsersSelectedCategories;
		public List<BlogAdminModule> AdminModulesColumn1;
		public List<BlogAdminModule> AdminModulesColumn2;
		public int BlogId;
		public string SiteUrl;
		
		private User _thisUser;
		private readonly MembershipUser _memUser;

		public EditBlogViewModel(string blogId)
		{
			BlogId = Int32.Parse(blogId);
	 		_memUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
			SiteUrl = HTTPUtils.GetFullyQualifiedApplicationPath() + "blog/";
				
			using (var context = new DataContext())
			{
				ThisBlog = context.Blogs.FirstOrDefault(x => x.BlogId == BlogId);

				// Make sure we have a permalink set
				if (String.IsNullOrEmpty(ThisBlog.PermaLink))
				{
					ThisBlog.PermaLink = ContentUtils.GetFormattedUrl(ThisBlog.Title);
				    context.SaveChanges();
				}

				// Get the list of Authors for the drop down select
				BlogUsers = context.BlogUsers.Where(x => x.IsActive == true).OrderBy(x => x.DisplayName).ToList();

				Categories = context.BlogCategories.Where(x => x.IsActive == true).ToList();

				UsersSelectedCategories = new List<string>();

				_thisUser = context.Users.FirstOrDefault(x => x.Username == _memUser.UserName);
			}

			// Get the admin modules that will be displayed to the user in each column
			getAdminModules();
		}

		private void getAdminModules()
		{
			using (var context = new DataContext())
			{
				AdminModulesColumn1 = context.BlogAdminModules.Where(x => x.User.Username == _thisUser.Username && x.ColumnNumber == 1).OrderBy(x => x.OrderNumber).ToList();
				AdminModulesColumn2 = context.BlogAdminModules.Where(x => x.User.Username == _thisUser.Username && x.ColumnNumber == 2).OrderBy(x => x.OrderNumber).ToList();

				// If no settings have been saved, set some defaults for the user
				if (AdminModulesColumn1.Count == 0 && AdminModulesColumn2.Count == 0)
				{
					setDefaultModules();

					AdminModulesColumn1 = context.BlogAdminModules.Where(x => x.User.Username == _thisUser.Username && x.ColumnNumber == 1).OrderBy(x => x.OrderNumber).ToList();
					AdminModulesColumn2 = context.BlogAdminModules.Where(x => x.User.Username == _thisUser.Username && x.ColumnNumber == 2).OrderBy(x => x.OrderNumber).ToList();
				}
			}
		}

		private void setDefaultModules()
		{
			using (var context = new DataContext())
			{
				var user = context.Users.FirstOrDefault(x => x.Username == _memUser.UserName);
				var modules = DefaultAdminModules.GetDefaultAdminModules(user);

				foreach (var module in modules)
				{
					user.BlogAdminModules.Add(module);
				}

				context.SaveChanges();
			}
		}
	}
}