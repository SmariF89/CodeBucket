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
        IDbSet<ContactLog> _contacts { get; set; }
        IDbSet<ApplicationUser> Users { get; set; }

        int SaveChanges();
        //System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity);
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDataContext
    {
        public IDbSet<Project> _projects { get; set; }
        public IDbSet<ProjectFile> _projectFiles { get; set; }
        public IDbSet<ProjectOwner> _projectOwners { get; set; }
        public IDbSet<ProjectMember> _projectMembers { get; set; }
        public IDbSet<FileType> _fileTypes { get; set; }
        public IDbSet<ExceptionLogger> _exceptions { get; set; }
        public IDbSet<ContactLog> _contacts { get; set; }

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