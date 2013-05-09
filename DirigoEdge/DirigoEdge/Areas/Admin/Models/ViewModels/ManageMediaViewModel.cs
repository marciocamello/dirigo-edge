using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageMediaViewModel
	{
		public List<string> BlogImages;
		public const string supportedExtensions = "*.jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg,*.wmf,*.emf,*.xbm,*.ico,*.eps,*.tif,*.tiff";

		public ManageMediaViewModel(string directory)
		{
			BlogImages = new List<string>();

			
			
			var images = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower()));

			foreach (string image in images)
			{
				string fileName = Path.GetFileName(image);

				// Skip over placeholder element
				if (fileName == "ph")
				{
					continue;
				}

				string imgSrc = Utils.ContentGlobals.IMAGEUPLOADDIRECTORY + fileName;
				BlogImages.Add(imgSrc);
			}
		}
	}
}