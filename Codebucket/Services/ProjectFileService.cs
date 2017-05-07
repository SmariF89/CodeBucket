using Codebucket.Models;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

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
					_id = item.ID,
					_projectID = item._projectID,
                    _projectFileName = item._projectFileName,
                    _projectFileType = item._projectFileType,
                    _projectFileData = item._projectFileData
                });
            }

            return newProjectFileViewModel;
        }

        public bool usernameExists(string username)
        {
            bool userExists = (from user in _db.Users
                                 where user.UserName == username
                                 select user).Any();

            return userExists; 
        }

        public bool isProjectOwner(string username, int projectID)
        {

            ProjectOwner own = new ProjectOwner();
            own = (from owned in _db._projectOwners
                   where owned._projectID == projectID select owned).SingleOrDefault();

            if(own._userName == username)
            {
                return true;
            }

            else
            {
                return false;
            }


        }

        public ProjectFileViewModel getProjectFileById(int? id) 
        {
            if (id.HasValue) 
            {
                ProjectFile newProjectFile = _db._projectFiles.Find(id);

                ProjectFileViewModel viewModel = new ProjectFileViewModel();
				viewModel._id = newProjectFile.ID;
                viewModel._projectFileData = newProjectFile._projectFileData.ToString();
                viewModel._projectFileName = newProjectFile._projectFileName.ToString();
                viewModel._projectFileType = newProjectFile._projectFileType.ToString();

                return viewModel;
            }

            return null;
        }

        public String getFileTypeByProjectId(int projectId)
        {
            string fileType = (from projectFile in _db._projectFiles
                               where projectFile._projectID == projectId
                               select projectFile._projectFileType).FirstOrDefault();

            return fileType.Substring(fileType.LastIndexOf('.') + 1);
        }


        public void addProjectFile(CreateProjectFileViewModel model)
        {
            ProjectFile newProjectFile = new ProjectFile();
            newProjectFile._projectFileName = model._projectFileName + "." + model._projectFileType;
            newProjectFile._projectFileData = model._projectFileData;
            newProjectFile._projectFileType = "." + model._projectFileType;
            newProjectFile._projectID = model._projectID;

            _db._projectFiles.Add(newProjectFile);
            _db.SaveChanges();
        }

		public void updateProjectFile(ProjectFileViewModel file)
		{
			if (file._id != 0)
			{
				//ProjectFile projectFiles = (from projectFile in _db._projectFiles
				//							where projectFile.ID == file._id
				//							select projectFile).FirstOrDefault();

				//projectFiles._projectFileData = file._projectFileData;
				//_db._projectFiles.Add(projectFiles);
				_db._projectFiles.Find(file._id)._projectFileData = file._projectFileData;
				_db.SaveChanges();
			}
		}

		public List<Project> getAllProjects()
        {
            //System.Web.HttpContext.Current.User.Identity.Name;


            //var db = from f in _db._projects where f.ID == 
            /*var ownedProjects = (from p in _db._projects
                                 join f in _db._projectOwners on p.ID equals f._projectID
                   
            select p).ToList();*/

            //System.Web.HttpContext.Current.User.Identity.Name

            ProjectOwner currentMember = new ProjectOwner();


            currentMember = (from c in _db._projectOwners
                                     where c._userName == System.Web.HttpContext.Current.User.Identity.Name
                                     select c).FirstOrDefault();
            //int id = currentMember.ID;

            if(currentMember == null)
            {
                return null;
            }


            /* var ownedProjects = (from c in _db._projects
                                  from p in _db._projectOwners where p.ID == id &&     //c.ID == p._projectID && p._userName == currentMember._userName
                                  select c).ToList();*/

            else
            { 

            var owned = (from project in _db._projects
                         join o in _db._projectOwners
                         on project.ID equals o._projectID
                         where o.ID.Equals(currentMember.ID)
                         select project).ToList();
                return owned;


            }
            //return _db._projects.ToList();
            // return ownedProjects; 
        }
	}
}