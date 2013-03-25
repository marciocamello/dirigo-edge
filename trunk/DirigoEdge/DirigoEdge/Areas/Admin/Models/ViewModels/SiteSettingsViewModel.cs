using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class SiteSettingsViewModel
	{
		public SiteSettings Settings;

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
		}
	}
}