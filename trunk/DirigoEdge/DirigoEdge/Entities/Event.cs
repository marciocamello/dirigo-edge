using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DirigoEdge.Utils;

public class Event
{
	[Key]
	public virtual int EventId { get; set; }

	[Required]
	public virtual String Title { get; set; }
    public virtual String HtmlContent { get; set; }
    public virtual String MetaDescription { get; set; }
    public virtual Boolean IsActive { get; set; }
    public virtual String OGTitle { get; set; }
    public virtual String OGImage { get; set; }
    public virtual String OGType { get; set; }
    public virtual String OGUrl { get; set; }
	public virtual DateTime? DateCreated { get; set; }
    public virtual DateTime? StartDate { get; set; }
    public virtual DateTime? EndDate { get; set; }
    public virtual String FeaturedImageUrl { get; set; }
    public virtual String ShortDesc { get; set; }
    public virtual Boolean IsFeatured { get; set; }
    public virtual String PermaLink { get; set; }
    public virtual int EventCategoryId { get; set; }
    public virtual String MainCategory { get; set; }
}