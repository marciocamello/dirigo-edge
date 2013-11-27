using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DirigoEdge.Entities;
using DirigoEdge.Utils;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class BlogAdminController : Controller
    {
		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult ModifyBlog(Blog entity)
		{
			JsonResult result = new JsonResult();

			if (!String.IsNullOrEmpty(entity.Title))
			{
				using (var context = new DataContext())
				{
					Blog editedBlog = context.Blogs.FirstOrDefault(x => x.BlogId == entity.BlogId);
					if (editedBlog != null)
					{
						editedBlog.Author = scrubInput(entity.Author);
						editedBlog.AuthorId = entity.AuthorId;
						editedBlog.HtmlContent = entity.HtmlContent;
						editedBlog.ImageUrl = scrubInput(entity.ImageUrl);
						editedBlog.IsActive = entity.IsActive;
						editedBlog.IsFeatured = entity.IsFeatured;
						editedBlog.Title = scrubInput(entity.Title);
						editedBlog.PermaLink = scrubInput(entity.PermaLink);
						//jptodo
						//editedBlog.Categories = scrubInput(entity.Categories);
						editedBlog.MainCategory = scrubInput(entity.MainCategory);
						editedBlog.Tags = scrubInput(entity.Tags);
						editedBlog.ShortDesc = entity.ShortDesc;
						editedBlog.Date = entity.Date;

						// Meta
						editedBlog.Canonical = entity.Canonical;
						editedBlog.OGImage = entity.OGImage;
						editedBlog.OGTitle = entity.OGTitle;
						editedBlog.OGType = entity.OGType;
						editedBlog.OGUrl = entity.OGUrl;
						editedBlog.MetaDescription = entity.MetaDescription;

						context.SaveChanges();

						result.Data = new { id = entity.BlogId };
					}
				}
			}

			return result;
		}

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult AddBlog(Blog entity)
		{
			JsonResult result = new JsonResult();

			if (!String.IsNullOrEmpty(entity.Title))
			{
				using (DataContext context = new DataContext())
				{
					context.Blogs.Add(entity);
					context.SaveChanges();
				}

				result.Data = new { id = entity.BlogId };
			}

			return result;
		}

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult DeleteBlog(string id)
		{
			JsonResult result = new JsonResult();

			if (String.IsNullOrEmpty(id))
			{
				return result;
			}

			int blogId = Int32.Parse(id);

			using (var context = new DataContext())
			{
				var blog = context.Blogs.FirstOrDefault(x => x.BlogId == blogId);

				context.Blogs.Remove(blog);
				context.SaveChanges();
			}

			return result;
		}

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult ModifyContent(ContentPage entity, bool isBasic)
		{
			var result = new JsonResult();

			if (!String.IsNullOrEmpty(entity.DisplayName))
			{
				using (var context = new DataContext())
				{
					ContentPage editedContent = context.ContentPages.FirstOrDefault(x => x.ContentPageId == entity.ContentPageId);
					if (editedContent != null)
					{
						editedContent.DisplayName = scrubInput(entity.DisplayName);
						editedContent.HTMLContent = entity.HTMLContent;

						if (!isBasic)
						{
							editedContent.JSContent = entity.JSContent;
							editedContent.CSSContent = entity.CSSContent;
						}

						editedContent.Template = entity.Template;
						editedContent.Title = entity.Title;
						editedContent.PublishDate = entity.PublishDate;

						// SEO Related Info
						editedContent.MetaDescription = entity.MetaDescription;
						editedContent.OGTitle = entity.OGTitle;
						editedContent.OGImage = entity.OGImage;
						editedContent.OGType = entity.OGType;
						editedContent.OGUrl = entity.OGUrl;
						editedContent.Canonical = entity.Canonical;

						context.SaveChanges();

						// Save a Page Revision if enabled
						if (SiteSettingsUtils.PageRevisionsEnabled())
						{
							var revision = new ContentPageRevision()
								{
									AuthorName = Utils.UserUtils.CurrentMembershipUsername(),
									ContentHtml = entity.HTMLContent,
									ContentPageId = entity.ContentPageId,
									DateCreated = DateTime.Now
								};

							context.ContentPageRevisions.Add(revision);
							
							context.SaveChanges();
						}						
					}
				}
			}
			return result;
		}

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		public JsonResult UpdateModuleShort(int id, string html)
		{
			var result = new JsonResult();
			
			using (var context = new DataContext())
			{
				var module = context.ContentModules.FirstOrDefault(x => x.ContentModuleId == id);
				if (module != null)
				{
					module.HTMLContent = html;
				}

				context.SaveChanges();
			}
			
			return result;
		}

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		public JsonResult SaveModules(AdminModules entity)
		{
			var result = new JsonResult();

			if (entity != null)
			{
				var user = Membership.GetUser(HttpContext.User.Identity.Name);
				string userName = user.UserName;
				
				using (var context = new DataContext())
				{
					// First delete all entries for user
					var modules = context.BlogAdminModules.Where(x => x.User.Username == userName);
					foreach (var mod in modules)
					{
						context.BlogAdminModules.Remove(mod);
					}

					// Then add the new modules to the user
					if (entity.AdminModulesColumn1 != null)
					{
						foreach (var module in entity.AdminModulesColumn1)
						{
							var thisUser = context.Users.FirstOrDefault(x => x.Username == userName);

							// Make sure modules exist
							checkNullUserModules(thisUser);

							thisUser.BlogAdminModules.Add(module);
						}
					}

					if (entity.AdminModulesColumn2 != null)
					{
						foreach (var module in entity.AdminModulesColumn2)
						{
							var thisUser = context.Users.FirstOrDefault(x => x.Username == userName);

							// Make sure modules exist
							checkNullUserModules(thisUser);

							thisUser.BlogAdminModules.Add(module);
						}
					}

					context.SaveChanges();
				}
			}
			
			return result;
		}

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult ModifyModule(ContentModule entity)
		{
			var result = new JsonResult();

			if (!String.IsNullOrEmpty(entity.ModuleName))
			{
				using (var context = new DataContext())
				{
					ContentModule editedContent = context.ContentModules.FirstOrDefault(x => x.ContentModuleId == entity.ContentModuleId);
					if (editedContent != null)
					{
						editedContent.ModuleName = scrubInput(entity.ModuleName);
						editedContent.HTMLContent = entity.HTMLContent;
						editedContent.JSContent = entity.JSContent;
						editedContent.CSSContent = entity.CSSContent;

						context.SaveChanges();
					}
				}
			}
			return result;
		}

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		public ActionResult UploadBlogImage(IEnumerable<HttpPostedFileBase> files)
		{
			// The Name of the Upload component is "files"
			if (files != null)
			{
				foreach (var file in files)
				{
					// Some browsers send file names with full path.
					// We are only interested in the file name.
					var fileName = Path.GetFileName(file.FileName);
					var physicalPath = Path.Combine(Server.MapPath("~/Content/Uploaded/BlogFeaturedImages/"), fileName);

					// The files are not actually saved in this demo
					file.SaveAs(physicalPath);
				}
			}

			// Return an empty string to signify success
			return Content("");
		}
		
		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UploadBlogImageInline(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
		{
			//===================================================================================
			// Takes an image uploaded from the blog editor and saves it to the content directory
			//===================================================================================
			string output = "<h1>Failure</h1>"; // Default to fail
			if (upload != null)
			{
				var fileName = Path.GetFileName(upload.FileName);
				var physicalPath = Path.Combine(Server.MapPath("~" + Utils.ContentGlobals.BLOGIMAGEUPLOADDIRECTORY), fileName);

				upload.SaveAs(physicalPath);

				// Alert the user image was successfully uploaded
				string finalUrl = Utils.ContentGlobals.BLOGIMAGEUPLOADDIRECTORY + fileName;
				string message = "Image successfully uploaded";

				output = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + finalUrl + "\", \"" + message + "\");</script></body></html>";
			}

			return Content(output);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult LoadStockImages()
		{
			StringBuilder sb = new StringBuilder();

			string directory = Server.MapPath("~" + Utils.ContentGlobals.STOCKBLOGIMAGESDIRECTORY);
			string[] images = Directory.GetFiles(directory, "*.jpg");
			foreach (string image in images)
			{
				string imgSrc = Utils.ContentGlobals.STOCKBLOGIMAGESDIRECTORY + Path.GetFileName(image);
				sb.Append(String.Format("<a href='javascript:void(0);' class='stockImage has-tip tip-top' title=\"<img src='{0}' />\" >", imgSrc));
				sb.Append(String.Format("<img src='{0}' alt='Stock Image' />", imgSrc));
				sb.Append("</a>");
			}

			return Content(sb.ToString());
		}

		#region Helper Methods

		private void checkNullUserModules(User thisUser)
		{
			if (thisUser.BlogAdminModules == null)
			{
				thisUser.BlogAdminModules = new List<BlogAdminModule>();
			}
		}

		private string scrubInput(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return String.Empty;
			}
			else
			{
				return input.Trim();
			}
		}

		#endregion
    }

	public class BlogEntity
	{
		public string Title { get; set; }
		public string HtmlContent { get; set; }
	}

	public class AdminModules
	{
		public List<BlogAdminModule> AdminModulesColumn1 { get; set; }
		public List<BlogAdminModule> AdminModulesColumn2 { get; set; }
	}
}