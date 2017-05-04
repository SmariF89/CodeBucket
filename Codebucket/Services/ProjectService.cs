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

        public ProjectService()
        {
            _db = new ApplicationDbContext();
        }

        public List<ProjectViewModel> getAllProjectsByApplicationUserId(int? id)
        {
            return null;
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
    }
}