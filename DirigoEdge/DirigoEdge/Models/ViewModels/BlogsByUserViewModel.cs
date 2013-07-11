using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogsByUserViewModel
	{
		public List<Blog> BlogRoll;
		public string BlogUsername;

		public BlogsByUserViewModel(string username)
		{
			// Get back to the original name before url conversion
			BlogUsername = username.Replace(ContentGlobals.BLOGDELIMMETER, " ");

			using (var context = new DataContext())
			{
				BlogRoll = context.Blogs.Where(x => x.Author == BlogUsername && x.IsActive).ToList();
			}
		}
	}
}