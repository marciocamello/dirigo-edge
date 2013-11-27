using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DirigoEdge.Areas.Admin.Models.DataModels;
using DirigoEdge.Utils;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class EditEventViewModel
	{
		public Event ThisEvent;
		public List<EventCategory> EventCategories;
		public List<string> UsersSelectedCategories;
		public List<EventAdminModule> EventAdminModulesColumn1;
		public List<EventAdminModule> EventAdminModulesColumn2;
		public int EventId;
		public string SiteUrl;

		
		private User _thisUser;
		private readonly MembershipUser _memUser;

		public EditEventViewModel(string eventId)
		{
			EventId = Int32.Parse(eventId);
	 		_memUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
			SiteUrl = HTTPUtils.GetFullyQualifiedApplicationPath() + "event/";
				
			using (var context = new DataContext())
			{
				ThisEvent = context.Events.FirstOrDefault(x => x.EventId == EventId);

				// Make sure we have a permalink set
				if (String.IsNullOrEmpty(ThisEvent.PermaLink))
				{
					ThisEvent.PermaLink = ContentUtils.GetFormattedUrl(ThisEvent.Title);
				}

				EventCategories = context.EventCategories.Where(x => x.IsActive == true).ToList();

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
				EventAdminModulesColumn1 = context.EventAdminModules.Where(x => x.User.Username == _thisUser.Username && x.ColumnNumber == 1).OrderBy(x => x.OrderNumber).ToList();
				EventAdminModulesColumn2 = context.EventAdminModules.Where(x => x.User.Username == _thisUser.Username && x.ColumnNumber == 2).OrderBy(x => x.OrderNumber).ToList();

				// If no settings have been saved, set some defaults for the user
				if (EventAdminModulesColumn1.Count == 0 && EventAdminModulesColumn2.Count == 0)
				{
					setDefaultModules();

					EventAdminModulesColumn1 = context.EventAdminModules.Where(x => x.User.Username == _thisUser.Username && x.ColumnNumber == 1).OrderBy(x => x.OrderNumber).ToList();
					EventAdminModulesColumn2 = context.EventAdminModules.Where(x => x.User.Username == _thisUser.Username && x.ColumnNumber == 2).OrderBy(x => x.OrderNumber).ToList();
				}
			}
		}

		private void setDefaultModules()
		{
			using (var context = new DataContext())
			{
				var user = context.Users.FirstOrDefault(x => x.Username == _memUser.UserName);
				var modules = DefaultAdminModules.GetEventAdminModules(user);

				foreach (var module in modules)
				{
					user.EventAdminModules.Add(module);
				}

				context.SaveChanges();
			}
		}
	}
}