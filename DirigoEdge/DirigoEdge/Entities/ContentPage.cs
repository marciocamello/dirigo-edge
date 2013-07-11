using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DirigoEdge.Entities;

public class ContentPage
{
	[Key]
	public virtual int ContentPageId { get; set; }
	public virtual String DisplayName { get; set; }
	public virtual String Title { get; set; }
	public virtual String Permalink { get; set; }

	public virtual String HTMLContent { get; set; }
	public virtual String CSSContent { get; set; }
	public virtual String JSContent { get; set; }
	public virtual DateTime? CreateDate { get; set; }
	public virtual Boolean? IsActive { get; set; }
	public virtual DateTime? PublishDate { get; set; }
	public virtual String Template { get; set; }

	// SEO Related
	public virtual String MetaDescription { get; set; }
	public virtual String OGTitle { get; set; }
	public virtual String OGImage { get; set; }
	public virtual String OGType { get; set; }
	public virtual String OGUrl { get; set; }
	public virtual String RobotsNoFollow { get; set; }
	public virtual String Canonical { get; set; }

	// Keep track of changes / revisions
	public virtual ICollection<ContentPageRevision> Revisions { get; set; }
}