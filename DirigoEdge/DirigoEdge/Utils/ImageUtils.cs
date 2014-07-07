using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using DirigoEdge.Models.ImageResizing;

namespace DirigoEdge.Utils
{
    public static class ImageUtils
    {
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".BMP", ".GIF", ".PNG" };

        public static bool IsFileAnImage(string imagePath)
        {
            return ImageExtensions.Contains(Path.GetExtension(imagePath).ToUpperInvariant());
        }

        public static string GetImageThumbPath(string imagePath, bool relative = false)
        {
            string currentDirectory = Path.GetDirectoryName(imagePath);
            string filename = Path.GetFileName(imagePath);
            string newDirectory = HttpContext.Current.Server.MapPath(currentDirectory + "\\thumbs");
            string newImagePath = newDirectory + "\\" + filename;

            if (relative)
            {
                newImagePath = newImagePath.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], "\\").Replace("\\", "/");
            }


            return newImagePath;
        }

        /// <summary>
        /// Generates a thumbnail based on an existing image path
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="width">Optional width of image to be resized</param>
        /// <param name="height">Optional height of image to be resized</param>
        /// <returns></returns>
        public static void GenerateThumbnail(string path, int width = 120, int height = 120, bool preserveAspectRatio = true)
        {
            // Save image to a sub directory called thumb
            // Build the directory if it doesn't exist
            string currentDirectory = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            string newDirectory = HttpContext.Current.Server.MapPath(currentDirectory + "\\thumbs");
            string newImagePath = newDirectory + "\\" + filename;

            // Create the thumb directory
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }

            // Don't save over the image if it already exists
            if (!System.IO.File.Exists(newImagePath))
            {
                // Resize the image
                var image = new WebImage(HttpContext.Current.Server.MapPath("~" + path));
                image.Resize(width, height, preserveAspectRatio, true);

                image.Save(newImagePath, "png", false);
            }
        }

        /// <summary>
        /// Generates a thumbnail in .png format based on an existing image path
        /// </summary>
        /// <param name="currentImagePath">The path to the existing image. Ex: /my/existing/image.jpg</param>
        /// <param name="newDirectory">Directory to store relative image path. For example, "small" will generate an image to /images/small/my/existing/image.jpg </param>
        /// <param name="width">Scale Image to this width</param>
        /// <param name="height">if preserveAspectRatio is false, will crop image to this height</param>
        /// <param name="preserveAspectRatio">If set to true will preserve original image's aspect ratio</param>
        /// <returns>An ImageFileResult Object</returns>
        public static ImageFileResult GenerateThumbnail(string currentImagePath, string newDirectory, int width = 120, int height = 120, bool preserveAspectRatio = true)
        {
            string filename = Path.GetFileName(currentImagePath);
            string rootDirectory = HttpContext.Current.Server.MapPath("/images/" + newDirectory);
            string newImagePath = rootDirectory + "\\" + currentImagePath.Replace("/", "\\");
            newDirectory = newImagePath.Replace(filename, "");

            // Create the thumb directory
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }

            // Don't save over the image if it already exists
            if (!File.Exists(newImagePath))
            {
                // Resize the image
                var image = new WebImage(HttpContext.Current.Server.MapPath("~/" + currentImagePath));
                image.Resize(width, height, preserveAspectRatio, true);

                image.Save(newImagePath, "png", false);
            }

            return new ImageFileResult(newImagePath);
        }
    }
}