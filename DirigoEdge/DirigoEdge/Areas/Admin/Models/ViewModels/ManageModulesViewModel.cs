using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageModulesViewModel
	{
		public List<ContentModule> Modules;

		public ManageModulesViewModel()
		{
			using (DataContext context = new DataContext())
			{
				Modules = context.ContentModules.ToList();
			}
		}
	}
}