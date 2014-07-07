using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ControllerHelper;
using DirigoEdge.Areas.Admin.Models;
using DirigoEdge.Areas.Admin.Models.ViewModels;
using DirigoEdge.Entities;
using DirigoEdge.Models.ViewModels;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class AdminController : Controller 
    {
        // Not authorized to view this page
        public ActionResult NotAuthorized()
        {
            return View();
        }

        // Dashboard      
        [UserIsLoggedIn]
        public ActionResult Index()
        {
	        var model = new DashBoardViewModel();

            return View(model);
        }

        [PermissionsFilter(Permissions = "Can Manage Media")]
		public ActionResult ManageMedia(string id)
        {
            var directory = id != null
                                   ? Server.MapPath("~" + Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + "\\" + id)
                                   : Server.MapPath("~" + Utils.ContentGlobals.IMAGEUPLOADDIRECTORY);

            var model = new ManageMediaViewModel(directory);
			
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Blogs")]
		public ActionResult ManageBlogs()
		{
			var model = new ManageBlogsViewModel();
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Blog Authors")]
        public ActionResult ManageBlogAuthors()
        {
            var model = new ManageBlogAuthorsViewModel();
            return View(model);
        }

        [PermissionsFilter(Permissions = "Can Edit Blogs")]
		public ActionResult EditBlog(string id)
		{
			var model = new EditBlogViewModel(id);
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Events")]
        public ActionResult EditEvent(string id)
        {
            var model = new EditEventViewModel(id);
            return View(model);
        }

        [PermissionsFilter(Permissions = "Can Edit Events")]
        public ActionResult ManageEvents()
        {
            var model = new ManageEventsViewModel();
            return View(model);
        }

        [PermissionsFilter(Permissions = "Can Edit Events")]
        public ActionResult ManageEventCategories()
        {
            var model = new ManageEventCategoriesViewModel();
            return View(model);
        }

        [PermissionsFilter(Permissions = "Can Edit Blog Categories")]
		public ActionResult ManageCategories()
		{
			var model = new ManageCategoriesViewModel();
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Users")]
		public ActionResult ManageUsers()
		{
			var model = new ManageUsersViewModel();
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Users")]
        public ActionResult ManageUserRoles()
        {
            var model = new ManageUserRolesViewModel();
            return View(model);
        }

        [PermissionsFilter(Permissions = "Can Edit Pages")]
		public ActionResult ManageContent()
		{
			var model = new ManageContentViewModel();
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Pages")]
		public ActionResult EditSlideShow(int id)
		{
			var model = new EditSlideShowViewModel(id);
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Pages")]
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

        [PermissionsFilter(Permissions = "Can Edit Pages")]
		public ActionResult EditContent(int id)
		{
			var model = new EditContentViewModel(id);
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Pages")]
		public ActionResult EditContentBasic(int id)
		{
			var model = new EditContentViewModel(id);
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Pages")]
        public ActionResult PreviewContent(int id)
        {
            var model = new ContentViewViewModel(id);

            if (model.ThePage != null)
            {
                return View(model.TheTemplate.ViewLocation, model);
            }

            HttpContext.Response.StatusCode = 404;
            return View("~/Views/Home/Error404.cshtml");
        }

        [PermissionsFilter(Permissions = "Can Edit Modules")]
		public ActionResult ManageModules()
		{
			var model = new ManageModulesViewModel();
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Modules")]
		public ActionResult EditModule(int id)
		{
			var model = new EditModuleViewModel(id);
			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Modules")]
        public ActionResult CloneModule(int id)
        {
            var newModule = new ContentModule();

            using (var context = new DataContext())
            {
                var moduleToClone = context.ContentModules.FirstOrDefault(x => x.ContentModuleId == id);

                newModule.CreateDate = DateTime.Today;
                newModule.HTMLContent = moduleToClone.HTMLContent;
                newModule.HTMLUnparsed = moduleToClone.HTMLUnparsed;
                newModule.JSContent = moduleToClone.JSContent;
                
                newModule.CSSContent = moduleToClone.CSSContent;
                newModule.SchemaEntryValues = moduleToClone.SchemaEntryValues;
                newModule.SchemaId = moduleToClone.SchemaId;
                newModule.ModuleName = "New Module";

                context.ContentModules.Add(newModule);
                context.SaveChanges();

                // Update the Display Name
                newModule.ModuleName = "New Module " + newModule.ContentModuleId;
                context.SaveChanges();
            }

            return RedirectToAction("EditModule", "Admin", new { id = newModule.ContentModuleId });
        }

        [PermissionsFilter(Permissions = "Can Edit Modules")]
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

        [PermissionsFilter(Permissions = "Can Edit Modules")]
        public ActionResult ManageSchemas()
        {
            var model = new ManageSchemasViewModel();
            return View(model);
        }

        [PermissionsFilter(Permissions = "Can Edit Modules")]
        public ActionResult NewSchema()
        {
            // Create a new Content Page to be passed to the edit content action
            using (DataContext context = new DataContext())
            {
                var schema = new Schema() { DisplayName = "New Schema " };
                context.Schemas.Add(schema);
                context.SaveChanges();

                // Update the DisplayName with the new id we now have
                schema.DisplayName = schema.DisplayName + schema.SchemaId;
                context.SaveChanges();

                return RedirectToAction("EditSchema", "Admin", new { id = schema.SchemaId });
            }
        }

        [PermissionsFilter(Permissions = "Can Edit Modules")]
        public ActionResult EditSchema(int id)
        {
            var model = new EditSchemaViewModel(id);
            return View(model);
        }

        [PermissionsFilter(Permissions = "Can Edit Settings")]
		public ActionResult SiteSettings()
		{
			var model = new SiteSettingsViewModel();

			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Settings")]
		public ActionResult BlogSettings()
		{
			var model = new BlogSettingsViewModel();

			return View(model);
		}

        [PermissionsFilter(Permissions = "Can Edit Settings")]
		public ActionResult FeatureSettings()
        {
            var model = new FeatureSettingsViewModel();

            return View(model);
        }

		#region Admin Actions

        [PermissionsFilter(Permissions = "Can Edit Blog Authors")]
        public JsonResult ModifyBlogUser(BlogUser user)
		{
            var result = new JsonResult();

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

        [PermissionsFilter(Permissions = "Can Edit Users")]
		public JsonResult ModifyUser(User user)
		{
			var result = new JsonResult();

			if (!String.IsNullOrEmpty(user.UserId.ToString()))
			{
				using (var context = new DataContext())
				{
                    var cfrp = new CodeFirstRoleProvider();
					var editUser = context.Users.FirstOrDefault(x => x.UserId == user.UserId);

                    editUser.Username = user.Username;
					editUser.UserImageLocation = user.UserImageLocation;
                    editUser.IsLockedOut = user.IsLockedOut;

                    // Modify the user roles
                    // First delete existing roles
                    foreach (var role in editUser.Roles)
				    {
                        // Only remove roles if it's not in the new set
                        if (user.Roles != null && !user.Roles.Contains(role))
                        {
                            Roles.RemoveUserFromRole(user.Username, role.RoleName);
                            cfrp.RemoveUsersFromRoles(new string[] { user.Username }, new string[] { role.RoleName });    
                        }
				    }
                    
                    // Add the asigned roles
                    foreach (var role in user.Roles)
                    {
                        // Add to Membership Framework
                        if (!Roles.IsUserInRole(user.Username, role.RoleName))
                        {
                            Roles.AddUserToRole(user.Username, role.RoleName);    
                        }

                        // Add to CodeFirst as well
                        cfrp.AddUsersToRoles(new string[] { user.Username }, new string[] { role.RoleName });
                    }

					context.SaveChanges();
				}
			}

			return result;
		}

        [PermissionsFilter(Permissions = "Can Edit Blog Authors")]
        public JsonResult AddBlogUser(BlogUser user)
		{
            var result = new JsonResult();

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

        [PermissionsFilter(Permissions = "Can Edit Blog Authors")]
		public JsonResult AddUser(User user)
		{
			var result = new JsonResult();
            var cfrp = new CodeFirstRoleProvider();

			if (!String.IsNullOrEmpty(user.Username))
			{
                // Add to .Net Membership Framework First
                WebSecurity.CreateUserAndAccount(user.Username, user.Password);
                
                // Now add additional fields to  CodeFirst User
				using (var context = new DataContext())
				{
				    var newlyAddedUser = context.Users.Where(x => x.Username == user.Username).FirstOrDefault();

				    newlyAddedUser.CreateDate = DateTime.Now;
                    newlyAddedUser.UserImageLocation = user.UserImageLocation;
                    context.SaveChanges();

                    // Add the asigned roles
				    foreach (var role in user.Roles)
				    {
                        // Add to Membership Framework
                        Roles.AddUserToRole(user.Username, role.RoleName);
                        
                        // Add to CodeFirst as well
                        cfrp.AddUsersToRoles(new string[] { user.Username }, new string[] { role.RoleName });
				    }
				}
			}

			return result;
		}

        [PermissionsFilter(Permissions = "Can Edit Users")]
        public JsonResult AddUserRole(Role role)
        {
            var result = new JsonResult();
            var cfrp = new CodeFirstRoleProvider();

            if (cfrp.RoleExists(role.RoleName))
            {
                // bail
                return result;
            }

            using (var context = new DataContext())
            {
                var NewRole = new Role
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = role.RoleName,
                    Permissions = role.Permissions
                };
                context.Roles.Add(NewRole);
                context.SaveChanges();
            }

            // Add to WebSecurity as well
            Roles.CreateRole(role.RoleName);

            return result;
        }

        [PermissionsFilter(Permissions = "Can Edit Users")]
        public JsonResult DeleteRole(Role role)
        {
            var result = new JsonResult();

            if (!String.IsNullOrEmpty(role.RoleId.ToString()))
            {
                // Delete from CodeFirst
                using (var context = new DataContext())
                {
                    var RoleToDelete = context.Roles.FirstOrDefault(x => x.RoleId == role.RoleId);

                    if (RoleToDelete != null)
                    {
                        context.Roles.Remove(RoleToDelete);
                        context.SaveChanges();

                        // Disallow deletion of administrators role
                        if (RoleToDelete.RoleName == "Administrators")
                        {
                            return result;
                        }
                    }
                }

                // Now Check WebSecurity
                if (!String.IsNullOrEmpty(role.RoleName) && role.RoleName != "Administrators")
                {
                    // Delete from WebSecurity
                    Roles.DeleteRole(role.RoleName);
                }
            }

            return result;
        }

        [PermissionsFilter(Permissions = "Can Edit Users")]
        public JsonResult ModifyRolePermissions(Role role)
        {
            var result = new JsonResult();

            if (!String.IsNullOrEmpty(role.RoleId.ToString()))
            {
                // Delete from CodeFirst
                using (var context = new DataContext())
                {
                    var RoleToModify = context.Roles.FirstOrDefault(x => x.RoleId == role.RoleId);

                    // Don't modify Admin Permissions. They get everything
                    if (RoleToModify.RoleName != "Administrators")
                    {
                        // Now change permissions
                        RoleToModify.Permissions = role.Permissions;

                        context.SaveChanges();
                    }
                }
            }

            return result;
        }

        [PermissionsFilter(Permissions = "Can Edit Users")]
        public JsonResult UpdateRoleCode(Role role)
        {
            var result = new JsonResult();

            if (!String.IsNullOrEmpty(role.RoleId.ToString()))
            {
                // Delete from CodeFirst
                using (var context = new DataContext())
                {
                    var RoleToModify = context.Roles.FirstOrDefault(x => x.RoleId == role.RoleId);

                    RoleToModify.RegistrationCode = role.RegistrationCode;

                    context.SaveChanges();
                }
            }

            return result;
        }

        [PermissionsFilter(Permissions = "Can Edit Users")]
        public ActionResult GetRoleUsers(string RoleName)
        {
            ViewBag.Role = RoleName;

            return View("~/Areas/Admin/Views/Shared/Partials/GetRoleUsers.cshtml");
        }

        [PermissionsFilter(Permissions = "Can Edit Users")]
        public ActionResult ModifyUsersInRole(List<Guid> RemoveUsers, List<Guid> AddUsers, Guid RoleID)
        {

            var result = new JsonResult();

            var cfrp = new CodeFirstRoleProvider();

            using (var context = new DataContext())
            {
                var role = context.Roles.FirstOrDefault(x => x.RoleId == RoleID);

                if (role == null) { return result; }

                // Remove Users
                if (RemoveUsers != null && RemoveUsers.Any())
                {
                    foreach (var gid in RemoveUsers)
                    {
                        var user = context.Users.Where(x => x.UserId == gid).FirstOrDefault();

                        if (user != null)
                        {
                            Roles.RemoveUserFromRole(user.Username, role.RoleName);
                            cfrp.RemoveUsersFromRoles(new string[] { user.Username }, new string[] { role.RoleName });
                        }
                    }
                }

                // Add Users
                if (AddUsers != null && AddUsers.Any())
                {
                    foreach (var gid in AddUsers)
                    {
                        var user = context.Users.Where(x => x.UserId == gid).FirstOrDefault();

                        if (user != null)
                        {
                            // Add to Membership Framework
                            if (!Roles.IsUserInRole(user.Username, role.RoleName))
                            {
                                Roles.AddUserToRole(user.Username, role.RoleName);
                            }
                            
                            cfrp.AddUsersToRoles(new string[] { user.Username }, new string[] { role.RoleName });
                        }
                    }
                }
                
            }

            return result;
        }
        

        [PermissionsFilter(Permissions = "Can Edit Users")]
		public JsonResult DeleteUser(User user)
		{
			var result = new JsonResult();

			if (!String.IsNullOrEmpty(user.UserId.ToString()))
			{
				using (var context = new DataContext())
				{
					var UserToDelete = context.Users.FirstOrDefault(x => x.UserId == user.UserId);

                    // Clean up Roles First
                    foreach (var role in Roles.GetRolesForUser(UserToDelete.Username))
				    {
                        Roles.RemoveUserFromRole(UserToDelete.Username, role);    
				    }

                    // Clean Up CodeFirst Items
				    var eventModule = context.EventAdminModules.Where(x => x.User.UserId == UserToDelete.UserId);
				    context.EventAdminModules.RemoveRange(eventModule);

				    context.SaveChanges();

                    // Finally Delete From Membership
				    WebSecurity.DeleteUser(UserToDelete.Username);
				}
			}

			return result;
		}

        [PermissionsFilter(Permissions = "Can Edit Users")]
        public JsonResult ChangeUserPassword(User user, string newPassword)
		{
			var result = new JsonResult();

			if (!String.IsNullOrEmpty(user.UserId.ToString()))
			{
				using (var context = new DataContext())
				{
					var userToUpdate = context.Users.FirstOrDefault(x => x.UserId == user.UserId);
                    var cfmp = new CodeFirstMembershipProvider();                   
				    
                    cfmp.ChangePassword(userToUpdate.Username, newPassword);

				    context.SaveChanges();
				}
			}

			return result;
		}

        [PermissionsFilter(Permissions = "Can Edit Blog Authors")]
        public JsonResult DeleteBlogUser(BlogUser user)
		{
            var result = new JsonResult();

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

        [PermissionsFilter(Permissions = "Can Edit Blogs")]
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

        [PermissionsFilter(Permissions = "Can Edit Events")]
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

        [PermissionsFilter(Permissions = "Can Edit Pages")]
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

        [PermissionsFilter(Permissions = "Can Edit Modules")]
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

        [UserIsLoggedIn]
        public JsonResult fileUpload(HttpPostedFileBase file)
        {
            var result = new JsonResult();

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var physicalPath = Path.Combine(Server.MapPath("~" + Utils.ContentGlobals.IMAGEUPLOADDIRECTORY), fileName);

                var overwrite = System.IO.File.Exists(physicalPath);

                file.SaveAs(physicalPath);
                System.IO.File.SetCreationTime(physicalPath, DateTime.Now);



                string imgPath = Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + file.FileName;
                var media = new Media { Path = imgPath, CreateDate = DateTime.Now };
                string htmlstr = overwrite ? "" : ControllerContext.RenderPartialToString("~/Areas/Admin/Views/Shared/Partials/MediaRowPartial.cshtml", media);

                result.Data = new { html = htmlstr, path = imgPath };
            }

            return result;
        }

        /*
         * Method to return html to view for on screen editor
         */
        [UserIsLoggedIn]
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
				CreateDate = DateTime.Now,
                IsRevision = false,
                PublishDate = DateTime.Now
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