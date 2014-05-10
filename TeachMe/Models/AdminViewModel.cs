using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public IPagedList<ApplicationUser> Users { get; set; }

        public string SortParm { get; set; }

        public string Filter { get; set; }
    }

    public class AdminUserDetailsViewModel
    {
        public ApplicationUser User { get; set; }

        public Teacher Teacher { get; set; }

        [Display(Name = "תפקידים")]
        public string Roles
        {
            get
            {
                return string.Join(",", User.Roles.Select(x => x.Role.Name));
            }
        }

        /// <summary>
        /// Set edit mode
        /// </summary>
        public bool InEditMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Disabled { get { return InEditMode ? "" : "disabled"; } }
    }
}