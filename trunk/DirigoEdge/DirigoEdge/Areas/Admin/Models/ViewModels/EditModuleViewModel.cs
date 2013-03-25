using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class EditModuleViewModel
	{
		public ContentModule TheModule;

		public EditModuleViewModel(int id)
		{
			using (var context = new DataContext())
			{
				TheModule = context.ContentModules.FirstOrDefault(x => x.ContentModuleId == id);				
			}
		}
	}
}