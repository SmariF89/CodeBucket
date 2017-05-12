using System.Data.Entity;
using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;

namespace Codebucket.Tests
{
    class MockDataContext : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDataContext()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            _projects       = new InMemoryDbSet<Project>();
            _projectFiles   = new InMemoryDbSet<ProjectFile>();
            _projectOwners  = new InMemoryDbSet<ProjectOwner>();
            _projectMembers = new InMemoryDbSet<ProjectMember>();
            _fileTypes      = new InMemoryDbSet<FileType>();
            _exceptions     = new InMemoryDbSet<ExceptionLogger>();
            _contacts       = new InMemoryDbSet<ContactLog>();
            Users           = new InMemoryDbSet<ApplicationUser>();
            UserViewModel = new InMemoryDbSet<ApplicationUserViewModel>();
            CreateProjectViewModel = new InMemoryDbSet<CreateProjectViewModel>();
        }

        public IDbSet<Project> _projects               { get; set; }
        public IDbSet<ProjectFile> _projectFiles       { get; set; }
        public IDbSet<ProjectOwner> _projectOwners     { get; set; }
        public IDbSet<ProjectMember> _projectMembers   { get; set; }
        public IDbSet<FileType> _fileTypes             { get; set; }
        public IDbSet<ExceptionLogger> _exceptions     { get; set; }
        public IDbSet<ContactLog> _contacts            { get; set; }
        public IDbSet<ApplicationUser> Users           { get; set; }
        public IDbSet<ApplicationUserViewModel> UserViewModel { get; set; }
        public IDbSet<CreateProjectViewModel> CreateProjectViewModel { get; set; }
        // TODO: bætið við fleiri færslum hér
        // eftir því sem þeim fjölgar í AppDataContext klasanum ykkar!


        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}
