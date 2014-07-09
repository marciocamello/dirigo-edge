using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using DirigoEdge.Models;
using DirigoEdge.Models.ImageResizing;
using ImageResizer;

namespace DirigoEdge.Utils
{
    public static class ImageUtils
    {
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".BMP", ".GIF", ".PNG" };

        public static bool IsFileAnImage(string imagePath)
        {
            return ImageExtensions.Contains(Path.GetExtension(imagePath).ToUpperInvariant());
        }

        /// <summary>
        /// Returns fully qualified path of a thumbnail, given a relative image path
        /// </summary>
        /// <param name="imagePath">The relative Image Path</param>
        /// <param name="relative">True if image is relative to a given directory. Ex: /images/myimage.jpg</param>
        /// <returns></returns>
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
        /// Generates a thumbnail in /thumbs/ in the directory where the current image exists. Ex : /image/test.jpg will generate /image/thumbs/test.jpg
        /// </summary>
        /// <param name="path">Realtive path to the image</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="preserveAspectRatio"></param>
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
            if (!File.Exists(newImagePath))
            {
                string resizeQuery = String.Empty;

                // Resize parameters based on user input
                if (height > 0 || preserveAspectRatio)
                {
                    // fit to specified width and height
                    resizeQuery = String.Format("maxwidth={0}&maxheight={1}", width, height);
                }
                else
                {
                    // Leave off height to preserve aspect ratio
                    resizeQuery = String.Format("maxwidth={0}", width);
                }

                ImageBuilder.Current.Build(HttpContext.Current.Server.MapPath("~/" + path), newImagePath, new ResizeSettings(resizeQuery));
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
        public static ImageFileResult GenerateThumbnail(string currentImagePath, string newDirectory, int width = 120, int? height = null, bool preserveAspectRatio = true)
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
                // If no height specified set height to absurdly large value so it resizes based on width

                int imageHeight = height.GetValueOrDefault();
                string resizeQuery = String.Empty;

                // Resize parameters based on user input
                if (imageHeight > 0 || preserveAspectRatio)
                {
                    // fit to specified width and height
                    resizeQuery = String.Format("maxwidth={0}&maxheight={1}", width, imageHeight);
                }
                else
                {
                    // Leave off height to preserve aspect ratio
                    resizeQuery = String.Format("maxwidth={0}", width);
                }

                ImageBuilder.Current.Build(HttpContext.Current.Server.MapPath("~/" + currentImagePath), newImagePath, new ResizeSettings(resizeQuery));
            }

            return new ImageFileResult(newImagePath);
        }
    }

    public class BackgroundImage
    {
        public string Filepath { get; set; }
        public int? Width { get; set; }
    }
}