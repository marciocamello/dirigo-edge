using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using DirigoEdge.Entities;
using DirigoEdge.Models.DataModels;
using DirigoEdge.Utils;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class EditContentViewModel
	{
	    public string Heading = "Edit Page";

		public ContentPage ThePage;
		public Dictionary<string, ContentTemplate> Templates;
		public bool UseWordWrap;
        public List<ContentPage> Revisions;
        public string SiteUrl;
	    public int BasePageId;

	    public bool IsNewerVersion;
	    public int NewerVersionId;

        public List<Schema> Schemas;
	    public bool ShowSchemaSelector = false;
        public bool ShowFieldEditor = false;

		public EditContentViewModel(int id)
		{
			using (var context = new DataContext())
			{
				ThePage = context.ContentPages.Where(x => x.ContentPageId == id).Take(1).FirstOrDefault();

                // If we are editing a draft, we actually need to be editing the parent page, but keep the drafts contents (html, css, meta, etc).
                // To accomplish this, we can simply change the id of the page we're editing in memory, to the parent page.
                BasePageId = ThePage.IsRevision ? Convert.ToInt32(ThePage.ParentContentPageId) : ThePage.ContentPageId;

				var userName = Membership.GetUser().UserName;
				UseWordWrap = context.Users.FirstOrDefault(x => x.Username == userName).ContentAdminWordWrap;

                SiteUrl = HTTPUtils.GetFullyQualifiedApplicationPath();

				// Take care of any legacy pages that don't have a publish date associated
				if (ThePage.PublishDate == null)
				{
					ThePage.PublishDate = DateTime.Now;
					context.SaveChanges();
				}

                // Take care of any legacy pages where Unparsed html was not saved
			    if (String.IsNullOrEmpty(ThePage.HTMLUnparsed) && !String.IsNullOrEmpty(ThePage.HTMLContent))
			    {
			        ThePage.HTMLUnparsed = ThePage.HTMLContent;
			    }				

                // Set a permalink if one hasn't been created / legacy support for DisplayName
                if (String.IsNullOrEmpty(ThePage.Permalink))
                {
                    ThePage.Permalink = ContentUtils.GetFormattedUrl(ThePage.DisplayName);
                    context.SaveChanges();
                }

                // Set Page Title if one hasn't been created / legacy support for DisplayName
                if (String.IsNullOrEmpty(ThePage.Title))
                {
                    ThePage.Title = ThePage.DisplayName;
                    context.SaveChanges();
                }

                // Check to see if there is a newer version available
                var newerVersion = context.ContentPages.Where(x => (x.ParentContentPageId == BasePageId || x.ContentPageId == BasePageId) && x.PublishDate > ThePage.PublishDate && x.ContentPageId != ThePage.ContentPageId).OrderByDescending(x => x.PublishDate).FirstOrDefault();

                if (newerVersion != null)
                {
                    IsNewerVersion = true;
                    NewerVersionId = newerVersion.ContentPageId;
                }

                Templates = new ContentTemplates().Templates;

                Revisions = context.ContentPages.Where(x => x.ParentContentPageId == BasePageId || x.ContentPageId == BasePageId).OrderByDescending(x => x.PublishDate).ToList();

                // Get list of schemas for drop down
                Schemas = context.Schemas.ToList();
			}
		}
	}
}