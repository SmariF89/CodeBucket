using Codebucket.Models;
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

        }

        public void updateProject(ProjectViewModel model)
        {

        }
    }
}