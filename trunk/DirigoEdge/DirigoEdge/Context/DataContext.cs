using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DirigoEdge.Entities;

public class DataContext : DbContext
{
    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //{
    //    modelBuilder.Conventions.Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();
    //}

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

	// Basic Content
	public DbSet<Blog> Blogs { get; set; }
	public DbSet<BlogCategory> BlogCategories { get; set; }
	public DbSet<BlogUser> BlogUsers { get; set; }
	public DbSet<ContentPage> ContentPages { get; set; }
	public DbSet<ContentModule> ContentModules { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }

	// SlideShow
	public DbSet<SlideshowModule> SlideshowModules { get; set; }
	public DbSet<Slide> Slides { get; set; }

	// Settings
	public DbSet<BlogSettings> BlogSettings { get; set; }
	public DbSet<SiteSettings> SiteSettings { get; set; }
	public DbSet<FeatureSettings> FeatureSettings { get; set; }

	// Blog Admin Settings
	public DbSet<BlogAdminModule> BlogAdminModules { get; set; }

    // Event Admin Settings
    public DbSet<EventAdminModule> EventAdminModules { get; set; }

	// Content Page Revisions
	public DbSet<ContentPageRevision> ContentPageRevisions { get; set; }

    // User Roles Permissions
    public DbSet<RolePermissions> RolePermissions { get; set; }

    // Plugins from App Store
    public DbSet<Plugin> Plugins { get; set; }
    
    // Schema Editor
    public DbSet<Schema> Schemas { get; set; }
}