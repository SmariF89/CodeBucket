using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public List<ProjectViewModel> getAllProjectsByApplicationUserId(string userName)
        {
            //var projectIds = (from i in _db._projectMembers
            //              where (i._userName == userName)
            //              select i._projectID);

            //List<Project> projects = new List<Project>();
            //foreach (int i in projectIds)
            //{
            //    projects = (from j in _db._projects
            //                where j.ID == projectIds.ElementAt(i)
            //                select j).ToList();
            //}

            //List<ProjectViewModel> projectViewModels = new List<ProjectViewModel>();
            //foreach (Project i in projects)
            //{
            //    projectViewModels.Add(new ProjectViewModel
            //    {
            //        _projectName = i._projectName,
            //        _projectFiles = _fileService.getProjectFilesByProjectID,
            //        _projectMembers = _userService.getProjectMembersByUserID
            //    });
            //}

            //return projectViewModels;
        }

        public ProjectViewModel getProjectById(int? id)
        {
            return null;
        }

        public void addProject(ProjectViewModel model)
        {
            Project newProject = new Project();

            newProject._projectName = model._projectName;

            _db._projects.Add(newProject);
            _db.SaveChanges();
        }

        public void updateProject(ProjectViewModel model)
        {

        }
    }
}