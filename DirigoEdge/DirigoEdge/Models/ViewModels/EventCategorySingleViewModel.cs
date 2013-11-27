using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DirigoEdge.Models.ViewModels
{
	public class EventCategorySingleViewModel
	{
		public EventCategory TheCategory;
		public List<Event> EventRoll;
		public List<string> ImageList;

		private static readonly Random random = new Random();
		private static readonly object syncLock = new object();
		private HttpServerUtilityBase _server;

		public EventCategorySingleViewModel(string category, HttpServerUtilityBase server)
		{
			_server = server;

			category = formatCategoryString(category);

			//ImageList = getImageList();


			using (var context = new DataContext())
			{
                var tomorrow = DateTime.Now.Date;
				TheCategory = context.EventCategories.FirstOrDefault(x => x.CategoryName == category);

                EventRoll = context.Events.Where(x => x.MainCategory == category && x.IsActive == true && DateTime.Compare(x.EndDate.Value, tomorrow) >= 0).ToList();

				// Set a random picture on the eventRoll if none is currently set
				//foreach (var event in EventRoll)
				//{
				//	if (String.IsNullOrEmpty(event.ImageUrl))
				//	{
				//		event.ImageUrl = getRandomImage();
				//	}

				//}
			}
		}

		private string getRandomImage()
		{
			// http://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
			lock (syncLock)
			{
				// synchronize
				int index = random.Next(ImageList.Count);
				return ImageList[index];
			}
		}

		private List<string> getImageList()
		{
			var imageList = new List<string>();

			string directory = _server.MapPath("~/Content/StockImages/");
			string[] images = Directory.GetFiles(directory, "*.jpg");

			foreach (string image in images)
			{
				imageList.Add("/Content/StockImages/" + Path.GetFileName(image));
			}

			return imageList;

		}
		private string formatCategoryString(string category)
		{
			// Handle edge case on ampersand:
			// ex : branding-&-strategy, development-&-execution
			category = category.Replace("-and-", "-&-");

			// E-Commerce should not have it's dash removed
			if (category.ToLower() != "e-commerce")
			{
				category = category.Replace(Utils.ContentGlobals.BLOGDELIMMETER, " ");
			}

			return category;
		}
	}
}