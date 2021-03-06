﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DirigoEdge.Entities;
using DirigoEdge.Utils;

public class ContentPage
{
	[Key]
	public virtual int ContentPageId { get; set; }
	public virtual String DisplayName { get; set; }
	public virtual String Title { get; set; }
	public virtual String Permalink { get; set; }

    // Stores the html content that is formatted on save. Does not parse shortcodes.
	public virtual String HTMLContent { get; set; }

    // Store html that hasn't been run through a templating script such as mustache.js
    public virtual String HTMLUnparsed { get; set; }

	/// <summary>
	/// Returns Html Content with included module html, if any. Use this when outputting to a page
	/// </summary>
	public String HTMLContentFormatted
	{
		get
		{
			return ContentUtils.GetFormattedPageContent(HTMLContent);
		}
	}

	public virtual String CSSContent { get; set; }
	public virtual String JSContent { get; set; }
	public virtual DateTime? CreateDate { get; set; }
	public virtual Boolean? IsActive { get; set; }
	public virtual DateTime? PublishDate { get; set; }
	public virtual String Template { get; set; }

	// SEO Related
	public virtual String MetaDescription { get; set; }
	public virtual String OGTitle { get; set; }
	public virtual String OGImage { get; set; }
	public virtual String OGType { get; set; }
	public virtual String OGUrl { get; set; }
	public virtual String RobotsNoFollow { get; set; }
	public virtual String Canonical { get; set; }

    // revisions stored as pages with this flag set to true
    public virtual bool IsRevision { get; set; }
    // If a draft, reference the parent page
    public virtual int? ParentContentPageId { get; set; }
    public virtual String DraftAuthorName { get; set; }

	// Keep track of changes / revisions
	public virtual ICollection<ContentPageRevision> Revisions { get; set; }

    public virtual int? SchemaId { get; set; }
    public virtual string SchemaEntryValues { get; set; }
}