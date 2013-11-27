using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class FeatureSettingsViewModel
	{
		public FeatureSettings Settings;

		public FeatureSettingsViewModel()
		{
			using (var context = new DataContext())
			{
				Settings = context.FeatureSettings.FirstOrDefault();

				// Set some initial values if none are found.
				if (Settings == null)
				{
					Settings = new FeatureSettings()
					{
						EventsEnabled = false
					};

					context.FeatureSettings.Add(Settings);

					context.SaveChanges();
				}
			}
		}
	}
}