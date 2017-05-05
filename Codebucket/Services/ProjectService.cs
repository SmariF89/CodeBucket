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

        public ProjectService()
        {
            _db = new ApplicationDbContext();
        }

        //TODO: Check if this works after implementing more important stuff.
        public List<ProjectViewModel> getAllProjectsByApplicationUserId(ApplicationUser user)
        {
            List<ProjectViewModel> ownedProjectViewModel = new List<ProjectViewModel>();
            var ownedProjects = _db._projectOwners.ToList();

            //item._projectID;

            foreach (var item in ownedProjects)
            {
                ownedProjectViewModel.Add(new ProjectViewModel
                {
                    //_project = item
                    //,
                    _projectName = (from j in _db._projects
                                    where j.ID == item._projectID
                                    select j._projectName).FirstOrDefault()
                }
                );
            }
            return ownedProjectViewModel;

        }

        public ProjectViewModel getProjectById(int? id)
        {
            return null;
        }

        public void addProject(ProjectViewModel model, string ownerName)
        {
            Project newProject = new Project();

            newProject._projectName = model._projectName;
            
            _db._projects.Add(newProject);
            _db.SaveChanges();

            string extension = _db._fileTypes.Where(x => x.ID == model.projectTypeId).SingleOrDefault()._extension;

            ProjectFile defaultFile = new ProjectFile();
            defaultFile._projectFileName = "index" + "." + extension;
            defaultFile._projectFileType = "." + extension;
            defaultFile._projectFileData = "<p>Hello world!</p>";
            defaultFile._projectID = _db._projects.Where(x => x._projectName == model._projectName).SingleOrDefault().ID;

            _db._projectFiles.Add(defaultFile);
            _db.SaveChanges();

            ProjectOwner owner = new ProjectOwner();
            owner._projectID = defaultFile._projectID;
            owner._userName = ownerName;

            _db._projectOwners.Add(owner);
            _db.SaveChanges();
        }

        public void updateProject(ProjectViewModel model)
        {
           
        }

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

        public List<SelectListItem> populateDropdownData()
        {
            List<SelectListItem> fileTypes = new List<SelectListItem>();
            fileTypes.Add(new SelectListItem() { Value = "", Text = "- Choose a file type -" });
            _db._fileTypes.ToList().ForEach((x) => { fileTypes.Add(new SelectListItem() { Value = x.ID.ToString(), Text = x._description }); });

            return fileTypes;
        }
    }
}




