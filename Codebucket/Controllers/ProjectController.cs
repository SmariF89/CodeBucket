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
    public class ProjectController : Controller
    {
        private ProjectService _projectService = new ProjectService();

        #region Display all projects by current user

        // GET: Project
        [HttpGet]
        public ActionResult Index()
        {
            List<ProjectViewModel> modelList = _projectService.getAllProjectsByApplicationUserId(User.Identity.Name);

            return View(modelList);
        }
        #endregion

        #region Display all files in project selected, redirects action to ProjectFile/listAllProjectFiles
        [HttpGet]
        // FIXME::Just calling a function in ProjectFileController. possible to remove ? or at least rename ?
        public ActionResult displayProjectFiles(int? id) 
        {
            return RedirectToAction("displayProject", "ProjectFile", new { id });
        }
        #endregion

        #region Create new project for current user

        // GET: CreateNewProject
        [HttpGet]
        public ActionResult createNewProject()
        {
            CreateProjectViewModel model = new CreateProjectViewModel();
            model._projectType = _projectService.populateDropdownData();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createNewProject(FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                CreateProjectViewModel viewModel = new CreateProjectViewModel();
                viewModel._projectName = collection["_projectName"];
                return View("createNewProject", viewModel);
            }
            else
            {
                CreateProjectViewModel model = new CreateProjectViewModel();

                model._projectName = collection["_projectName"];
                model._projectTypeId = Int32.Parse(collection["radioChoice"]);

                if (_projectService.createNewProjectIsValid(model._projectName, User.Identity.Name))
                {
                    _projectService.addProject(model, User.Identity.Name);
                    return RedirectToAction("Index", "Project");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This project already exists.");
                    return View("createNewProject");
                }
            }
        }

        #endregion

        [HttpGet]
        public ActionResult deleteProject(int? id)
        {
            if (id != null)
            {
                string userName = User.Identity.Name;
                ProjectViewModel model = new ProjectViewModel();
                // Skrítið að það þurfi að ná í project með username og id. Þá þarf 
                // maður að declera userinn óþarflega oft. Þarf að laga?
                model = _projectService.getProjectByProjectId(userName, id);

                return View(model);
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("deleteProject")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            // Skrítið að það þurfi að ná í project með username og id. Þá þarf 
            // maður að declera userinn óþarflega oft. Þarf að laga?
            ProjectViewModel model = new ProjectViewModel();
            model = _projectService.getProjectByProjectId(userName, id);
            int idOfProject = model._id;

            _projectService.deleteProject(model);

            return RedirectToAction("Index");
        }
    }
}