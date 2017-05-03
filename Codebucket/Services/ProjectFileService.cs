using Codebucket.Models;
using Codebucket.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebucket.Services
{
    public class ProjectFileService
    {
        private ApplicationDbContext _db;

        public ProjectFileService()
        {
            _db = new ApplicationDbContext();
        }

        public List<ProjectFileViewModel> getAllProjectFilesByProjectId(int? id)
        {
            return null;
        }
        
        public ProjectFileViewModel getProjectFileById(int? id)
        {
            return null;
        }

        public void addProjectFile(string projectFileData)
        {

        }

        public void updateProjectFile(string projectFileData)
        {
            
        }
    }
}