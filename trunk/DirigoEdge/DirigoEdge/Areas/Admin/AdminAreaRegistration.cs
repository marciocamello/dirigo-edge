using System.Web.Mvc;

namespace DirigoEdge.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Admin_default",
				"Admin/{action}/{id}",
				new { Controller = "Admin", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
