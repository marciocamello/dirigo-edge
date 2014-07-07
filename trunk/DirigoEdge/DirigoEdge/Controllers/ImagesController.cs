using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirigoEdge.Utils;

namespace DirigoEdge.Controllers
{
    public class ImagesController : Controller
    {
        public ActionResult RenderWithResize(string path, int width, int height, string directory)
        {
            try
            {
                return ImageUtils.GenerateThumbnail(path, directory, width, height);
            }
            catch (Exception e)
            {
                return this.instantiate404ErrorResult(path);
            }
        }

        private HttpNotFoundResult instantiate404ErrorResult(string file)
        {
            return new HttpNotFoundResult(string.Format("The file {0} does not exist.", file));
        }
    }
}
