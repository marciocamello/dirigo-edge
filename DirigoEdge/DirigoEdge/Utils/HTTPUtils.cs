using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Utils
{
	public static class HTTPUtils
	{
		public static string GetFullyQualifiedApplicationPath()
		{
			//Return variable declaration
			var appPath = string.Empty;

			//Getting the current context of HTTP request
			var context = HttpContext.Current;

			//Checking the current context content
			if (context != null)
			{
				//Formatting the fully qualified website url/name
				appPath = string.Format("{0}://{1}{2}{3}", context.Request.Url.Scheme, context.Request.Url.Host, context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port, context.Request.ApplicationPath);

				if (!appPath.EndsWith("/"))
				{
					appPath += "/";
				}
			}

			return appPath;
		}
	}
}