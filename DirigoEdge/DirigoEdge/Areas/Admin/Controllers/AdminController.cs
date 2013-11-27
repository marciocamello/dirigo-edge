﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DirigoEdge.Areas.Admin.Models.ViewModels;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class AdminController : Controller 
    {
		[Authorize(Roles = "Administrators")]
        public ActionResult Index()
        {
	        var model = new DashBoardViewModel();

            return View(model);
        }

		[Authorize(Roles = "Administrators")]
		public ActionResult ManageMedia()
		{
			string dir = Server.MapPath("~" + Utils.ContentGlobals.IMAGEUPLOADDIRECTORY);
			var model = new ManageMediaViewModel(dir);
			
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult ManageBlogs()
		{
			var model = new ManageBlogsViewModel();
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult EditBlog(string id)
		{
			var model = new EditBlogViewModel(id);
			return View(model);
		}

        [Authorize(Roles = "Administrators")]
        public ActionResult EditEvent(string id)
        {
            var model = new EditEventViewModel(id);
            return View(model);
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult ManageEvents()
        {
            var model = new ManageEventsViewModel();
            return View(model);
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult ManageEventCategories()
        {
            var model = new ManageEventCategoriesViewModel();
            return View(model);
        }

		[Authorize(Roles = "Administrators")]
		public ActionResult ManageCategories()
		{
			var model = new ManageCategoriesViewModel();
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult ManageUsers()
		{
			var model = new ManageUsersViewModel();
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult ManageContent()
		{
			var model = new ManageContentViewModel();
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult EditSlideShow(int id)
		{
			var model = new EditSlideShowViewModel(id);
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult NewContentPage()
		{
			// Create a new Content Page to be passed to the edit content action
			using (var context = new DataContext())
			{
				ContentPage page = getDefaultContentPage();
				context.ContentPages.Add(page);
				context.SaveChanges();

				// Update the page title / permalink with the new id we now have
				page.DisplayName = "Page " + page.ContentPageId;
				context.SaveChanges();

				return RedirectToAction("EditContent", "Admin", new { id = page.ContentPageId });
			}
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult EditContent(int id)
		{
			var model = new EditContentViewModel(id);
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult EditContentBasic(int id)
		{
			var model = new EditContentViewModel(id);
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult ManageModules()
		{
			var model = new ManageModulesViewModel();
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult EditModule(int id)
		{
			var model = new EditModuleViewModel(id);
			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult NewContentModule()
		{
			int ModuleId = 0;

			// Create a new Content Page to be passed to the edit content action
			using (var context = new DataContext())
			{
				ContentModule page = getDefaultContentModule();

				context.ContentModules.Add(page);
				context.SaveChanges();

				// Update the page title / permalink with the new id we now have
				ModuleId = page.ContentModuleId;
				page.ModuleName = "Module " + ModuleId;
				context.SaveChanges();
			}

			return RedirectToAction("EditModule", "Admin", new { id = ModuleId });
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult SiteSettings()
		{
			var model = new SiteSettingsViewModel();

			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult BlogSettings()
		{
			var model = new BlogSettingsViewModel();

			return View(model);
		}

        [Authorize(Roles = "Administrators")]
		public ActionResult FeatureSettings()
        {
            var model = new FeatureSettingsViewModel();

            return View(model);
        }

		#region Admin Actions

		[Authorize(Roles = "Administrators")]
		public JsonResult ModifyUser(BlogUser user)
		{
			JsonResult result = new JsonResult();

			if (!String.IsNullOrEmpty(user.UserId.ToString()))
			{
				using (var context = new DataContext())
				{
					var editUser = context.BlogUsers.FirstOrDefault(x => x.UserId == user.UserId);

					editUser.DisplayName = user.DisplayName;
					editUser.UserImageLocation = user.UserImageLocation;
					editUser.IsActive = user.IsActive;

					context.SaveChanges();
				}
			}

			return result;
		}

		[Authorize(Roles = "Administrators")]
		public JsonResult AddUser(BlogUser user)
		{
			JsonResult result = new JsonResult();

			if (!String.IsNullOrEmpty(user.DisplayName))
			{
				using (var context = new DataContext())
				{
					var newUser = new BlogUser
					{
						CreateDate = DateTime.Now,
						DisplayName = user.DisplayName,
						Username = user.Username,
						IsActive = user.IsActive,
						UserImageLocation = user.UserImageLocation
					};
					context.BlogUsers.Add(newUser);
					context.SaveChanges();
				}
			}

			return result;
		}

		[Authorize(Roles = "Administrators")]
		public JsonResult DeleteUser(BlogUser user)
		{
			JsonResult result = new JsonResult();

			if (!String.IsNullOrEmpty(user.UserId.ToString()))
			{
				using (var context = new DataContext())
				{
					var UserToDelete = context.BlogUsers.FirstOrDefault(x => x.UserId == user.UserId);

					context.BlogUsers.Remove(UserToDelete);
					context.SaveChanges();
				}
			}

			return result;
		}

		[Authorize(Roles = "Administrators")]
		public ActionResult AddBlog()
		{
			string blogId = String.Empty;

			// Create a new blog to be passed to the edit blog action
			using (var context = new DataContext())
			{
				Blog blog = new Blog() { IsActive = false, Title = "New Blog", Date = DateTime.Now };

				context.Blogs.Add(blog);
				context.SaveChanges();

				// Update the blog title / permalink with the new id we now have
				blogId = blog.BlogId.ToString();

				blog.Title = blog.Title + " " + blogId;
				context.SaveChanges();
			}

			return RedirectToAction("EditBlog", "Admin", new { id = blogId });
		}

        [Authorize(Roles = "Administrators")]
        public ActionResult AddEvent()
        {
            string eventId = String.Empty;

            // Create a new event to be passed to the edit event action
            using (var context = new DataContext())
            {
                var thisEvent = new Event() { IsActive = false, Title = "New Event", DateCreated = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7)};

                context.Events.Add(thisEvent);
                context.SaveChanges();

                // Update the event title / permalink with the new id we now have
                eventId = thisEvent.EventId.ToString();

                thisEvent.Title = thisEvent.Title + " " + eventId;
                context.SaveChanges();
            }

            return RedirectToAction("EditEvent", "Admin", new { id = eventId });
        }

		[Authorize(Roles = "Administrators")]
		public JsonResult DeleteContent(string id)
		{
			var result = new JsonResult();

			if (String.IsNullOrEmpty(id))
			{
				return result;
			}

			int pageId = Int32.Parse(id);

			using (var context = new DataContext())
			{
				var page = context.ContentPages.FirstOrDefault(x => x.ContentPageId == pageId);

				context.ContentPages.Remove(page);
				context.SaveChanges();
			}

			return result;
		}

		[Authorize(Roles = "Administrators")]
		public JsonResult DeleteModule(string id)
		{
			var result = new JsonResult();

			if (String.IsNullOrEmpty(id))
			{
				return result;
			}

			int moduleId = Int32.Parse(id);

			using (var context = new DataContext())
			{
				var module = context.ContentModules.FirstOrDefault(x => x.ContentModuleId == moduleId);

				context.ContentModules.Remove(module);
				context.SaveChanges();
			}

			return result;
		}

		[Authorize(Roles = "Administrators")]
		public JsonResult fileUpload(HttpPostedFileBase file)
		{
			if (file != null)
			{
				var fileName = Path.GetFileName(file.FileName);
				var physicalPath = Path.Combine(Server.MapPath("~" + Utils.ContentGlobals.IMAGEUPLOADDIRECTORY), fileName);

				file.SaveAs(physicalPath);
			}

			string imgPath = Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + file.FileName;

			var sb = new StringBuilder();
			sb.Append("<li>");

			sb.Append("<span class='container'>");

			sb.Append(String.Format("<img src='{0}' />", imgPath));

			sb.Append("<span><a class='delete' href='#'>Delete</a><a class='info' href='#' data-size=''>Info</a></span>");

			sb.Append("</span>");
			sb.Append("</li>");

			return new JsonResult() { Data = new { html = sb.ToString(), path = imgPath }};
		}

        /*
         * Method to return html to view for on screen editor
         */
		[Authorize(Roles = "Administrators")]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult getModuleData(string id)
        {
            var result = new JsonResult();

			int moduleId = Int32.Parse(id);

            using (var context = new DataContext())
            {
                var editorData = context.ContentModules.FirstOrDefault(x => x.ContentModuleId == moduleId);
                result.Data = new
                    {
                        html = editorData.HTMLContent, 
                        js = editorData.JSContent, 
                        css = editorData.CSSContent,
                        title = editorData.ModuleName
                    };
            }

            return result;
        }

		#endregion


		#region Helper Methods

	    private ContentPage getDefaultContentPage()
		{
			return new ContentPage()
			{
				DisplayName = "PlaceHolder",
				IsActive = false,
				HTMLContent = "",
				CSSContent = "",
				JSContent = "",
				CreateDate = DateTime.Now
			};
		}

		public ContentModule getDefaultContentModule()
		{
			return new ContentModule()
			{
				ModuleName = "Placeholder",
				HTMLContent = "<h2>My Module</h2>",
				CSSContent = ".page { \n\n}",
				JSContent = "$(document).ready(function() { \n    // Awesome Code Here\n\n});",
				CreateDate = DateTime.Now
			};
		}

		public static string RenderPartialViewToString(string viewName, object model, ControllerContext context, ViewDataDictionary viewData, TempDataDictionary tempData)
		{
			viewData.Model = model;

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
				ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, tempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}

		#endregion

	    
    }
}