using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirigoEdge.Entities;
using WebGrease.Css.Extensions;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class SlideAdminController : Controller
    {
		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult SaveSlideshow(SlideshowModule entity)
		{
			var result = new JsonResult();

			if (!String.IsNullOrEmpty(entity.SlideShowName))
			{
				using (var context = new DataContext())
				{
					SlideshowModule editedContent = context.SlideshowModules.FirstOrDefault(x => x.SlideshowModuleId == entity.SlideshowModuleId);
					if (editedContent != null)
					{
						editedContent.SlideShowName = entity.SlideShowName;

						editedContent.AdvanceSpeed = entity.AdvanceSpeed;
						editedContent.Animation = entity.Animation;
						editedContent.AnimationSpeed = entity.AnimationSpeed;
						editedContent.PauseOnHover = entity.PauseOnHover;
						editedContent.ShowBullets = entity.ShowBullets;

						editedContent.UseDirectionalNav = entity.UseDirectionalNav;
						editedContent.UseTimer = entity.UseTimer;

						// In order to save slides we must first delete prior data, then come back and add them
						editedContent.Slides.ToList().ForEach(r => context.Set<Slide>().Remove(r));
						context.SaveChanges();

						// Now add in the new Slides
						foreach (var slide in entity.Slides)
						{
							editedContent.Slides.Add(slide);
						}
						context.SaveChanges();
					}
				}
			}
			return result;
		}
    }
}
