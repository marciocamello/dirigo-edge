using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.ImageResizing
{
    public class ImageFileResult : FilePathResult
    {
        public ImageFileResult(string fileName): base(fileName, string.Format("image/{0}", fileName.FileExtensionForContentType()))
        {

        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.SetDefaultImageHeaders();
            base.WriteFile(response);
        }
    }
}