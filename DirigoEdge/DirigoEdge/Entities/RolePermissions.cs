using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

/// <summary>
/// Must have a DisplayName to show up in Listing
/// </summary>
public class RolePermissions
{
    [Key]
    public virtual int RolePermissionsId { get; set; }

    [Display(Name = "Can Edit Blogs")]
    public bool CanEditBlogs { get; set; }

    [Display(Name = "Can Edit Blog Categories")]
    public bool CanEditBlogCategories { get; set; }

    [Display(Name = "Can Edit Blog Authors")]
    public bool CanEditBlogAuthors { get; set; }

    [Display(Name = "Can Edit Pages")]
    public bool CanEditPages { get; set; }

    [Display(Name = "Can Edit Modules")]
    public bool CanEditModules { get; set; }

    // User / Role Permissions
    [Display(Name = "Can Edit Users")]
    public bool CanEditUsers { get; set; }

    [Display(Name = "Can Edit Settings")]
    public bool CanEditSettings { get; set; }

    [Display(Name = "Can Manage Media")]
    public bool CanManageMedia { get; set; }

    [Display(Name = "Can Edit Events")]
    public bool CanEditEvents { get; set; }
}