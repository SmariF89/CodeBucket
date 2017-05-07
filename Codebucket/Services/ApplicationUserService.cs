using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebucket.Services
{
    public class ApplicationUserService
    {
        private ApplicationDbContext _db;

        public ApplicationUserService()
        {
            _db = new ApplicationDbContext();
        }

        public ApplicationUserViewModel getApplicationUserById(int? id)
        {
            return null;
        }

        public List<ProjectOwner> getAllProjectOwnersByProjectId(int? id)
        {
            List<ProjectOwner> newProjectOwner = (from projectOwner in _db._projectOwners
                                                     where projectOwner._projectID == id
                                                     select projectOwner).ToList();
            return newProjectOwner;
        }

        public List<ProjectMember> getAllProjectMembersByProjectId(int? id)
        {
            List<ProjectMember> newProjectMember = (from projectMember in _db._projectMembers
                                                    where projectMember._projectID == id
                                                    select projectMember).ToList();
            return newProjectMember;
        }

        public List<ProjectMemberViewModel> getAllProjectMemberViewModelsByProjectId(int? id)
        {
            List<ProjectMemberViewModel> newProjectMemberViewModel = new List<ProjectMemberViewModel>();
            List<ProjectMember> newProjectMember = new List<ProjectMember>();

            newProjectMember = getAllProjectMembersByProjectId(id);

            foreach (var item in newProjectMember)
            {
                newProjectMemberViewModel.Add(new ProjectMemberViewModel
                {
                    _projectID = item._projectID,
                    _userName = item._userName
                });
            }

            return newProjectMemberViewModel;
        }
    }
}