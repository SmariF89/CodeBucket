using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Codebucket.Models.Entities;

namespace Codebucket.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IAppDataContext
    {
        IDbSet<Project> _projects { get; set; }
        IDbSet<ProjectFile> _projectFiles { get; set; }
        IDbSet<ProjectOwner> _projectOwners { get; set; }
        IDbSet<ProjectMember> _projectMembers { get; set; }
        IDbSet<FileType> _fileTypes { get; set; }
        IDbSet<ExceptionLogger> _exceptions { get; set; }
        //IDbSet<ContactLog> _contacts { get; set; }

        int SaveChanges();
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //IAppDataContext
    {
        public DbSet<Project> _projects { get; set; }
        public DbSet<ProjectFile> _projectFiles { get; set; }
        public DbSet<ProjectOwner> _projectOwners { get; set; }
        public DbSet<ProjectMember> _projectMembers { get; set; }
        public DbSet<FileType> _fileTypes { get; set; }
        public DbSet<ExceptionLogger> _exceptions { get; set; }
        public DbSet<ContactLog> _contacts { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        ///Uncomment this method in order to add new table to db.

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}