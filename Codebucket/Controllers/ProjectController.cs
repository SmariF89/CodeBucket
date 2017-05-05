using Codebucket.Models;
using Codebucket.Models.ViewModels;
using Codebucket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectService _projectService = new ProjectService();
        private ApplicationUserService _applicationUserService = new ApplicationUserService();

        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        //GET: CreateNewProject
        [HttpGet]
        public ActionResult createNewProject()
        {
            CreateProjectViewModel model = new CreateProjectViewModel();
            model.projectType = _projectService.populateDropdownData();
            return View(model);
        }

        // POST: CreateNewProject
        [HttpPost]
        public ActionResult createNewProject(FormCollection collection)
        {
            string ownerName = System.Web.HttpContext.Current.User.Identity.Name;
            CreateProjectViewModel model = new CreateProjectViewModel();

            model._projectName = collection["_projectName"];
            model.projectTypeId = Int32.Parse(collection["radioChoice"]);
            _projectService.addProject(model, ownerName);

            return RedirectToAction("Index", "Home");
        }


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

        // GET: AddProjectMember
        [HttpGet]
        public ActionResult addProjectMember()
        {
            AddMemberViewModel model = new AddMemberViewModel();
            return View(model);
        }

        // POST: AddProjectMember
        [HttpPost]
        public ActionResult addProjectMember(AddMemberViewModel model)
        {
            if(ModelState.IsValid)
            {
                _projectService.addProjectMember(model);
                return RedirectToAction("Index", "Home");
            }
            return HttpNotFound();
        }
    }
}