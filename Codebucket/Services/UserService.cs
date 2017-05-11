using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Codebucket.Services
{
    public class UserService
    {
        private ApplicationDbContext _db;

        #region Constructor.
        // Constructor, makes a new instance of database connection.
        public UserService()
        {
            _db = new ApplicationDbContext();
        }
        #endregion

        #region Get project members and owners.
        // Get all project owners by project ID, Returns a list of the entitiy ProjectOwner.
        
        //public List<ProjectOwner> getAllProjectOwnersByProjectId(int? id)
        //{
        //    if (id != null || id > 0)
        //    {
        //        return (from projectOwner in _db._projectOwners
        //                where projectOwner._projectID == id
        //                select projectOwner).ToList();
        //    }
        //    return null;
        //}

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
                        _userName = item._userName,
                        _id = item.ID                        
                    });
                }
                return projectMemberViewModel;
            }
            return null;
        }
        //Get a single projectMemberViewModel by project member id.
        public ProjectMemberViewModel getProjectMemberByProjectMemberID(int projectMemberID)
        {
            ProjectMemberViewModel member = new ProjectMemberViewModel();

            ProjectMember memberFound = (from m in _db._projectMembers
                                         where m.ID == projectMemberID
                                         select m).FirstOrDefault();

            member._userName = memberFound._userName;
            member._projectID = memberFound._projectID;
            member._id = memberFound.ID;

            return member;
        }

        public string getOwnerName(int projectID)
        {
            if (_db._projects.Find(projectID) == null)
            {
                return null;
            }

            ProjectOwner ownerInProject = (from owned in _db._projectOwners
                                           where owned._projectID == projectID
                                           select owned).FirstOrDefault();

            string owner = ownerInProject._userName;

            return owner;
        }
        #endregion

        #region Add Contact logs.
        public bool addContactLog(ConctactLogViewModel model)
        {
            if (model._contactName != null || model._contactEmail != null || model._contactEmail != null) // FIXME::?
            {
                ContactLog contactLog = new ContactLog
                {
                    _contactName = model._contactName,
                    _contactEmail = model._contactEmail,
                    _ContactMessage = model._contactMessage
                };
                _db._contacts.Add(contactLog);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region Add project member.
        public void addProjectMember(AddMemberViewModel model)
        {
            ProjectMember newProjectMember = new ProjectMember();

            // Select project from db that corresponds to user selected/entered project
            var project = from p in _db._projects
                          where p.ID == model._projectID
                          select p;

            // Select username from db that corresponds to user selected/entered username
            var user = from u in _db.Users
                       where u.UserName == model._userName
                       select u;

            if (project.FirstOrDefault() != null && user.FirstOrDefault() != null)
            {
                newProjectMember._projectID = project.FirstOrDefault().ID;
                newProjectMember._userName = user.FirstOrDefault().UserName;

                _db._projectMembers.Add(newProjectMember);
                _db.SaveChanges();
            }
            // TODO :: THOW EXCEPTION, else {if project or user was not found.}
        }
        #endregion

        #region Check if owner or username exists.
        // Checks if username exists in the database, returns a bool value if true or not.
        public bool userIsInDataBase(string username)
        {
            string getUserName = (from user in _db.Users
                                  where user.UserName == username
                                  select user.UserName).FirstOrDefault();

            return (getUserName == username);
        }

        public bool isProjectOwnerOrMember(string username, int projectID) //MOVME::This belongs in ApplicationUserService??
        {
            return (isProjectOwner(username, projectID) || isProjectMember(username, projectID));
        }

        // Checks if username is owner of project, returns a bool value if true or not.
        public bool isProjectOwner(string username, int projectID)  //MOVME::This belongs in ApplicationUserService??
        {
            ProjectOwner ownerInProject = (from owned in _db._projectOwners
                                           where owned._userName == username && owned._projectID == projectID
                                           select owned).FirstOrDefault();

            return (ownerInProject != null);
        }

        // Checks if username is owner of project, returns a bool value if true or not.
        public bool isProjectMember(string username, int projectID)  //MOVME::This belongs in ApplicationUserService??
        {
            ProjectMember memberInProject = (from member in _db._projectMembers
                                             where member._userName == username && member._projectID == projectID
                                             select member).FirstOrDefault();

            return (memberInProject != null);
        }

        public bool isProjectMemberInAnyProject(int memberID)
        {
            ProjectMember memberInProject = (from member in _db._projectMembers
                                             where member.ID == memberID
                                             select member).FirstOrDefault();

            return (memberInProject != null);
        }
        #endregion

        #region Delete member.
        public void deleteProjectMemberByUserNameAndProjectID(string userName, int projectID)
        {
            ProjectMember memberFound = (from m in _db._projectMembers
                                         where m._projectID == projectID && userName == m._userName
                                         select m).FirstOrDefault();

            deleteProjectMember(memberFound.ID);
        }

        public void deleteProjectMember(int projectMemberID)
        {
            ProjectMember memberToDel = (from member in _db._projectMembers
                                         where member.ID == projectMemberID
                                         select member).FirstOrDefault();

            _db._projectMembers.Remove(memberToDel);
            _db.SaveChanges();
        }
        #endregion
    }
}