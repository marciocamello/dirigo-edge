using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DirigoEdge.Utils
{
    public class UserRoleUtilities
    {
        /// <summary>
        /// Returns a list of Role Permissions and their respective labels. If no (DisplayName) attribute present, will NOT be added to list.
        /// </summary>
        /// <param name="rp"></param>
        /// <returns></returns>
        public static Dictionary<string, bool> GetRolePermissionsList(RolePermissions rp)
        {
            var kvp = new Dictionary<string, bool>();

            if (rp == null)
            {
                return kvp;
            }
                
            foreach (var prop in rp.GetType().GetProperties())
            {
                // User Reflection to get custom atttributes
                Attribute[] attrs = System.Attribute.GetCustomAttributes(prop);
                var attr = attrs.FirstOrDefault();

                string DisplayName;
                if (attr is DisplayAttribute)
                {
                    var dispAttr = (DisplayAttribute)attr;
                    DisplayName = dispAttr.Name;

                    bool value = (bool)prop.GetValue(rp, null);

                    kvp.Add(DisplayName, value);
                }
            }

            return kvp;
        }

        public static bool UserHasPermission(string permissionName)
        {
            var user = Membership.GetUser();

            if (user == null)
            {
                return false;
            }

            return UserHasPermission(user.UserName, permissionName);
        }

        public static bool UserHasPermission(string userName, string permissionName)
        {
            // No user, might as well bail
            if (String.IsNullOrEmpty(userName))
            {
                return false;
            }

            // Check required permission
            using (var context = new DataContext())
            {
                var theUser = context.Users.FirstOrDefault(x => x.Username == userName);

                // Loop through the roles to see if any permissions are found
                foreach (var role in theUser.Roles)
                {
                    // Admins get everything
                    if (role.RoleName == "Administrators")
                    {
                        return true;
                    }

                    // Otherwise check parameters
                    Dictionary<string, bool> rolePerms = GetRolePermissionsList(role.Permissions);
                    if (rolePerms.ContainsKey(permissionName) && rolePerms[permissionName])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}