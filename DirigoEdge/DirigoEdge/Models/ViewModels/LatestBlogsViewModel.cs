using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.ViewModels
{
    public class LatestBlogsViewModel
    {
        public List<Blog> Blogs;

        public LatestBlogsViewModel(int blogCount = 10)
        {
            using (var context = new DataContext())
            {
                Blogs = context.Blogs.OrderByDescending(x => x.Date).Take(blogCount).ToList();
            }
        }
    }
}