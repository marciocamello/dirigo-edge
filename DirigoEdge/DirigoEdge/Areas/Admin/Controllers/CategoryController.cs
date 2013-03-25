using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class CategoryController : Controller
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
					var newCategory = new BlogCategory
					{
						CategoryName = name,
						CreateDate = DateTime.Now,
						IsActive = true
					};

					context.BlogCategories.Add(newCategory);
					context.SaveChanges();

					result.Data = new {id = newCategory.CategoryId};

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
					var cat = context.BlogCategories.FirstOrDefault(x => x.CategoryId == catId);

					context.BlogCategories.Remove(cat);
					context.SaveChanges();
				}
			}

			return result;
		}
    }
}
