using Codebucket.Models;
using Codebucket.Models.Entities;
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

        public void addProjectFile(ProjectFileViewModel model)
        {
            ProjectFile newProjectFile = new ProjectFile();

            newProjectFile._projectFileName = model._projectFileName;
            newProjectFile._projectFileData = model._projectFileData;
            newProjectFile._projectFileType = model._projectFileType;
            newProjectFile._projectID = 1;

            _db._projectFiles.Add(newProjectFile);
            _db.SaveChanges();
        }

        public void updateProjectFile(string projectFileData)
        {
            
        }
    }
}