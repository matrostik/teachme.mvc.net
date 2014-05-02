using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    /// <summary>
    /// TeachMeDBContext connection
    /// used to work with database
    /// </summary>
    public class TeachMeDBContext : DbContext
    {
        public TeachMeDBContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
    }
}