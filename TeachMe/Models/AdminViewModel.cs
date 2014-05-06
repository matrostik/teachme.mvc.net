using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    public class AdminIndexViewModel
    {
        /// <summary>
        /// List of all Users
        /// </summary>
        public List<ApplicationUser> Users { get; set; }

        /// <summary>
        /// Total users count
        /// </summary>
        public int UsersCount { get { return Users.Count; } }

        /// <summary>
        /// Unconfirmed users count
        /// </summary>
        /// <returns></returns>
        public int GetUncofirmedUsersCount
        {
            get
            {
                return Users == null ? 0 : Users.Count(u => !u.IsConfirmed);
            }
        }

    }

    public class AdminUsersViewModel
    {
        /// <summary>
        /// List of all Users
        /// </summary>
        public List<ApplicationUser> Users { get; set; }

        public string SortParm { get; set; }
    }

    public class AdminUserDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public string Roles
        {
            get
            {
                return string.Join(",", User.Roles.Select(x => x.Role.Name));
            }
        }
    }
}