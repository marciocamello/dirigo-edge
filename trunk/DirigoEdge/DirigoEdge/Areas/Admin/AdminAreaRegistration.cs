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
            //// Vacation Packages Sample Route
            //context.MapRoute(
            //    name: "VacationPackages",
            //    url: "admin/packages/",
            //    defaults: new { controller = "Admin", action = "ManageEntity", schemaId = 1, heading = "Manage Vacation Packages", buttonText = "New Vacation Package +", editHeading = "Edit Vacation Package" }
            //);

			context.MapRoute(
				"Admin_default",
				"Admin/{action}/{id}",
				new { Controller = "Admin", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
