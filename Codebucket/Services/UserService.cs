using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Codebucket.Services
{
    public class UserService
    {
        private readonly IAppDataContext _db;

        #region Constructor.
        /// <summary>
        /// Constructor, makes a new instance of database connection. If parameter is null it sets the _db as
        /// 'new ApplicationDbContext' else it takes the 'IAppDataContext context' parameter and uses that 
        /// (used for unit testing).
        /// </summary>
        /// <param name="context"></param>
        public UserService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
        }
        #endregion

        #region Get project members and owners.
        /// <summary>
        /// Currently not being used but will be used in further implementation on this program. 
        /// ----
        /// Gets all project owners by project ID, if project ID is not valid return null.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>List of 'ProjectOwner'</returns>
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
        /// <summary>
        /// Gets all project members by project ID, if project ID is not valid return null.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>List of 'ProjectMember'</returns>
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
        /// <summary>
        /// Get all project members by project id, if project ID is not valid return null.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>List of 'ProjectMemberViewModel'</returns>
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

        /// <summary>
        /// Get a single project member by project member ID.
        /// </summary>
        /// <param name="projectMemberID">Project Member ID</param>
        /// <returns>'ProjectMemberViewModel'</returns>
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

        /// <summary>
        /// Gets owner of project by project ID, if project ID is not valid return null.
        /// </summary>
        /// <param name="projectID">Project ID</param>
        /// <returns>String</returns>
        public string getOwnerName(int projectID)
        {
            ProjectOwner ownerInProject = (from owned in _db._projectOwners
                                           where owned._projectID == projectID
                                           select owned).FirstOrDefault();

            if (ownerInProject == null)
            {
                return null;
            }

            string owner = ownerInProject._userName;

            return owner;
        }
        #endregion

        #region Add Contact logs.
        /// <summary>
        /// Take data from 'ConctactLogViewModel' and save to Db table 'contactLog', if model is not valid return false.
        /// </summary>
        /// <param name="model">'ContactLogViewModel'</param>
        /// <returns>bool</returns>
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
        /// <summary>
        /// Gets project by project ID and user by username if both are valid adds a
        /// new 'ProjectMember' to Db.
        /// </summary>
        /// <param name="model">'AddMemberViewModel'</param>
        public void addProjectMember(AddMemberViewModel model)
        {
            ProjectMember newProjectMember = new ProjectMember();

            // Select project from db that corresponds to user selected/entered project
            var project = (from p in _db._projects
                          where p.ID == model._projectID
                          select p).SingleOrDefault();

            // Select username from db that corresponds to user selected/entered username
            var user = (from u in _db.Users
                       where u.UserName == model._userName
                       select u).SingleOrDefault();

            if (project != null && user != null)
            {
                newProjectMember._projectID = project.ID;
                newProjectMember._userName = user.UserName;

                _db._projectMembers.Add(newProjectMember);
                _db.SaveChanges();
            }
        }
        #endregion

        #region Check if owner or username exists.
        /// <summary>
        /// Checks if username exists in the database, returns a bool value if true or not.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>'bool'</returns>
        public bool userIsInDataBase(string username)
        {
            string getUserName = (from user in _db.Users
                                  where user.UserName == username
                                  select user.UserName).FirstOrDefault();

            return (getUserName == username);
        }

        /// <summary>
        /// Check if project is owner or member of project by username and project ID.
        /// Uses two functions (isProjectOwner and isProjectMember)
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="projectID">Project ID</param>
        /// <returns>bool</returns>
        public bool isProjectOwnerOrMember(string username, int projectID)
        {
            return (isProjectOwner(username, projectID) || isProjectMember(username, projectID));
        }

        /// <summary>
        /// Checks if username is owner of project by username and project ID, returns a bool value if true or not.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="projectID">Project ID</param>
        /// <returns>bool</returns>
        public bool isProjectOwner(string username, int projectID)
        {
            ProjectOwner ownerInProject = (from owned in _db._projectOwners
                                           where owned._userName == username && owned._projectID == projectID
                                           select owned).FirstOrDefault();

            return (ownerInProject != null);
        }

        /// <summary>
        /// Checks if username is owner of project by username and project ID, returns a bool value if true or not.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="projectID">Project ID</param>
        /// <returns>bool</returns>
        public bool isProjectMember(string username, int projectID)  //MOVME::This belongs in ApplicationUserService??
        {
            ProjectMember memberInProject = (from member in _db._projectMembers
                                             where member._userName == username && member._projectID == projectID
                                             select member).FirstOrDefault();

            return (memberInProject != null);
        }

        /// <summary>
        /// Check is user is a member of any project, returns a bool value if true or not.
        /// </summary>
        /// <param name="memberID">Project member ID</param>
        /// <returns>bool</returns>
        public bool isProjectMemberInAnyProject(int memberID)
        {
            ProjectMember memberInProject = (from member in _db._projectMembers
                                             where member.ID == memberID
                                             select member).FirstOrDefault();

            return (memberInProject != null);
        }
        #endregion

        #region Delete member.
        /// <summary>
        /// Deletes a member from Db by username and project ID.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="projectID">Project ID</param>
        public void deleteProjectMemberByUserNameAndProjectID(string userName, int projectID)
        {
            ProjectMember memberFound = (from m in _db._projectMembers
                                         where m._projectID == projectID && userName == m._userName
                                         select m).FirstOrDefault();

            deleteProjectMember(memberFound.ID);
        }

        /// <summary>
        /// Deletess a member from Db by project member ID.
        /// </summary>
        /// <param name="projectMemberID">Project member ID</param>
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