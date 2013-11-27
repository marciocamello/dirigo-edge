using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class EventCategoryController : Controller
    {
	    [HttpPost]
	    [Authorize(Roles = "Administrators")]
	    [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddCategory(string name)
	    {
		    var result = new JsonResult();

		    if (!String.IsNullOrEmpty(name))
		    {
				using (var context = new DataContext())
				{
                    var newCategory = new EventCategory
					{
						CategoryName = name,
						CreateDate = DateTime.Now,
						IsActive = true
					};

                    context.EventCategories.Add(newCategory);
					context.SaveChanges();

                    result.Data = new { id = newCategory.EventCategoryId };

					return result;
				}
		    }

			return result;
	    }

		[HttpPost]
		[Authorize(Roles = "Administrators")]
		[AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DeleteCategory(string id)
		{
			var result = new JsonResult();

			if (!String.IsNullOrEmpty(id))
			{
				int catId = Int32.Parse(id);
				using (var context = new DataContext())
				{
                    var cat = context.EventCategories.FirstOrDefault(x => x.EventCategoryId == catId);

                    context.EventCategories.Remove(cat);
					context.SaveChanges();
				}
			}

			return result;
		}
    }
}
