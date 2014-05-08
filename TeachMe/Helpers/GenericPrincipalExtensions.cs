using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace TeachMe.Helpers
{
    public static class GenericPrincipalExtensions
    {
        /// <summary>
        /// Get user firstname
        /// </summary>
        /// <param name="user">IPrincipal user</param>
        /// <returns>firstname</returns>
        public static string FirstName(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                try
                {
                    ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
                    var claim = claimsIdentity.FindFirst("FirstName");
                    return claim.Value;
                }
                catch (Exception)
                { }
                return "";
            }
            else
                return "";
        }

        /// <summary>
        /// Get user lastname
        /// </summary>
        /// <param name="user">IPrincipal user</param>
        /// <returns>lastname</returns>
        public static string LastName(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                try
                {
                    ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
                    var claim = claimsIdentity.FindFirst("LastName");
                    return claim.Value;
                }
                catch (Exception)
                { }
                return "";
            }
            else
                return "";
        }

        /// <summary>
        /// Get user fullname
        /// </summary>
        /// <param name="user">IPrincipal user</param>
        /// <returns>fullname</returns>
        public static string FullName(this IPrincipal user)
        {
            return user.FirstName() + " " + user.LastName();
        }
    }
}