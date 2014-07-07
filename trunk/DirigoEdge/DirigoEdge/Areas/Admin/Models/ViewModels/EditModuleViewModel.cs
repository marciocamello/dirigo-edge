using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class EditModuleViewModel
	{
		public ContentModule TheModule;

        public List<Schema> Schemas;

		public EditModuleViewModel(int id)
		{
			using (var context = new DataContext())
			{
				TheModule = context.ContentModules.FirstOrDefault(x => x.ContentModuleId == id);

                // Set Unparsed Html on Legacy Modules
                if (String.IsNullOrEmpty(TheModule.HTMLUnparsed) && !String.IsNullOrEmpty(TheModule.HTMLContent))
                {
                    TheModule.HTMLUnparsed = TheModule.HTMLContent;
                }

			    Schemas = context.Schemas.ToList();
			}
		}
	}
}