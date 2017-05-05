using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebucket.Services
{
    public class ProjectService
    {
        private ApplicationDbContext _db;
        private ProjectFileService _projectFileService = new ProjectFileService();
        private ApplicationUserService _applicationUserService = new ApplicationUserService();

        public ProjectService()
        {
            _db = new ApplicationDbContext();
        }

        public List<ProjectViewModel> getAllOwnerProjectsByApplicationUserId(ApplicationUser user)
        {
            List<ProjectViewModel> newOwnerProjectViewModel = new List<ProjectViewModel>();
            List<Project> newOwnerProjects = new List<Project>();

            IEnumerable<ProjectOwner> ownerProjectsIds = (from projectOwner in _db._projectOwners
                                                   where projectOwner._userName == user.UserName
                                                   select projectOwner);

            foreach (var item in ownerProjectsIds)
            {
                //newOwnerProjects.Add(getProjectEntityById(item._projectID));
                newOwnerProjects.Add(getProjectEntityById(1));

            }

            foreach (var item in newOwnerProjects)
            {
                newOwnerProjectViewModel.Add(new ProjectViewModel
                {
                    //_projectName = "test",
                    _projectName = item._projectName,
                    _project = item,
                    _projectFiles = _projectFileService.getAllProjectFilesByProjectId(item.ID),
                    _projectMembers = _applicationUserService.getAllProjectMembersByProjectId(item.ID)
                });
            }

            return newOwnerProjectViewModel;

            //List<ProjectViewModel> ownedProjectViewModel = new List<ProjectViewModel>();
            //var ownedProjects = _db._projectOwners.ToList();

            ////item._projectID;

            //foreach (var item in ownedProjects)
            //{
            //    ownedProjectViewModel.Add(new ProjectViewModel
            //    {
            //        //_project = item
            //        //,
            //        _projectName = (from j in _db._projects
            //                        where j.ID == item._projectID
            //                        select j._projectName).FirstOrDefault()
            //    }
            //    );
            //}
            //return ownedProjectViewModel;

            //--------------------------------------

        }

       

        public List<ProjectViewModel> getAllMemberProjectsByApplicationUserId(ApplicationUser user)
        {
            List<ProjectViewModel> newMemberProjectViewModel = new List<ProjectViewModel>();
            return newMemberProjectViewModel;
        }

        public ProjectViewModel getProjectById(int? id)
        {

            return null;
        }

        public void addProject(ProjectViewModel model, ApplicationUser user)
        {
            Project newProject = new Project();

            newProject._projectName = model._projectName;
            

            _db._projects.Add(newProject);
            _db.SaveChanges();

            ProjectOwner owner = new ProjectOwner();
            owner._projectID = newProject.ID;
            owner._userName = user.UserName;
            
            //owner._projectID = 2;
            //owner._userName = "Snorri";

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

        private Project getProjectEntityById(int? id)
        {
            Project newProject = new Project();

            newProject = (from project in _db._projects
                          select project).FirstOrDefault();

            //newProject = _db._projects.Find(id);

            return newProject;
        }

        private List<ApplicationUserViewModel> getApplicationUserViewModel() // Needed ?
        {
            List <ApplicationUserViewModel> newApplicationUserViewModel = new List<ApplicationUserViewModel>();

            return newApplicationUserViewModel;
        }

    }
}




