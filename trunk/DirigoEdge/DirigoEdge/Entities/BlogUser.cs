using System;
using System.ComponentModel.DataAnnotations;

public class BlogUser
{
	[Key]
	public virtual int UserId { get; set; }

	[Required]
	public virtual String Username { get; set; }
	public virtual String DisplayName { get; set; }
	public virtual String UserImageLocation { get; set; }
	public virtual String Email { get; set; }
	public virtual DateTime? CreateDate { get; set; }
	public virtual Boolean? IsLockedOut { get; set; }
	public virtual Boolean IsActive { get; set; }
}