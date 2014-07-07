using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DirigoEdge.Areas.Admin.Models
{
    public class PermissionsFilter : AuthorizeAttribute
    {
        public string Permissions { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                // The user is not authenticated
                return false;
            }

            // Check Required Permissions
            var user = Membership.GetUser();
            List<string> requiredPermissionList = Permissions.Split(',').Select(x => x.Trim()).ToList();
            using (var context = new DataContext())
            {
                var theUser = context.Users.FirstOrDefault(x => x.Username == user.UserName);

                // Loop through the required permissions, see if any of the User's roles have that permission
                // If so, immediately return true since we've found an acceptance
                foreach (string reqPermission in requiredPermissionList)
                {
                    // Loop through the roles to see if any permissions are found
                    foreach (var role in theUser.Roles)
                    {
                        // Admins get everything
                        if (role.RoleName == "Administrators")
                        {
                            return true;
                        }

                        // Otherwise check parameters
                        Dictionary<string, bool> rolePerms =Utils.UserRoleUtilities.GetRolePermissionsList(role.Permissions);
                        if (rolePerms.ContainsKey(reqPermission) && rolePerms[reqPermission])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Admin/NotAuthorized");
        }
    }

    public class UserIsLoggedIn : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);

            return authorized;
        }
    }
}