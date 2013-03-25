using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class BlogSettingsViewModel
	{
		public BlogSettings Settings;

		public BlogSettingsViewModel()
		{
			using (var context = new DataContext())
			{
				Settings = context.BlogSettings.FirstOrDefault();

				// Set some initial values if none are found.
				if (Settings == null)
				{
					Utils.UserUtils.SetDefaultBlogSettings();
					Settings = context.BlogSettings.FirstOrDefault();
				}
			}
		}
	}
}