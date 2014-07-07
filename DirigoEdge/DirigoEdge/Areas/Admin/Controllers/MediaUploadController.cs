using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ControllerHelper;
using DirigoEdge.Areas.Admin.Models;
using DirigoEdge.Areas.Admin.Models.ViewModels;
using DirigoEdge.Areas.Admin.Models.DataModels;
using DirigoEdge.Utils;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class MediaUploadController : Controller
    {
        [HttpPost]
        [PermissionsFilter(Permissions = "Can Manage Media")]
		public JsonResult UploadFile(HttpPostedFileBase file, String category = null)
	    {
            var result = new JsonResult();
	        string warning = String.Empty;

            if (file != null)
            {
                string uploadToPath = String.IsNullOrEmpty(category)
                                       ? Utils.ContentGlobals.IMAGEUPLOADDIRECTORY
                                       : Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + category + "/";

                string physicalPath = Path.Combine(Server.MapPath("~" + uploadToPath), Path.GetFileName(file.FileName));

                string fullFilename = file.FileName;

                if (System.IO.File.Exists(physicalPath))
                {
                    string filename = Path.GetFileNameWithoutExtension(physicalPath);
                    string extension = Path.GetExtension(physicalPath);
                    int dateHash = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    fullFilename = filename + "_" + dateHash + extension;

                    physicalPath = Path.Combine(Server.MapPath("~" + uploadToPath), fullFilename);

                    warning = "File was renamed.";
                }

                try
                {
                    // Save the file to disk
                    file.SaveAs(physicalPath);

                    // If it's an image, render a thumbnail
                    if (ImageUtils.IsFileAnImage(physicalPath))
                    {
                        ImageUtils.GenerateThumbnail(uploadToPath + file.FileName);
                    }

                    // Set metadata
                    System.IO.File.SetCreationTime(physicalPath, DateTime.Now);

                    // Return html to be populated client side
                    var media = new Media { Path = uploadToPath + fullFilename, CreateDate = DateTime.Now };
                    string renderedHtml = ControllerContext.RenderPartialToString("~/Areas/Admin/Views/Shared/Partials/MediaRowPartial.cshtml", media);

                    result.Data = new {success = true, html = renderedHtml, path = media.Path, category, warning};
                }
                catch (SystemException err)
                {
                    result.Data = new { success = false, error = err};
                }
            }
            else
            {
                result.Data = new {success = false, error = "Could not find specified file"};
            }

            return result;
        }

        [HttpPost]
        [PermissionsFilter(Permissions = "Can Manage Media")]
        public JsonResult RemoveFile(String filename)
        {
            string response;
            bool success;

            // Do a little scrubbing to prevent malicious deletion
            filename = filename.Replace(Utils.ContentGlobals.IMAGEUPLOADDIRECTORY, "").Replace("..", "");

            var filepath = Server.MapPath(Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + "/" + filename);
            
            if (System.IO.File.Exists(filepath))
            {
                try
                {
                    System.IO.File.Delete(filepath);
                    response = "File successfully deleted.";
                    success = true;
                }
                catch (IOException err)
                {
                    response = "Could not delete. " + err;
                    success = false;
                }
            }
            else
            {
                response = "Could not delete. File does not exist.";
                success = false;
            }

            var result = new JsonResult { Data = new { response, success } };

            return result;
        }

        [HttpPost]
        [PermissionsFilter(Permissions = "Can Manage Media")]
        public JsonResult AddFolder(String folder)
        {
            var result = new JsonResult();

            var parentPath = Server.MapPath(Utils.ContentGlobals.IMAGEUPLOADDIRECTORY);

            if (Directory.Exists(parentPath + "/" + folder))
            {
                result.Data = new { success = false, error = "Folder already exists" };
                return result;
            }

            try
            {
                Directory.CreateDirectory(parentPath + "/" + folder);
                result.Data = new { success = true, directory = parentPath + "/" + folder };
                return result;
            }
            catch (SystemException err)
            {
                result.Data = new { success = false, error = err };
                return result;
            }
        }

        [HttpPost]
        [PermissionsFilter(Permissions = "Can Manage Media")]
        public JsonResult DeleteFolder(String folder)
        {
            var result = new JsonResult();

            string directory = Server.MapPath(Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + "/" + folder);

            if (Directory.Exists(directory))
            {
                try
                {
                    Directory.Delete(directory, true);
                    result.Data = new { success = true, directory };
                    return result;
                }
                catch (SystemException err)
                {
                    result.Data = new { success = false, error = err };
                    return result;
                }
            }

            result.Data = new { success = false, error = "Folder doesn't exist" };
            return result;
        }

        [UserIsLoggedIn]
        public JsonResult FileBrowser(string id = null)
        {
            var result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            string directory = id != null ? Server.MapPath("~" + Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + "\\" + id) : null;

            var model = new FileBrowserViewModel(directory);

            string partialHtml = ControllerContext.RenderPartialToString("~/Areas/Admin/Views/Shared/Partials/FileBrowserPartial.cshtml", model);

            result.Data = new {success = true, html = partialHtml};

            return result;
        }

        [HttpPost]
        [PermissionsFilter(Permissions = "Can Manage Media")]
		public JsonResult ViewDirectory(string dir)
		{
			var result = new JsonResult();

			string directory = Server.MapPath("~/Content/Uploaded/Media");
			var images = Directory.GetFiles(directory, "*.*")
								.Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("gif"))
								.ToList();

			var sb = new StringBuilder();
			sb.Append("<ul id='UploadedMediaListing'>");
			
			foreach (string image in images)
			{
				string imgSrc = String.Format("/Content/Uploaded/Media/{0}", Path.GetFileName(image));
				sb.Append("<li>");
				sb.Append(String.Format("<a href='javascript:void(0);' class='stockImage th has-tip tip-top' title=\"<img class='constrained' src='{0}' />\" >", imgSrc));
				sb.Append(String.Format("<img src='{0}' alt='Stock Image' />", imgSrc));
				sb.Append("</a>");
				sb.Append("</li>");
			}
			sb.Append("</ul>");

			result.Data = new { html = sb.ToString()};
			return result;
		}
    }
}
