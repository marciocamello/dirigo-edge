using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogArchiveViewModel
	{
        public List<Blog> Blogs;
        public int BlogRollCount = 50;
        public int MaxArchiveCount = 6;
        public string LastMonth;

	    public IEnumerable<string> Grouped;

		public bool ReachedMaxBlogs;

        public BlogArchiveViewModel()
		{
			using (var context = new DataContext())
			{
                Blogs = context.Blogs.Where(x => x.IsActive)
                            .OrderByDescending(blog => blog.Date)
                            .ToList();

			    Grouped = (from p in Blogs
			        group p by
			            new {month = p.Date.ToString("MMM"), year = p.Date.ToString("yyyy"), dateString = p.Date.ToString("MM/yyyy")}
			            into d
			        select
			            String.Format(
                            "<a href=\"/blog?date={3}\" class=\"archive\">" +
                            "<span class=\"dateRef\" data-date=\"{3}\">{0} {1}</span>  ({2}) </a>",
                            d.Key.month, d.Key.year, d.Count(), d.Key.dateString)
                ).Take(MaxArchiveCount);

			    if (Grouped.Any())
			    {
			        LastMonth = Blogs.LastOrDefault().Date.ToString("MM/yyyy");
			    }

			}
		}
	}
}