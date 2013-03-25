using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirigoEdge.Entities
{
	public class SiteSettings
	{
		[Key]
		public virtual int BlogSettingsId { get; set; }

		// General
		public virtual string ContactEmail { get; set; }
		public virtual bool SearchIndex { get; set; }
	}
}