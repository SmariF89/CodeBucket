using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Codebucket.Services
{
    public class ApplicationUserService
    {
        private ApplicationDbContext _db;

        #region Cunstuctor
        // Constructor, makes a new instance of database connection.
        public ApplicationUserService()
        {
            _db = new ApplicationDbContext();
        }
        #endregion

        #region Get project members and owners
        // Get all project owners by project ID, Returns a list of the entitiy ProjectOwner.
        public List<ProjectOwner> getAllProjectOwnersByProjectId(int? id)
        {
            if (id != null || id > 0)
            {
                return (from projectOwner in _db._projectOwners
                        where projectOwner._projectID == id
                        select projectOwner).ToList();
            }
            return null;
        }

        // Get all project members by project ID, returns a list of the entity ProjectMember.
        public List<ProjectMember> getAllProjectMembersByProjectId(int? id)
        {
            if (id != null || id > 0)
            {
                return (from projectMember in _db._projectMembers
                    where projectMember._projectID == id
                    select projectMember).ToList();
            }
            return null;
        }

        // Get all project members by project id, returns a list of viewModel ProjectMemberViewModel.
        public List<ProjectMemberViewModel> getAllProjectMemberViewModelsByProjectId(int? id)
        {
            if (id != null || id > 0)
            {
                List<ProjectMemberViewModel> projectMemberViewModel = new List<ProjectMemberViewModel>();
                List<ProjectMember> projectMember = getAllProjectMembersByProjectId(id);

                foreach (var item in projectMember)
                {
                    projectMemberViewModel.Add(new ProjectMemberViewModel
                    {
                        _projectID = item._projectID,
                        _userName = item._userName
                    });
                }

                return projectMemberViewModel;
            }
            return null;
        }
        #endregion
    }
}