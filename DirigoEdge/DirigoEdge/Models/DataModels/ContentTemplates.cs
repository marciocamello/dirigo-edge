using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.DataModels
{
	public class ContentTemplates
	{
		// Here you can define view templates for content pages
		public Dictionary<string, ContentTemplate> Templates = new Dictionary<string, ContentTemplate>
		{
			// Empty / Default Template
			{
				"blank", new ContentTemplate
				{
					TemplateName = "None",
					ViewLocation = "/Views/Content/Templates/BlankTemplate.cshtml"
				}
			}
		};
	}

	public class ContentTemplate
	{
		public string TemplateName { get; set; }
		public string ViewLocation { get; set; }
	}
}