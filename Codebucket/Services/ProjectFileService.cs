using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codebucket.Services
{
    public class ProjectFileService
    {
        private ApplicationDbContext _db;

        #region Constructor
        // Make a new instance of database connection.
        public ProjectFileService()
        {
            _db = new ApplicationDbContext();
        }
        #endregion

        #region Get project file and get all project files
        // Get all project files by project id, returns a list of the type ProjectFileViewModel.
        public List<ProjectFileViewModel> getAllProjectFilesByProjectId(int? projectId)
        {
            if (projectId != null || projectId > 0)
            {
                List<ProjectFileViewModel> projectFileViewModels = new List<ProjectFileViewModel>();

                List<ProjectFile> projectFiles = (from projectFile in _db._projectFiles
                                                  where projectFile._projectID == projectId
                                                  select projectFile).ToList();

                foreach (var item in projectFiles)
                {
                    projectFileViewModels.Add(new ProjectFileViewModel
                    {
                        _id = item.ID,
                        _projectID = item._projectID,
                        _projectFileName = item._projectFileName,
                        _projectFileType = item._projectFileType,
                        _projectFileData = item._projectFileData
                    });
                }

                return projectFileViewModels;
            }

            return null;
        }

        // Get project file by project id, returns a viewmodel of the type ProjectFileViewModel
        public ProjectFileViewModel getProjectFileByProjectId(int? projectId)
        {
            if (projectId != null || projectId > 0)
            {
                return new ProjectFileViewModel
                {
                    _id = _db._projectFiles.Find(projectId).ID,
                    _projectFileData = _db._projectFiles.Find(projectId)._projectFileData.ToString(),
                    _projectFileName = _db._projectFiles.Find(projectId)._projectFileName.ToString(),
                    _projectFileType = _db._projectFiles.Find(projectId)._projectFileType.ToString()
                };
            }

            return null;
        }
        #endregion

        #region Get file type
        // Get file type by project id, returns a string.
        public String getFileTypeByProjectId(int? projectId)
        {
            if (projectId != null || projectId > 0)
            {
                string fileType = (from projectFile in _db._projectFiles
                                   where projectFile._projectID == projectId
                                   select projectFile._projectFileType).FirstOrDefault();

                return fileType.Substring(fileType.LastIndexOf('.') + 1);
            }
            return null;
        }
        #endregion

        #region Add project file
        public void addProjectFile(CreateProjectFileViewModel model) // TODO:: Add a if check ?
        {
            ProjectFile newProjectFile = new ProjectFile()
            {
                _projectFileName = model._projectFileName + "." + model._projectFileType,
                _projectFileData = model._projectFileData,
                _projectFileType = "." + model._projectFileType,
                _projectID = model._projectID
            };

            _db._projectFiles.Add(newProjectFile);
            _db.SaveChanges();
        }
        #endregion

        #region Check if owner or username exists
        // Checks if username exists in the database, returns a bool value if true or not.
        public bool usernameExists(string username)
        {
            if (username != null)
            {
                return (from user in _db.Users
                        where user.UserName == username
                        select user).Any();
            }
            else
            {
                throw new Exception();
            }
            
        }

        // Checks if username is owner of project, returns a bool value if true or not.
        public bool isProjectOwner(string username, int projectID) // FIXME:: does this work?
        {
            ProjectOwner own = (from owned in _db._projectOwners
                                where owned._projectID == projectID
                                select owned).FirstOrDefault();

            return (own != null);
        }
        #endregion

        #region Update a file
        // Update a file by file id, takes a parameter of a type ProjectFileViewModel.
        public void updateProjectFile(ProjectFileViewModel file)
        {
            if (file._id != 0)
            {
                _db._projectFiles.Find(file._id)._projectFileData = file._projectFileData;
                _db.SaveChanges();
            }
        }
        #endregion
    }
}