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

        public List<ProjectViewModel> getAllProjectsByApplicationUserId(int? id)
        {
            return null;
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


    }
}


// ========== DELETE LATER ========== //

//Project project = _db._projects.Find(model._project);
//project = project.Where(x => x._projectName.Contains(model._project));
//user = user.Where(s => s.UserName.Contains(model._userName));
//newProjectMember._userName = model._userName;

// ========== DELETE LATER ========== //