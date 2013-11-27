using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirigoEdge.Entities
{
	public class FeatureSettings
	{
		[Key]
		public virtual int FeatureSettingsId { get; set; }

		// Events
		public virtual bool EventsEnabled { get; set; }
	}
}