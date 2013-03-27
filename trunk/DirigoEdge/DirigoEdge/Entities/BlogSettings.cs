using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirigoEdge.Entities
{
	public class BlogSettings
	{
		[Key]
		public virtual int BlogSettingsId { get; set; }

		// General
		public virtual string BlogTitle { get; set; }
		public virtual int MaxBlogsOnHomepageBeforeLoad { get; set; }
		public virtual bool DisableAllCommentsGlobal { get; set; }

		// Facebook
		public virtual bool ShowFacebookLikeButton { get; set; }
		public virtual bool ShowFacebookComments { get; set; }
		public virtual string FacebookAppId { get; set; }

		// Disqus
		public virtual bool ShowDisqusComents { get; set; }
		public virtual string DisqusShortName { get; set; }
	}
}