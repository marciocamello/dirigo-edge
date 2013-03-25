using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Models.ViewModels
{
	public class HomeViewModel
	{
		public int SlideShowId = 1;
		public SlideshowModule Slideshow;

		public HomeViewModel()
		{
			using (var context = new DataContext())
			{
				Slideshow = context.SlideshowModules.FirstOrDefault(x => x.SlideshowModuleId == SlideShowId);
				if (Slideshow != null)
				{
					// Enumerate in memory so we can close the context
					Slideshow.Slides = Slideshow.Slides.ToList();
					// Remove Spaces so partial view can use the name as a jQuery ID
					Slideshow.SlideShowName = Slideshow.SlideShowName.Replace(" ", "");

				}
			}
		}
	}
}