using System;
using System.ComponentModel.DataAnnotations;

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
}