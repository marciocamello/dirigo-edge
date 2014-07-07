using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
    public class ManageBlogAuthorsViewModel
    {
        public List<BlogUser> Users;
        public const string NOUSERIMAGE = "/Areas/Admin/Content/Themes/Base/Images/User.png";

        public ManageBlogAuthorsViewModel()
        {
            using (var context = new DataContext())
            {
                Users = context.BlogUsers.OrderBy(x => x.DisplayName).ToList();
            }

            // Make sure all users have a thumbnail of some sort
            foreach (var user in Users)
            {
                user.UserImageLocation = String.IsNullOrEmpty(user.UserImageLocation) ? NOUSERIMAGE : user.UserImageLocation;
            }
        }
    }
}