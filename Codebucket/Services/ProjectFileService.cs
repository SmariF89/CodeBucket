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
                        _aceExtension = item._aceExtension,
                        _projectFileData = item._projectFileData
                    });
                }
                return projectFileViewModels;
            }

            return null;
        }

        // Get project file by project id, returns a viewmodel of the type ProjectFileViewModel
        public ProjectFileViewModel getProjectFileByProjectFileId(int projectFileId)
        {
            if (projectFileId > 0)
            {
                ProjectFileViewModel model = new ProjectFileViewModel();
                
                model._id = _db._projectFiles.Find(projectFileId).ID;
                model._projectFileData = _db._projectFiles.Find(projectFileId)._projectFileData.ToString();
                model._projectFileName = _db._projectFiles.Find(projectFileId)._projectFileName.ToString();
                model._projectFileType = _db._projectFiles.Find(projectFileId)._projectFileType.ToString();
                model._projectID = _db._projectFiles.Find(projectFileId)._projectID;
                model._aceExtension = _db._projectFiles.Find(projectFileId)._aceExtension.ToString();

                return model;
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

        public String getAceExtensionByProjectId(int projectId)
        {
            string ext = (from projectFile in _db._projectFiles
                    where projectFile._projectID == projectId
                    select projectFile._aceExtension).FirstOrDefault();
            return ext;
        }
        #endregion

        #region Add project file
        public void addProjectFile(CreateProjectFileViewModel model)
        {
            ProjectFile newProjectFile = new ProjectFile();
            newProjectFile._projectFileName = model._projectFileName + "." + model._projectFileType;
            newProjectFile._projectFileData = model._projectFileData;
            newProjectFile._projectFileType = "." + model._projectFileType;
            newProjectFile._aceExtension = getAceExtensionByProjectId(model._projectID);
            newProjectFile._projectID = model._projectID;

            _db._projectFiles.Add(newProjectFile);
            _db.SaveChanges();
        }
        #endregion

        #region Check if owner or username exists
        // Checks if username exists in the database, returns a bool value if true or not.
        public bool userIsInDataBase(string username)
        {
            string getUserName = (from user in _db.Users
                                  where user.UserName == username
                                  select user.UserName).FirstOrDefault();

            return (getUserName == username);           
        }
        
        public bool isProjectOwnerOrMember(string username, int projectID)
        {
            return (isProjectOwner(username, projectID) || isProjectMember(username, projectID));
        }

        // Checks if username is owner of project, returns a bool value if true or not.
        public bool isProjectOwner(string username, int projectID) 
        {
            ProjectOwner ownerInProject = (from owned in _db._projectOwners
                                           where owned._userName == username && owned._projectID == projectID
                                           select owned).FirstOrDefault();

            return (ownerInProject != null);
        }

        // Checks if username is owner of project, returns a bool value if true or not.
        public bool isProjectMember(string username, int projectID) 
        {
            ProjectMember memberInProject = (from member in _db._projectMembers
                                             where member._userName == username && member._projectID == projectID
                                             select member).FirstOrDefault();

            return (memberInProject != null);
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

        #region Validation for creating new file.
        public bool projectFileExists(string projectFileName, int projectID)
        {
            List<ProjectFileViewModel> projectFiles = getAllProjectFilesByProjectId(projectID);

            string fileEnding = projectFiles[0]._projectFileType;
            projectFileName = projectFileName + fileEnding;

            foreach(ProjectFileViewModel item in projectFiles)
            {
                if(projectFileName == item._projectFileName)
                {
                    return true; 
                }
            }

            return false;
        }
        #endregion


        public string getOwnerName (int projectID)
        {
            if (_db._projects.Find(projectID) == null)
            {
                return null;
            }

            ProjectOwner ownerInProject = (from owned in _db._projectOwners
                                           where owned._projectID == projectID
                                           select owned).FirstOrDefault();

            string owner = ownerInProject._userName;

            return owner;
        }

        public void deleteProjectFile(int? id)
        {
            ProjectFile fileToDel = _db._projectFiles.Find(id.Value);
            _db._projectFiles.Remove(fileToDel);
            _db.SaveChanges();
        }

        public void deleteProjectMember(int projectID)
        {
            ProjectMember memberToDel = (from member in _db._projectMembers
                                         where member._projectID == projectID
                                         select member).FirstOrDefault();

            _db._projectMembers.Remove(memberToDel);
            _db.SaveChanges();
        }

        public bool doesProjectFileExist(int id)
        {
            var doesProjectfileExist = _db._projectFiles.Find(id);

            return (doesProjectfileExist != null);
        }
    }
}