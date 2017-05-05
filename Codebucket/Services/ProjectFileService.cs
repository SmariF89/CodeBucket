using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            List<ProjectFileViewModel> newProjectFileViewModel = new List<ProjectFileViewModel>();

            IEnumerable<ProjectFile> projectFiles = (from projectFile in _db._projectFiles
                                                     where projectFile._projectID == id
                                                     select projectFile);

            foreach (var item in projectFiles)
            {
                newProjectFileViewModel.Add(new ProjectFileViewModel
                {
                    _projectFileName = item._projectFileName,
                    _projectFileType = item._projectFileType,
                    _projectFileData = item._projectFileData
                });
            }


            return newProjectFileViewModel;
        }

        public ProjectFileViewModel getProjectFileById(int? id) 
        {
            if (id.HasValue) 
            {
                ProjectFile newProjectFile = _db._projectFiles.Find(id);

                ProjectFileViewModel viewModel = new ProjectFileViewModel();
                viewModel._projectFileData = newProjectFile._projectFileData.ToString();
                viewModel._projectFileName = newProjectFile._projectFileName.ToString();
                viewModel._projectFileType = newProjectFile._projectFileType.ToString();

                return viewModel;
            }

            return null;
        }


        public void addProjectFile(ProjectFileViewModel model)
        {
            ProjectFile newProjectFile = new ProjectFile();

            newProjectFile._projectFileName = model._projectFileName;
            newProjectFile._projectFileData = model._projectFileData;
            newProjectFile._projectFileType = model._projectFileType;
            newProjectFile._projectID = model.ProjectID;

            _db._projectFiles.Add(newProjectFile);
            _db.SaveChanges();
        }

        public void updateProjectFile(string projectFileData)
        {
            
        }

        public List<Project> getAllProjects()
        {
           
            return _db._projects.ToList();
            
            
        }

        

        
    }
}