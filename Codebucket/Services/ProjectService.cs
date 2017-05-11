using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Services
{
    public class ProjectService
    {
        private ApplicationDbContext _db;
        private ProjectFileService _projectFileService = new ProjectFileService();
        private UserService _userService = new UserService();

        #region Constructor
        public ProjectService()
        {
            _db = new ApplicationDbContext();
        }
        #endregion

        #region Get all projects by user name
        public List<ProjectViewModel> getAllProjectsByApplicationUserId(string userName)
        {
            List<ProjectViewModel> newProjectViewModel = new List<ProjectViewModel>();

            newProjectViewModel = getAllOwnerProjectsByApplicationUserId(ref newProjectViewModel, userName);
            newProjectViewModel = getAllMemberProjectsByApplicationUserId(ref newProjectViewModel, userName);
            getProjectLogo(ref newProjectViewModel);

            newProjectViewModel.Sort(delegate (ProjectViewModel A, ProjectViewModel B)
            {
                return (String.Compare(A._projectName, B._projectName));
            });
            return newProjectViewModel;
        }

        private void getProjectLogo(ref List<ProjectViewModel> model)
        {
            foreach (var item in model)
            {
                if (item._projectFiles[0]._projectFileType == ".html")
                {
                    item._thumbnailUrl = "~/Content/Logos/html.png";
                }
                else if (item._projectFiles[0]._projectFileType == ".css")
                {
                    item._thumbnailUrl = "~/Content/Logos/css.png";
                }
                else if (item._projectFiles[0]._projectFileType == ".cpp")
                {
                    item._thumbnailUrl = "~/Content/Logos/cplusplus.png";
                }
                else if (item._projectFiles[0]._projectFileType == ".cs")
                {
                    item._thumbnailUrl = "~/Content/Logos/csharp.png";
                }
                else if (item._projectFiles[0]._projectFileType == ".java")
                {
                    item._thumbnailUrl = "~/Content/Logos/java.png";
                }
                else if (item._projectFiles[0]._projectFileType == ".js")
                {
                    item._thumbnailUrl = "~/Content/Logos/javascript.png";
                }
            }
        }


        public List<ProjectViewModel> getAllOwnerProjectsByApplicationUserId(ref List<ProjectViewModel> model, string userName)
        {
            List<Project> newOwnerProjects = new List<Project>();

            IEnumerable<ProjectOwner> ownerProjectsIds = (from projectOwner in _db._projectOwners
                                                          where projectOwner._userName == userName
                                                          select projectOwner);

            
            newOwnerProjects = (from a in _db._projects
                                join b in ownerProjectsIds on a.ID equals b._projectID
                                select a).ToList();

            foreach (var item in newOwnerProjects)
            {

                model.Add(new ProjectViewModel
                {
                    _id = item.ID,
                    _projectName = item._projectName,
                    _projectOwnerName = _userService.getOwnerName(item.ID), //Added this one 10.05.17 - Smári
                    _isProjectOwner = true,
                    _projectFileTypeId = item._projectFileTypeId,
                    _projectFiles = _projectFileService.getAllProjectFilesByProjectId(item.ID),
                    _projectMembers = _userService.getAllProjectMemberViewModelsByProjectId(item.ID)
                });
            }

            return model;
        }

        public List<ProjectViewModel> getAllMemberProjectsByApplicationUserId(ref List<ProjectViewModel> model, string userName)
        {
            List<Project> newOwnerProjects = new List<Project>();

            IEnumerable<ProjectMember> memberProjectsIds = (from projectMember in _db._projectMembers
                                                            where projectMember._userName == userName
                                                            select projectMember);

            newOwnerProjects = (from a in _db._projects
                                join b in memberProjectsIds on a.ID equals b._projectID
                                select a).ToList();

            foreach (var item in newOwnerProjects)
            {
                model.Add(new ProjectViewModel
                {
                    _id = item.ID,
                    _projectName = item._projectName,
                    _isProjectOwner = false,
                    _projectOwnerName = _userService.getOwnerName(item.ID), //Added this one 10.05.17 - Smári
                    _projectFileTypeId = item.ID,
                    _projectFiles = _projectFileService.getAllProjectFilesByProjectId(item.ID),
                    _projectMembers = _userService.getAllProjectMemberViewModelsByProjectId(item.ID)
                });
            }

            return model;
        }
        #endregion

        #region Get single project by id.   
        public ProjectViewModel getProjectByProjectId(string userName, int? id)
        {
            Project entity = _db._projects.Find(id);
            ProjectViewModel model = new ProjectViewModel();

            model._id = entity.ID;
            model._projectName = entity._projectName;
            model._projectFileTypeId = entity._projectFileTypeId;
            model._isProjectOwner = _userService.isProjectOwner(userName, id.Value);
            model._projectMembers = _userService.getAllProjectMemberViewModelsByProjectId(id);

            List<ProjectFileViewModel> modelFiles = new List<ProjectFileViewModel>();
            model._projectFiles = _projectFileService.getAllProjectFilesByProjectId(id);
            return model;
        }
        #endregion

        #region Add project by current user name

        public void addProject(CreateProjectViewModel model, string ownerName)
        {
            Project newProject = new Project();

            newProject._projectName = model._projectName;
            newProject._projectFileTypeId = model._projectTypeId;
            
            _db._projects.Add(newProject);
            _db.SaveChanges();
            
            string extension = _db._fileTypes.Where(x => x.ID == model._projectTypeId).SingleOrDefault()._extension;
            string aceExtension = _db._fileTypes.Where(x => x.ID == model._projectTypeId).SingleOrDefault()._aceExtension;
            string defaultData = _db._fileTypes.Where(x => x.ID == model._projectTypeId).SingleOrDefault()._initialCode;

            ProjectFile defaultFile = new ProjectFile();
            defaultFile._projectFileName = "index" + "." + extension;
            defaultFile._projectFileType = "." + extension;
            defaultFile._aceExtension = aceExtension;
            defaultFile._projectFileData = defaultData;
            defaultFile._projectID = _db._projects.OrderByDescending(p => p.ID)
                                    .Select(p => p.ID).FirstOrDefault();

            _db._projectFiles.Add(defaultFile);
            _db.SaveChanges();

            ProjectOwner owner = new ProjectOwner();
            owner._projectID = defaultFile._projectID;
            owner._userName = ownerName;

            _db._projectOwners.Add(owner);
            _db.SaveChanges();
        }
        #endregion
        
        #region Validation For creating new Project.
        public bool createNewProjectIsValid(string projectName, string userName)
        {
            Project foundProject = new Project();

            List<ProjectOwner> projectsOwnedByUser = (from ownedProject in _db._projectOwners
                                                      where ownedProject._userName == userName
                                                      select ownedProject).ToList();

            List<ProjectViewModel> listOfProjects = new List<ProjectViewModel>();

            foreach(ProjectOwner item in projectsOwnedByUser)
            {
                listOfProjects.Add(getProjectByProjectId(userName, item._projectID));
            }
            
            foreach(ProjectViewModel item in listOfProjects)
            {
                if(projectName == item._projectName)
                {
                    return false;
                }
            }
            
            return true;
        }

        public bool projectExist(int? id)
        {
            var projectExist = _db._projects.Find(id);

            return (projectExist != null);
        }

        #endregion

        #region Delete project.
        public void deleteProject(ProjectViewModel model)
        {
            // Delete all files in the project.
            foreach (ProjectFileViewModel item in model._projectFiles)
            {
                _projectFileService.deleteProjectFile(item._id);
            }

            // Delete all members from the project.
            foreach (ProjectMemberViewModel item in model._projectMembers)
            {
                _userService.deleteProjectMember(item._projectID);
            }

            // Delete the owner of the project.
            ProjectOwner ownerInProject = (from owner in _db._projectOwners
                                           where owner._projectID == model._id
                                           select owner).FirstOrDefault();

            // Delete the project.
            Project projectToDel = _db._projects.Find(model._id);
            _db._projects.Remove(projectToDel);

            _db._projectOwners.Remove(ownerInProject);
            _db.SaveChanges();
        }
        #endregion
    }
}




