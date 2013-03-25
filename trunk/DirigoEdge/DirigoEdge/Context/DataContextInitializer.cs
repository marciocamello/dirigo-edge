using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;
using DirigoEdge.Entities;

public class DataContextInitializer : CreateDatabaseIfNotExists<DataContext>
{
    protected override void Seed(DataContext context)
    {
		// Setup default user for admin access
		WebSecurity.Register("admin", "admin123", "admin@yoursite.com", true, "Demo", "Demo");
		Roles.CreateRole("Administrators");
		Roles.AddUserToRole("admin", "Administrators");

		// Set up 
		AddPlaceholderModules(context);

	    // Set up some basic blog related info to showcase functionality
	    AddBlogUser(context);

	    AddBlog(context);

		AddSlideShow(context);

	    AddSiteAndBlogSettings(context);

	    context.SaveChanges();
    }

	private void AddSiteAndBlogSettings(DataContext context)
	{
		var blogSettings = new BlogSettings()
		{
			BlogTitle = "My Blog",
			MaxBlogsOnHomepageBeforeLoad = 20
		};
		context.BlogSettings.Add(blogSettings);

		var siteSettings = new SiteSettings()
		{
			SearchIndex = true
		};
		context.SiteSettings.Add(siteSettings);
	}

	// Set up a default slideshow on homepage
	private static void AddSlideShow(DataContext context)
	{
		var slideShow = new SlideshowModule
		{
			AdvanceSpeed = 5000,
			Animation = "horizontal-push",
			AnimationSpeed = 800,
			UseTimer = true,
			PauseOnHover = false,
			UseDirectionalNav = true,
			ShowBullets = true,
			SlideShowName = "HomePageSlide"
		};
		context.SlideshowModules.Add(slideShow);

		context.SaveChanges();

		// Add some slides to the slideshow
		slideShow.Slides = new List<Slide>
		{
			new Slide
			{
				Caption = "Welcome, to Edge.",
				ImageLocation = "http://placehold.it/1200x400&text=Hello. I am a slider."
			},
			new Slide
			{
				Caption = "Get started by accessing your admin panel and editing your content.",
				ImageLocation = "http://placehold.it/1200x400&text=Slide 1"
			}
		};
	}

	// Set up some dummy content modules to showcase admin editor on homepage
	private static void AddPlaceholderModules(DataContext context)
	{
		context.ContentModules.Add(new ContentModule
		{
			CreateDate = DateTime.Now,
			ModuleName = "Homepage Module 1",
			HTMLContent = "<p><img src=\"http://placehold.it/400x300&amp;text=[img]\"></p><h4>This is an editable content area.</h4><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>"
		});

		context.ContentModules.Add(new ContentModule
		{
			CreateDate = DateTime.Now,
			ModuleName = "Homepage Module 2",
			HTMLContent = "<p><img src=\"http://placehold.it/400x300&amp;text=[img]\"></p><h4>This is an editable content area.</h4><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>"
		});

		context.ContentModules.Add(new ContentModule
		{
			CreateDate = DateTime.Now,
			ModuleName = "Homepage Module 3",
			HTMLContent = "<p><img src=\"http://placehold.it/400x300&amp;text=[img]\"></p><h4>This is an editable content area.</h4><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>"
		});

		context.ContentModules.Add(new ContentModule
		{
			CreateDate = DateTime.Now,
			ModuleName = "Header Logo Image",
			HTMLContent = "<a href=\"/\"><img src=\"http://placehold.it/400x100&amp;text=Logo\" alt=\"\"></a>"
		});
	}

	private static void AddBlog(DataContext context)
	{
		context.Blogs.Add(new Blog
		{
			Author = "Anonymous",
			Date = DateTime.Now,
			IsActive = true,
			HtmlContent = "<p>This is my test blog. Woohoo!</p>",
			ShortDesc = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam.",
			ImageUrl = "/Content/StockFeaturedImages/abstract3.jpg",
			IsFeatured = true,
			Title = "Hello World!",
			Canonical = "hello-world",
			AuthorId = 1
		});
	}

	private static void AddBlogUser(DataContext context)
	{
		context.BlogUsers.Add(new BlogUser
		{
			CreateDate = DateTime.Now,
			IsActive = true,
			DisplayName = "Anonymous",
			Email = "anon@mysite.com",
			IsLockedOut = false,
			UserId = 1,
			Username = "anonymous",
			UserImageLocation = "/Areas/Admin/Content/themes/base/Images/User.png"
		});
	}
}