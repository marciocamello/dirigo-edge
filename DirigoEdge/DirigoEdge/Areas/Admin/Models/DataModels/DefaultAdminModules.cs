using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.DataModels
{
	public static class DefaultAdminModules
	{
		public static List<BlogAdminModule> GetDefaultAdminModules(User user)
		{
			return new List<BlogAdminModule>
			{
				// == Left Column
				new BlogAdminModule
				{
					ModuleName = "FeaturedTextModule",
					ColumnNumber = 1,
					OrderNumber = 0,
					User = user
				},

				// == Right Column
				new BlogAdminModule
				{
					ModuleName = "PublishModule",
					OrderNumber = 0,
					ColumnNumber = 2,
					User = user
				},
				new BlogAdminModule
				{
					ModuleName = "AuthorModule",
					OrderNumber = 1,
					ColumnNumber = 2,
					User = user
				},
				new BlogAdminModule
				{
					ModuleName = "CategoriesModule",
					OrderNumber = 2,
					ColumnNumber = 2,
					User = user
				},
				new BlogAdminModule
				{
					ModuleName = "TagsModule",
					OrderNumber = 3,
					ColumnNumber = 2,
					User = user
				},
				new BlogAdminModule
				{
					ModuleName = "SEOMetaModule",
					OrderNumber = 4,
					ColumnNumber = 2,
					User = user,
					IsCollapsed = true
				},
				new BlogAdminModule
				{
					ModuleName = "FeaturedImageModule",
					OrderNumber = 5,
					ColumnNumber = 2,
					User = user
				}
			};
		}

        public static List<EventAdminModule> GetEventAdminModules(User user)
        {
            return new List<EventAdminModule>
			{
				// == Left Column
				new EventAdminModule
				{
					ModuleName = "FeaturedTextModule",
					OrderNumber = 0,
					ColumnNumber = 1,
					User = user
				},

				// == Right Column
                new EventAdminModule
				{
					ModuleName = "PublishModule",
					OrderNumber = 0,
					ColumnNumber = 2,
					User = user
				},
				new EventAdminModule
				{
					ModuleName = "CategoriesModule",
					OrderNumber = 1,
					ColumnNumber = 2,
					User = user
				},
				new EventAdminModule
				{
					ModuleName = "FeaturedImageModule",
					OrderNumber = 3,
					ColumnNumber = 2,
					User = user
				}
			};
        }
	}
}