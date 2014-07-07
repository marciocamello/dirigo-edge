using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Models.DataModels;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.ViewModels
{
	public class ContentViewViewModel
	{
		public ContentPage ThePage;
		public ContentTemplate TheTemplate;

		public PageDataCollection PageData;

		public ContentViewViewModel(string title)
		{
			using (var context = new DataContext())
			{
                // Use Permalink first
                ThePage = context.ContentPages.Where(x => x.Permalink == title && x.IsRevision == false && x.IsActive == true).Take(1).FirstOrDefault();

                // If not found, try the title
                if (ThePage == null)
                {
                    ThePage = context.ContentPages.Where(x => x.DisplayName == title && x.IsRevision == false && x.IsActive == true).Take(1).FirstOrDefault();
                }

				if (ThePage != null)
				{
                    this.init();
				}
			}
		}

        public ContentViewViewModel(int pageId)
        {
            using (var context = new DataContext())
            {
                ThePage = context.ContentPages.Where(x => x.ContentPageId == pageId).Take(1).FirstOrDefault();

                if (ThePage != null)
                {
                    this.init();
                }
            }
        }

        private void init()
        {
            TheTemplate = GetContentTemplate(ThePage.Template);

            PageData = ContentUtils.GetFormattedPageContentAndScripts(ThePage.HTMLContent);
        }

		private ContentTemplate GetContentTemplate(string templateName)
		{
			var templates = new ContentTemplates();

			if (String.IsNullOrEmpty(templateName))
			{
				return templates.Templates["blank"];
			}

			return templates.Templates.ContainsKey(ThePage.Template) ? templates.Templates[ThePage.Template] : templates.Templates["blank"];
		}
	}
}