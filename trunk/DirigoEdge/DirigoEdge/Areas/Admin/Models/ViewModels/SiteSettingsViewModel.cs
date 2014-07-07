using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class SiteSettingsViewModel
	{
		public SiteSettings Settings;
		public Dictionary<int, bool> SiteRetensionTimeValues; // Count / IsSelected
        public List<string> RolesList;	    

		public SiteSettingsViewModel()
		{
			using (var context = new DataContext())
			{
				Settings = context.SiteSettings.FirstOrDefault();

				// Set some initial values if none are found.
				if (Settings == null)
				{
					Settings = new SiteSettings()
					{
						SearchIndex = true
					};

					context.SiteSettings.Add(Settings);

					context.SaveChanges();
				}
			}

			SiteRetensionTimeValues = new Dictionary<int, bool>
				{
					{ 5, Settings.ContentPageRevisionsRetensionCount == 5 },
					{ 10, Settings.ContentPageRevisionsRetensionCount == 10 },
					{ 25, Settings.ContentPageRevisionsRetensionCount == 25 },
					{ 50, Settings.ContentPageRevisionsRetensionCount == 50 }
				};

            RolesList = Roles.GetAllRoles().ToList();
		}
	}
}