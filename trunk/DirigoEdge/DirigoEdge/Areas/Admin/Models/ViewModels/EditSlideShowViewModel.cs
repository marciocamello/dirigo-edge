
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class EditSlideShowViewModel
	{
		public SlideshowModule TheSlideShow;

		public EditSlideShowViewModel(int slideId)
		{
			using (var context = new DataContext())
			{
				TheSlideShow = context.SlideshowModules.FirstOrDefault(x => x.SlideshowModuleId == slideId);

				// Make sure all of the inner slides are enumerated through before disposing the connection
				TheSlideShow.Slides = TheSlideShow.Slides.ToList();
			}
		}
	}
}