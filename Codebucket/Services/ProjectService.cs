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

            Project project = _db._projects.Find(model._project);
            var user = _db.Users.Find(model._userName);

            if(project != null && user != null)
            {
                newProjectMember._projectID = project.ID;
                newProjectMember._userName = user.UserName;

                _db._projectMembers.Add(newProjectMember);
                _db.SaveChanges();
            }
                

        }


    }
}