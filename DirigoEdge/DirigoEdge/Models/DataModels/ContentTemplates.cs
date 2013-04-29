using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.DataModels
{
	public class ContentTemplates
	{
		private const string _templatesLocation = "/Views/Content/Templates";

		public Dictionary<string, ContentTemplate> Templates;

		public ContentTemplates()
		{
			addTemplates();
		}

		private void addTemplates()
		{
			Templates = new Dictionary<string, ContentTemplate>();

			// Get list of Template files in this location
			var templateList = System.IO.Directory.EnumerateFiles(HttpContext.Current.Server.MapPath(string.Format("~{0}", _templatesLocation)));

			// Loop through each template in the folder and add them to the collection
			foreach (string template in templateList)
			{
				string[] ignorePath = new string[] { @"\Views\Content\Templates\" };
				string[] ignoreExtenstion = new string[] { ".cshtml" };

				// split parts of file path
				string[] filename = template.Split(ignorePath, StringSplitOptions.RemoveEmptyEntries);
				string[] templateName = filename[1].Split(ignoreExtenstion, StringSplitOptions.RemoveEmptyEntries); // includes the word "Template"
				string[] templateShortName = templateName[0].Split(new string[] { "Template" }, StringSplitOptions.RemoveEmptyEntries); // stripped "Template"

				// add the template to the list
				ContentTemplate newTemplate = new ContentTemplate();
				newTemplate.TemplateName = templateShortName[0];
				newTemplate.ViewLocation = string.Format("{0}/{1}", _templatesLocation, filename[1]);
				Templates.Add(newTemplate.TemplateName.ToLower(), newTemplate);
			}
		}
	}

	public class ContentTemplate
	{
		public string TemplateName { get; set; }
		public string ViewLocation { get; set; }
	}
}