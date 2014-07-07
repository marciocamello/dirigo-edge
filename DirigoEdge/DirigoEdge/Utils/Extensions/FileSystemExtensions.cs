using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace DirigoEdge.Utils
{
    public static class FilesystemExtensions
    {
        public static string FileExtensionForContentType(this string fileName)
        {
            var pieces = fileName.Split('.');
            var extension = pieces.Length > 1 ? pieces[pieces.Length - 1] : string.Empty;
            return (extension.ToLower() == "jpg") ? "jpeg" : extension;
        }

        public static byte[] ToByteArray(this Bitmap image)
        {
            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Bmp);
                data = memoryStream.ToArray();
            }
            return data;
        }
    }
}