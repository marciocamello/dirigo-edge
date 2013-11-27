using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class EventAdminModule
{
	[Key]
	public virtual int EventAdminModuleId { get; set; }

	[Required]
	public virtual int OrderNumber { get; set; }
	public virtual int ColumnNumber { get; set; }
	public virtual String ModuleName { get; set; }
	public virtual bool IsCollapsed { get; set; }
	public virtual User User { get; set; }
}