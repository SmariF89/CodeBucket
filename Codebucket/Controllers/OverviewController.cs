using Codebucket.Models;
using Codebucket.Models.ViewModels;
using Codebucket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Codebucket.Utilities;
using Codebucket.Handlers;

namespace Codebucket.Controllers    
{
    public class OverviewController : Controller
    {
        /// <summary>
        /// Initializes new instances of services, used to call on service functions and get data or remove from Db.
        /// </summary>
        private ProjectService _projectService = new ProjectService(null);
        private ProjectFileService _projectFileService = new ProjectFileService(null);
        private UserService _userService = new UserService(null);

        #region Display all projects.
        /// <summary>
        /// Displays all project by current user.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Index()
        {
            List<ProjectViewModel> modelList = _projectService.getAllProjectsByApplicationUserId(User.Identity.Name);
            return View(modelList);
        }

        /// <summary>
        /// Redirects action to display a project in project controller.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult DisplayProjectRoute(int? id) 
        {
            return RedirectToAction("DisplayProject", "Project", new { id });
        }
        #endregion

        #region Create new project.
        /// <summary>
        /// CreateProjectViewModel for user to ba able to create a new project.
        /// </summary>
        /// <returns>ActionResult</returns>
        // GET: CreateNewProject
        [HttpGet]
        public ActionResult CreateNewProject()
        {
            CreateProjectViewModel model = new CreateProjectViewModel();
            model._projectType = _projectFileService.populateDropdownData();
            return View(model);
        }
        /// <summary>
        /// FormCollection sent from user used is checked if valid, if it is not valid send back to
        /// user with message, else if valid send forward to service layer and add project to Db and redirect
        /// action to overview/index.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewProject(FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                CreateProjectViewModel viewModel = new CreateProjectViewModel();
                viewModel._projectName = collection["_projectName"];
                return View("CreateNewProject", viewModel);
            }
            else
            {
                CreateProjectViewModel model = new CreateProjectViewModel();
                model._projectName = collection["_projectName"];
                model._projectTypeId = Int32.Parse(collection["radioChoice"]);

                if (_projectService.createNewProjectIsValid(model._projectName, User.Identity.Name))
                {
                    _projectService.addProject(model, User.Identity.Name);
                    return RedirectToAction("Index", "Overview");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This project already exists.");
                    return View("CreateNewProject");
                }
            }
        }
        #endregion

        #region Delete project.
        /// <summary>
        /// User has selected a project to delete and that project ID is passed in as a parameter. If project
        /// is valid the user is asked to confirm delete. If yes redirect to DeleteConfirmed, else go back to 
        /// overview/index.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult DeleteProject(int? id)
        {
            if (id != null)
            {
                if (_projectService.projectExist(id.Value))
                {
                    if (_userService.isProjectOwner(User.Identity.Name, id.Value))
                    {
                        ProjectViewModel model = new ProjectViewModel();
                        model = _projectService.getProjectByProjectId(User.Identity.Name, id);
                        return View(model);
                    }
                }
            }
            return HttpNotFound();
        }

        /// <summary>
        /// User has confirmed delete. If project ID is valid send forward to service layer and delete from Db, 
        /// else return HttpNotFound.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>ActionResult</returns>
        [HttpPost, ActionName("DeleteProject")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id != null)
            {
                if (_projectService.projectExist(id.Value))
                {
                    if (_userService.isProjectOwner(User.Identity.Name, id.Value))
                    {
                        ProjectViewModel model = new ProjectViewModel();
                        model = _projectService.getProjectByProjectId(User.Identity.Name, id.Value);
                        _projectService.deleteProject(model);
                        return RedirectToAction("Index");
                    }
                }
            }
            return HttpNotFound();
        }
        #endregion
    }
}