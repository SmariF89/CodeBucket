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
        // Make a new instance of database connection.
        public ProjectFileService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
        }
        #endregion

        #region Get project file and get all project files.
        // Get project file by project id, returns a viewmodel of the type ProjectFileViewModel
        // We had to change our queries from using the Find function to using LINQ queries in order
        // for the unit tests to work out.
        public ProjectFileViewModel getProjectFileByProjectFileId(int projectFileId)
        {
            if (projectFileId > 0)
            {
                ProjectFileViewModel model = new ProjectFileViewModel();

                //model._id = _db._projectFiles.Find(projectFileId).ID;
                model._id = (from f in _db._projectFiles
                             where f.ID == projectFileId
                             select f.ID).SingleOrDefault();
                //model._projectFileData = _db._projectFiles.Find(projectFileId)._projectFileData.ToString();
                model._projectFileData = (from f in _db._projectFiles
                                          where f.ID == projectFileId
                                          select f._projectFileData).SingleOrDefault().ToString();
                //model._projectFileName = _db._projectFiles.Find(projectFileId)._projectFileName.ToString();
                model._projectFileName = (from f in _db._projectFiles
                                          where f.ID == projectFileId
                                          select f._projectFileName).SingleOrDefault().ToString();
                //model._projectFileType = _db._projectFiles.Find(projectFileId)._projectFileType.ToString();
                model._projectFileType = (from f in _db._projectFiles
                                          where f.ID == projectFileId
                                          select f._projectFileType).SingleOrDefault().ToString();
                //model._projectID = _db._projectFiles.Find(projectFileId)._projectID;
                model._projectID = (from f in _db._projectFiles
                                    where f.ID == projectFileId
                                    select f._projectID).SingleOrDefault();
                //model._aceExtension = _db._projectFiles.Find(projectFileId)._aceExtension.ToString();
                model._aceExtension = (from f in _db._projectFiles
                                       where f.ID == projectFileId
                                       select f._aceExtension).SingleOrDefault().ToString();

                return model;
            }
            return null;
        }

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
        #endregion

        #region Get file type.
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

        #region Add project file.
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
        // Delete a file in project by file id, void returns no value.
        // Find() was changed to a LINQ query for the unit tests to work.
        public void deleteProjectFile(int? id)
        {
            //ProjectFile fileToDel = _db._projectFiles.Find(id.Value);
            ProjectFile fileToDel = (from f in _db._projectFiles
                                     where f.ID == id.Value
                                     select f).FirstOrDefault();
            _db._projectFiles.Remove(fileToDel);
            _db.SaveChanges();
        }
        #endregion

        #region Update file.
        // Update a file by file id, takes a parameter of a type ProjectFileViewModel.
        // Find() was changed to a LINQ query for the unit tests to work.
        public void updateProjectFile(ProjectFileViewModel file)
        {
            if (file._id != 0)
            {
                //_db._projectFiles.Find(file._id)._projectFileData = file._projectFileData;
                (from f in _db._projectFiles
                 where f.ID == file._id
                 select f).SingleOrDefault()._projectFileData = file._projectFileData;
                _db.SaveChanges();
            }
        }
        #endregion

        #region File exists.
        // Check if file exist by file id, returns bool value if true or not.
        // Find() was changed to a LINQ query for the unit tests to work.
        public bool doesProjectFileExist(int id)
        {
            //var doesProjectfileExist = _db._projectFiles.Find(id);
            var doesProjectFileExist = (from f in _db._projectFiles
                                        where f.ID == id
                                        select f).FirstOrDefault();

            return (doesProjectFileExist != null);
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

        #region Populate drop down data.
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