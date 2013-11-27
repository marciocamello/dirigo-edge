using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.ViewModels
{
	public class BlogsCategoriesViewModel
	{

        public List<BlogCatExtraData> Categories;
	    public String CurrentCategory;

        public BlogsCategoriesViewModel(string current)
        {
            CurrentCategory = current;

			using (var context = new DataContext())
			{

                Categories = new List<BlogCatExtraData>();

                var cats = context.BlogCategories.Where(x => x.IsActive).ToList();
                foreach (var cat in cats)
                {
                    int count = context.Blogs.Count(x => x.MainCategory == cat.CategoryName && x.IsActive);
                    Categories.Add(new BlogCatExtraData() { TheCategory = cat, BlogCount = count });
                }
			}
		}

        public class BlogCatExtraData
        {
            public BlogCategory TheCategory;
            public int BlogCount;
        }
	}
}