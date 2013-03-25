using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirigoEdge.Entities
{
	public class SlideshowModule
	{
		[Key]
		public int SlideshowModuleId { get; set; }
		public string SlideShowName { get; set; }
		public virtual IList<Slide> Slides { get; set; }

		// Slideshow settings
		public virtual int AdvanceSpeed { get; set; }
		public virtual String Animation { get; set; }
		public virtual int AnimationSpeed { get; set; }
		public virtual bool UseTimer { get; set; }
		public virtual bool PauseOnHover { get; set; }

		public virtual bool UseDirectionalNav { get; set; }
		public virtual bool ShowBullets { get; set; }
	}

	public class Slide
	{
		[Key]
		public int Id { get; set; }

		public virtual string ImageLocation { get; set; }
		public virtual string HtmlContent { get; set; }
		public virtual string Caption { get; set; }
	}

}