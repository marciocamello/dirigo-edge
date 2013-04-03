using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DirigoEdge.Areas.Admin.Controllers
{
    public class ContentAdminController : Controller
    {
		[Authorize(Roles = "Administrators")]
		public JsonResult SetWordWrap(bool wordWrap)
		{
			var userName = Membership.GetUser().UserName;

			if (!String.IsNullOrEmpty(userName))
			{
				using (var context = new DataContext())
				{
					context.Users.FirstOrDefault(x => x.Username == userName).ContentAdminWordWrap = wordWrap;

					context.SaveChanges();
				}
			}

			return new JsonResult();
		}
    }
}