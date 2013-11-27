using System;
using System.ComponentModel.DataAnnotations;

public class EventCategory
{
	[Key]
	public virtual int EventCategoryId { get; set; }

	[Required]
	public virtual String CategoryName { get; set; }

    [Required]
	public virtual DateTime CreateDate { get; set; }

	public virtual Boolean IsActive { get; set; }
}