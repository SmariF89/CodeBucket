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

        // GET: CreateNewProject
        [HttpGet]
        public ActionResult createNewProject()
        {
            ProjectViewModel model = new ProjectViewModel();
            return View(model);
        }

        // POST: CreateNewProject
        [HttpPost]
        public ActionResult createNewProject(ProjectViewModel model)
        {
            _projectService.addProject(model);
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

        [HttpGet]
        public ActionResult addProjectMember()
        {
            AddMemberViewModel model = new AddMemberViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult addProjectMember(AddMemberViewModel model) // IN PROGRESS - THORIR
        {
            _projectService.addProjectMember(model); 
            return RedirectToAction("Index", "Home");
        }
    }
}