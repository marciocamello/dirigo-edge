using System;
using System.ComponentModel.DataAnnotations;

public class BlogCategory
{
	[Key]
	public virtual int CategoryId { get; set; }

	[Required]
	public virtual String CategoryName { get; set; }
	public virtual DateTime CreateDate { get; set; }
	public virtual Boolean IsActive { get; set; }
}