using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageContentViewModel
	{
        // Label Customization
	    public string Heading = "Manage Content Pages";
	    public string NewButtonText = "New Page +";
	    public string EditContentHeading = "Edit Page";  // Passed to editcontent controller for display purposes
	    public int SchemaId = -1;

		public List<ContentPage> Pages;


		public ManageContentViewModel()
		{
            // Add any Schema Id's here that you don't want to be listed on this manage page
            List<int> excludeSchemas = new List<int> { 1, 3 };

			using (var context = new DataContext())
			{
                Pages = context.ContentPages.Where(x => x.IsRevision == false && !excludeSchemas.Contains((int)x.SchemaId)).ToList();
			}
		}

        public ManageContentViewModel(int schemaId)
        {
            SchemaId = schemaId;

            using (var context = new DataContext())
            {
                Pages = context.ContentPages.Where(x => x.IsRevision == false && x.SchemaId == schemaId).ToList();
            }
        }
	}
}