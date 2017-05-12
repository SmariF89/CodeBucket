using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Codebucket.Services
{   
    public class ProjectFileService
    {
        private readonly IAppDataContext _db;

        #region Constructor
        /// <summary>
        /// Constructor, makes a new instance of database connection. If parameter is null it sets the _db as
        /// 'new ApplicationDbContext' else it takes the 'IAppDataContext context' parameter and uses that 
        /// (used for unit testing).
        /// </summary>
        /// <param name="context"></param>
        public ProjectFileService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
        }
        #endregion

        #region Get project file and get all project files.
        /// <summary>
        /// Get project file by project id, returns a viewmodel of the type 'ProjectFileViewModel'.
        /// LINQ query for each of 'ProjectFileViewModel' properties used.
        /// </summary>
        /// <param name="projectFileId">Project File ID</param>
        /// <returns>'ProjectFileViewModel'</returns>
        public ProjectFileViewModel getProjectFileByProjectFileId(int projectFileId)
        {
            if (projectFileId > 0)
            {
                ProjectFileViewModel model = new ProjectFileViewModel();

                model._id = (from f in _db._projectFiles
                             where f.ID == projectFileId
                             select f.ID).SingleOrDefault();

                model._projectFileData = (from f in _db._projectFiles
                                          where f.ID == projectFileId
                                          select f._projectFileData).SingleOrDefault().ToString();

                model._projectFileName = (from f in _db._projectFiles
                                          where f.ID == projectFileId
                                          select f._projectFileName).SingleOrDefault().ToString();

                model._projectFileType = (from f in _db._projectFiles
                                          where f.ID == projectFileId
                                          select f._projectFileType).SingleOrDefault().ToString();

                model._projectID = (from f in _db._projectFiles
                                    where f.ID == projectFileId
                                    select f._projectID).SingleOrDefault();

                model._aceExtension = (from f in _db._projectFiles
                                       where f.ID == projectFileId
                                       select f._aceExtension).SingleOrDefault().ToString();

                return model;
            }
            return null;
        }

        /// <summary>
        /// Get all project files by project ID, returns a list of 'ProjectFileViewModel'. Get a list of 
        /// all projectFiles and goes through each 'ProjectFile' in the list and adds to the propperties from
        /// to a list of 'ProjectFileViewModel' one at a time, then returns it back. If project ID is not valid
        /// return null.
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>List of 'ProjectFileViewModel'</returns>
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
        #endregion

        #region Get file type.
        /// <summary>
        /// Get file type by project ID, if project ID is not valid return null.
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>String</returns>
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

        /// <summary>
        /// Get ace extension type by project ID from Db. Used to tell the ACE editor what file type it is 
        /// so it can adjust the syntax highlighting accordingly.
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>String</returns>
        public String getAceExtensionByProjectId(int projectId)
        {
            string ext = (from projectFile in _db._projectFiles
                    where projectFile._projectID == projectId
                    select projectFile._aceExtension).FirstOrDefault();
            return ext;
        }
        #endregion

        #region Add project file.
        /// <summary>
        /// Add project file to the Db. 
        /// </summary>
        /// <param name="model">'CreateProjectFileViewModel'</param>
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

        #region Delete file.
        /// <summary>
        /// Deletes a file from a project by file ID.
        /// </summary>
        /// <param name="id"></param>
        public void deleteProjectFile(int? id)
        {
            ProjectFile fileToDel = (from f in _db._projectFiles
                                     where f.ID == id.Value
                                     select f).FirstOrDefault();
            _db._projectFiles.Remove(fileToDel);
            _db.SaveChanges();
        }
        #endregion

        #region Update file.
        /// <summary>
        /// Update file by file ID in the Db.
        /// </summary>
        /// <param name="file">'ProjectFileViewModel'</param>
        public void updateProjectFile(ProjectFileViewModel file)
        {
            if (file._id != 0)
            {
                (from f in _db._projectFiles
                 where f.ID == file._id
                 select f).SingleOrDefault()._projectFileData = file._projectFileData;
                _db.SaveChanges();
            }
        }
        #endregion

        #region File exists.
        /// <summary>
        /// Check if file exist by file ID, returns bool value if true or not.
        /// </summary>
        /// <param name="id">File ID</param>
        /// <returns>bool</returns>
        public bool doesProjectFileExist(int id)
        {
            var doesProjectFileExist = (from f in _db._projectFiles
                                        where f.ID == id
                                        select f).FirstOrDefault();

            return (doesProjectFileExist != null);
        }
        #endregion

        #region Validation for creating new file.
        /// <summary>
        /// Check if project file exists, adds the filetype ending to the name of the project file so it can match 
        /// correctly in the Db. Returns a bool value if true or not.
        /// </summary>
        /// <param name="projectFileName">Project File Name</param>
        /// <param name="projectID">Project ID</param>
        /// <returns>bool</returns>
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

        #region Populate drop down data.
        /// <summary>
        /// Gets all file types so it can be populated in a drop down menu to select a filetype.
        /// </summary>
        /// <returns>List of 'SelectListItem'</returns>
        public List<SelectListItem> populateDropdownData()
        {
            List<SelectListItem> fileTypes = new List<SelectListItem>();
            fileTypes.Add(new SelectListItem() { Value = "", Text = "- Choose a file type -" });
            _db._fileTypes.ToList().ForEach((x) => { fileTypes.Add(new SelectListItem() { Value = x.ID.ToString(), Text = x._description }); });

            return fileTypes;
        }
        #endregion
    }
}