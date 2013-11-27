using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DirigoEdge.Entities;

namespace DirigoEdge.Utils
{
	public static class UserUtils
	{

		public static bool UserIsAdmin()
		{
			MembershipUser u = Membership.GetUser(HttpContext.Current.User.Identity.Name);

			return Roles.IsUserInRole("Administrators");
		}

		public static string CurrentMembershipUsername()
		{
			MembershipUser u = Membership.GetUser(HttpContext.Current.User.Identity.Name);

			return u.UserName;
		}
	}
}