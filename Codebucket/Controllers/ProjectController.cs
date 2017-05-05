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

        // GET: getAllProjectsByApplicationUserId
        [HttpGet]
        public ActionResult listAllProjects()
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = User.Identity.Name
            };

            ProjectViewModel model = new ProjectViewModel();




            return View(_projectService.getAllProjectsByApplicationUserId(user));
        }

        // POST: getAllProjectsByApplicationUserId
        //[HttpPost]
        //public ActionResult listAllProjects(ProjectViewModel model)
        //{
            

            

        //    return View();
        //}


        // GET: CreateNewProject
        [HttpGet]
        public ActionResult createNewProject()
        {
            ProjectViewModel model = new ProjectViewModel();
            model.projectType = _projectService.populateDropdownData();
            return View(model);
        }

        // POST: CreateNewProject
        [HttpPost]
        public ActionResult createNewProject(ProjectViewModel model)
        {
            if(ModelState.IsValid)
            {
                string ownerName = System.Web.HttpContext.Current.User.Identity.Name;
                _projectService.addProject(model, ownerName);
                return RedirectToAction("Index", "Home");
            }
            
            model.projectType = _projectService.populateDropdownData();
            return View(model);
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