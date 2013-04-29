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

		public ManageMediaViewModel(string directory)
		{
			BlogImages = new List<string>();

			string supportedExtensions = "*.jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg,*.wmf,*.emf,*.xbm,*.ico,*.eps,*.tif,*.tiff";
			//string[] images = Directory.GetFiles(directory, "*.jpg *.png");
			var images = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower()));

			foreach (string image in images)
			{
				string fileName = Path.GetFileName(image);

				// Skip over placeholder element
				if (fileName == "ph")
				{
					continue;
				}

				string imgSrc = String.Format("/content/uploaded/blogimages/{0}", fileName);
				BlogImages.Add(imgSrc);
			}

			//foreach (string image in images)
			//{
			//	string imgSrc = String.Format("/content/uploaded/blogimages/{0}", Path.GetFileName(image));
			//	BlogImages.Add(imgSrc);
			//}


		}

	}
}