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
        private ApplicationUserService _applicationUserService = new ApplicationUserService();

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

            return newProjectViewModel;
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
                    _isProjectOwner = true,
                    _projectTypeId = item.ID,
                    _projectFiles = _projectFileService.getAllProjectFilesByProjectId(item.ID),
                    //_projectMembers = _applicationUserService.getAllProjectMembersByProjectId(item.ID)                
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
                    _projectTypeId = item.ID,
                    _projectFiles = _projectFileService.getAllProjectFilesByProjectId(item.ID),
                    //_projectMembers = _applicationUserService.getAllProjectMembersByProjectId(item.ID)
                });
            }

            return model;
        }
        #endregion

        #region Add project by current user name

        public void addProject(CreateProjectViewModel model, string ownerName)
        {
            Project newProject = new Project();

            newProject._projectName = model._projectName;
            
            _db._projects.Add(newProject);
            _db.SaveChanges();

            string extension = _db._fileTypes.Where(x => x.ID == model._projectTypeId).SingleOrDefault()._extension;

            ProjectFile defaultFile = new ProjectFile();
            defaultFile._projectFileName = "index" + "." + extension;
            defaultFile._projectFileType = "." + extension;
            defaultFile._projectFileData = "<p>Hello world!</p>";
            defaultFile._projectID = _db._projects.Where(x => x._projectName == model._projectName).FirstOrDefault().ID;

            _db._projectFiles.Add(defaultFile);
            _db.SaveChanges();

            ProjectOwner owner = new ProjectOwner();
            owner._projectID = defaultFile._projectID;
            owner._userName = ownerName;

            _db._projectOwners.Add(owner);
            _db.SaveChanges();
        }
        #endregion

        #region Add project member, TODO::Throw exeption?

        public void addProjectMember(AddMemberViewModel model)
        {
            ProjectMember newProjectMember = new ProjectMember();

            // Select project from db that corresponds to user selected/entered project
            var project = from p in _db._projects
                          where p._projectName == model._project
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

        #region Populate drop down data

        public List<SelectListItem> populateDropdownData()
        {
            List<SelectListItem> fileTypes = new List<SelectListItem>();
            fileTypes.Add(new SelectListItem() { Value = "", Text = "- Choose a file type -" });
            _db._fileTypes.ToList().ForEach((x) => { fileTypes.Add(new SelectListItem() { Value = x.ID.ToString(), Text = x._description }); });

            return fileTypes;
        }
        #endregion
    }
}




