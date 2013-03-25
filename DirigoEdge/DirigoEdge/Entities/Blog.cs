using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Blog
{
	[Key]
	public virtual int BlogId { get; set; }

	[Required]
	public virtual String Title { get; set; }
    public virtual String HtmlContent { get; set; }
    public virtual String MetaDescription { get; set; }
    public virtual String Tags { get; set; }
    public virtual Boolean IsActive { get; set; }
    public virtual String Author { get; set; }
    public virtual String OGTitle { get; set; }
    public virtual String OGImage { get; set; }
    public virtual String OGType { get; set; }
    public virtual String OGUrl { get; set; }
    public virtual String RobotsNoFollow { get; set; }
    public virtual String Canonical { get; set; }
    public virtual Boolean AllowComments { get; set; }
	public virtual ICollection<BlogCategory> BlogCategories { get; set; }
	public virtual DateTime Date { get; set; }
    public virtual String ImageUrl { get; set; }
    public virtual String ShortDesc { get; set; }
    public virtual Int32 AuthorId { get; set; }
    public virtual Boolean IsFeatured { get; set; }
    public virtual String PermaLink { get; set; }
    public virtual String MainCategory { get; set; }
}