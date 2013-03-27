using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class BlogAdminModule
{
	[Key]
	public virtual int BlogAdminModuleId { get; set; }

	[Required]
	public virtual int OrderNumber { get; set; }
	public virtual int ColumnNumber { get; set; }
	public virtual String ModuleName { get; set; }
	public virtual bool IsCollapsed { get; set; }
	public virtual User User { get; set; }
}