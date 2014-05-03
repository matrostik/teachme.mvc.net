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
        public int GetUncofirmedUsers
        {
            get
            {
                return Users == null ? 0 : Users.Count(u => !u.IsConfirmed);
            }
        }

    }
}