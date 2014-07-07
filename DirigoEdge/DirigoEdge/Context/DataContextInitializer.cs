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
		// Set up Content
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
			SearchIndex = true,
			ContentPageRevisionsEnabled = false,
			ContentPageRevisionsRetensionCount = 10
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

        // Must Save the slideshow before we can add slides
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
			ModuleName = "homepage-module-1",
            HTMLContent = "<p><img src=\"http://placehold.it/400x300&amp;text=[img]\" alt=\"Placeholder Image\"></p><h4>This is an editable content area.</h4><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>"
		});

		context.ContentModules.Add(new ContentModule
		{
			CreateDate = DateTime.Now,
			ModuleName = "homepage-module-2",
            HTMLContent = "<p><img src=\"http://placehold.it/400x300&amp;text=[img]\" alt=\"Placeholder Image\"></p><h4>This is an editable content area.</h4><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>"
		});

		context.ContentModules.Add(new ContentModule
		{
			CreateDate = DateTime.Now,
			ModuleName = "homepage-module-3",
			HTMLContent = "<p><img src=\"http://placehold.it/400x300&amp;text=[img]\" alt=\"Placeholder Image\"></p><h4>This is an editable content area.</h4><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>"
		});

		context.ContentModules.Add(new ContentModule
		{
			CreateDate = DateTime.Now,
			ModuleName = "header-logo-image",
			HTMLContent = "<a href=\"/\"><img src=\"http://placehold.it/400x100&amp;text=Logo\" alt=\"Placeholder Logo Image\"></a>"
		});

        context.ContentModules.Add(new ContentModule
        {
            CreateDate = DateTime.Now,
            ModuleName = "header",
            HTMLContent = "<div class=\"three columns\"> <a class=\"logoHead\" href=\"/\"><img alt=\"Edge Logo\" src=\"/content/uploaded/images/content/edge_logo_head.png\" /></a> </div>  <nav class=\"nine columns\"> <ul class=\"nav-bar right\"> <li><a href=\"/\">Home</a></li> <li><a href=\"/about\">About</a></li>         <li><a href=\"/contact\">Contact</a></li>         <li><a href=\"/blog\">Blog</a></li> </ul> </nav>"
        });

        // Homepage
	    context.ContentPages.Add(new ContentPage
	    {
            ContentPageId = 1,            
            Permalink = "home",
            DisplayName = "home",
            Title = "Home",
            CreateDate = DateTime.Now,
            IsActive = true,
            HTMLContent = "<div class=\"row\"><div class=\"twelve columns\"><h3>Welcome Home</h3><p><img src=\"http://placehold.it/1200x400&amp;text=[Hero Image]\"></p></div></div><div class=\"row\"><div class=\"four columns\">[homepage-module-1]</div><div class=\"four columns\">[homepage-module-2]</div><div class=\"four columns\">[homepage-module-3]</div></div> <div class=\"row\"><div class=\"twelve columns\"><div class=\"panel\"><h4>Get in touch!</h4><div class=\"row\"><div class=\"nine columns\"><p>We'd love to hear from you, you attractive person you.</p></div><div class=\"three columns\"><a class=\"radius button right\" href=\"/contact\">Contact Us</a></div></div></div></div></div>"
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