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
		private const int LINKOFFSETTOP = -20;	// Default Offset for adminEditLink when positioned absolutely
		private const int LINKOFFSETRIGHT = 15;



		/// <summary>
		/// Give the user the ability edit content on screen with a Code Editor. WYSIWYG is not enabled.
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="value">The name of the content module to render</param>
		/// <returns></returns>
		public static MvcHtmlString RenderUserContentModuleWithCodeEditor(this HtmlHelper helper, string value)
        {
			using (var context = new DataContext())
			{
				var tempMod = context.ContentModules.FirstOrDefault(x => x.ModuleName == value);
				if (tempMod != null)
				{
					// Give logged in admins a way to quickly edit the module
					if (UserUtils.UserIsAdmin())
					{
						value = String.Format(
							"<div class='adminButtons singleButton'>" +
								"<a class='adminRawEdit onScreen' href='/admin/editmodule/{0}' target='_blank' data-id='{0}'>Edit {{ }}</a>" +
							"</div>{1}",
							tempMod.ContentModuleId, tempMod.HTMLContent);

						//value = String.Format("<a class='adminEdit' href='/admin/editmodule/{0}' target='_blank' data-id='{1}' data-html='{2}'>Edit</a>{2}", tempMod.ContentModuleId, tempMod.ContentModuleId, tempMod.HTMLContent);
					}
					else
					{
						value = tempMod.HTMLContent;
					}
				}
			}

			return new MvcHtmlString(string.IsNullOrEmpty(value) ? Nbsp : value);
        }

		/// <summary>
		/// Give the user the ability edit content on screen with either a WYSIWYG or a Code Editor.
		/// WYSIWYG may mess with an custom class or html elements, so only use this on basic content. For stricter content, use the Code Editor
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="value">The name of the content module to render</param>
		/// <returns></returns>
		public static MvcHtmlString RenderUserContentModuleWithOnScreenEditors(this HtmlHelper helper, string value)
		{
			return RenderUserContentModuleWithOnScreenEditors(helper, value, LINKOFFSETTOP, LINKOFFSETRIGHT);
		}

		/// <summary>
		/// Give the user the ability edit content on screen with either a WYSIWYG or a Code Editor.
		/// WYSIWYG may mess with an custom class or html elements, so only use this on basic content. For stricter content, use the Code Editor
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="value">The name of the content module to render</param>
		/// <param name="offSetTop">Override 'top' css attribute to position edit link</param>
		/// <param name="offsetRight">Override 'right' css attribute to position edit link</param>
		/// <returns></returns>
		public static MvcHtmlString RenderUserContentModuleWithOnScreenEditors(this HtmlHelper helper, string value, int offSetTop, int offsetRight)
		{
			using (var context = new DataContext())
			{
				var tempMod = context.ContentModules.FirstOrDefault(x => x.ModuleName == value);
				if (tempMod != null)
				{
					// Give logged in admins a way to quickly edit the module
					if (UserUtils.UserIsAdmin())
					{
                        value = String.Format(
                            "<div class='adminButtons' style='top:{2}px; right:{3}px;'>" +
								"<a class='adminEdit onScreen' href='/admin/editmodule/{0}' target='_blank' data-id='{0}'>Edit " + 
									"<img src='/Areas/Admin/Content/themes/base/Images/icons/edit_white.png' class='adminIcon'>" + 
								"</a>" +
								"<a class='adminRawEdit onScreen' href='/admin/editmodule/{0}' target='_blank' data-id='{0}'>Edit {{ }}</a>" +
                            "</div>{1}",
                            tempMod.ContentModuleId, tempMod.HTMLContent, offSetTop, offsetRight
                        );
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