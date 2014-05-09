using Microsoft.AspNet.Identity.EntityFramework;
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
    public class TeachMeDBContext : IdentityDbContext<ApplicationUser>
    {
        public TeachMeDBContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Institution> Institutions { get; set; }
    }
    
}