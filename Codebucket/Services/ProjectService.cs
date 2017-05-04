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

        public void addProjectMember(AddMemberViewModel model) // IN WORK
        {
            //ProjectMember newProjectMember = new ProjectMember();

            


            //newProjectMember._projectID = model
            //newProjectMember._userName = newProject.


            //_db._projectMembers.Add(newProjectMember);
            //_db.SaveChanges();

        }


    }
}