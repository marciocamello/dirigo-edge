using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DirigoEdge.Areas.Admin.Models.DataModels;
using DirigoEdge.Utils;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageUserRolesViewModel
	{
	    public Dictionary<Role, List<string>> RoleUsersKVP;
	    public List<string> RolesList;

        public Dictionary<string, bool> RolePersmissionsList;
	    
		public const string NOUSERIMAGE = "/Areas/Admin/Content/Themes/Base/Images/User.png";

        public ManageUserRolesViewModel()
		{
            RoleUsersKVP = new Dictionary<Role, List<string>>();

            RolesList = Roles.GetAllRoles().ToList();

            using (var context = new DataContext())
            {
                foreach (string role in RolesList)
                {
                    Role theRole = context.Roles.FirstOrDefault(x => x.RoleName == role);
                    if (theRole != null)
                    {
                        theRole.Permissions = theRole.Permissions;
                        RoleUsersKVP.Add(theRole, Roles.GetUsersInRole(role).ToList());
                    }
                } 
            }

            RolePersmissionsList = UserRoleUtilities.GetRolePermissionsList(new RolePermissions());
		}
	}
}