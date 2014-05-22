using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

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

        /// <summary>
        /// List of teachers
        /// </summary>
        public DbSet<Teacher> Teachers { get; set; }

        /// <summary>
        /// List of comments
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// List of cities
        /// </summary>
        public DbSet<City> Cities { get; set; }

        /// <summary>
        /// List strreets of Israel
        /// </summary>
        public DbSet<Street> Streets { get; set; }

        /// <summary>
        /// List of existing subjects to study
        /// </summary>
        public DbSet<Subject> Subjects { get; set; }

        /// <summary>
        /// List of existing institutions
        /// </summary>
        public DbSet<Institution> Institutions { get; set; }

        /// <summary>
        /// List of existing SubjectToTeach
        /// </summary>
        public DbSet<SubjectToTeach> SubjectsToTeach { get; set; }
    }
}