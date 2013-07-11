using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirigoEdge.Entities
{
	public class ContentPageRevision
	{
		[Key]
		public virtual int ContentPageRevisionId { get; set; }

		[Required]
		public virtual int ContentPageId { get; set; } // Id of the corresponding page
		
		public virtual String AuthorName { get; set; }
		public virtual DateTime DateCreated { get; set; }

		public virtual String ContentHtml { get; set; }
	}
}