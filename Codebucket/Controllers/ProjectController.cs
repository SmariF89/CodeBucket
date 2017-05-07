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
            int a = 0;

            a = 10 / a;      

            string userName = User.Identity.Name;

            return View(_projectService.getAllProjectsByApplicationUserId(userName));
        }
        #endregion

        #region Display all files in project selected, redirects action to ProjectFile/listAllProjectFiles
        [HttpGet]
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
            //model._projectType = _projectService.populateDropdownData();
            return View(model);
        }

        // POST: CreateNewProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createNewProject(FormCollection collection)
        {
            if(!ModelState.IsValid)
            {
                var viewModel = new CreateProjectViewModel();
                viewModel._projectName = collection["_projectName"];

                return View("CreateNewProject", viewModel);
            }

            else
            { 
            string ownerName = System.Web.HttpContext.Current.User.Identity.Name;
            CreateProjectViewModel model = new CreateProjectViewModel();

            model._projectName = collection["_projectName"];
            model._projectTypeId = Int32.Parse(collection["radioChoice"]);
            _projectService.addProject(model, ownerName);

            return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region Update project TODO::needed?

        // GET: UpdateProject
        [HttpGet]
        public ActionResult updateProject(int? id)
        {
            return null;
        }

        // POST: UpdateProject
        [HttpPost]

        public ActionResult updateProject(ProjectViewModel model)
        {
            return null;
        }
        #endregion
    }
}