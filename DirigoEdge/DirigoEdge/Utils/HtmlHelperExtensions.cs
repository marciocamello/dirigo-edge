using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DirigoEdge.Entities;
using DirigoEdge.Utils;

namespace MvcHtmlHelpers
{
	public static class HtmlHelperExtensions
	{
		private const string Nbsp = "&nbsp;";
		private const int LINKOFFSETTOP = -25;	// Default Offset for adminEditLink when positioned absolutely
		private const int LINKOFFSETRIGHT = 0;

		public static MvcHtmlString RenderUserContentModule(this HtmlHelper helper, string value)
        {
			using (var context = new DataContext())
			{
				var tempMod = context.ContentModules.FirstOrDefault(x => x.ModuleName == value);
				if (tempMod != null)
				{
					// Give logged in admins a way to quickly edit the module
					if (UserUtils.UserIsAdmin())
					{
						value = String.Format("<a class='adminEdit' href='/admin/editmodule/{0}' target='_blank' data-id='{1}' data-html='{2}'>Edit</a>{2}", tempMod.ContentModuleId, tempMod.ContentModuleId, tempMod.HTMLContent);
					}
					else
					{
						value = tempMod.HTMLContent;
					}
				}
			}

			return new MvcHtmlString(string.IsNullOrEmpty(value) ? Nbsp : value);
        }

		public static MvcHtmlString RenderUserContentModuleWithOnScreenEditor(this HtmlHelper helper, string value)
		{
			return RenderUserContentModuleWithOnScreenEditor(helper, value, LINKOFFSETTOP, LINKOFFSETRIGHT);
		}

		public static MvcHtmlString RenderUserContentModuleWithOnScreenEditor(this HtmlHelper helper, string value, int offSetTop, int offsetRight)
		{
			using (var context = new DataContext())
			{
				var tempMod = context.ContentModules.FirstOrDefault(x => x.ModuleName == value);
				if (tempMod != null)
				{
					// Give logged in admins a way to quickly edit the module
					if (UserUtils.UserIsAdmin())
					{
						value = String.Format("<a class='adminEdit onScreen' href='/admin/editmodule/{0}' target='_blank' data-id='{1}' style='top:{3}px;right:{4}px'  data-html='{2}'>Edit</a>{2}", tempMod.ContentModuleId, tempMod.ContentModuleId, tempMod.HTMLContent, offSetTop, offsetRight);
					}
					else
					{
						value = tempMod.HTMLContent;
					}
				}
			}

			return new MvcHtmlString(string.IsNullOrEmpty(value) ? Nbsp : value);
		}
	}
}