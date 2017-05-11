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
        private ProjectService _projectService = new ProjectService(null);
        private ProjectFileService _projectFileService = new ProjectFileService(null);
        private UserService _userService = new UserService(null);

        #region Display all projects.
        // Overview/Index
        [HttpGet]
        public ActionResult Index()
        {
            List<ProjectViewModel> modelList = _projectService.getAllProjectsByApplicationUserId(User.Identity.Name);
            return View(modelList);
        }
  
        [HttpGet]
        // FIXME::Just calling a function in ProjectController. possible to remove ? or at least rename ?
        public ActionResult DisplayProjectRoute(int? id) 
        {
            return RedirectToAction("DisplayProject", "Project", new { id });
        }
        #endregion

        #region Create new project.
        // GET: CreateNewProject
        [HttpGet]
        public ActionResult CreateNewProject()
        {
            CreateProjectViewModel model = new CreateProjectViewModel();
            model._projectType = _projectFileService.populateDropdownData();
            return View(model);
        }

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