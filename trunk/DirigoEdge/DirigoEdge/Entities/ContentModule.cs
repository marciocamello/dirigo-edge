using System;
using System.ComponentModel.DataAnnotations;

public class ContentModule
{
	[Key]
	public virtual int ContentModuleId { get; set; }

	[Required]
	public virtual String ModuleName { get; set; }
	public virtual String HTMLContent { get; set; }
	public virtual String CSSContent { get; set; }
	public virtual String JSContent { get; set; }
	public virtual DateTime? CreateDate { get; set; }
	public virtual Boolean? IsActive { get; set; }
}