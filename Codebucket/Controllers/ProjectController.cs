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
            ApplicationUser user = new ApplicationUser
            {
                UserName = User.Identity.Name
            };

            // Same as in return, remove if it couses no problems.
            //List<ProjectViewModel> model = new List<ProjectViewModel>();
            //model = _projectService.getAllOwnerProjectsByApplicationUserId(user);

            return View(_projectService.getAllProjectsByApplicationUserId(user));
        }

        /*
        // GET: getAllOwnerProjectsByApplicationUserId
        [HttpGet]
        public ActionResult listAllProjects()
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = User.Identity.Name
            };

            // Same as in return, remove if it couses no problems.
            //List<ProjectViewModel> model = new List<ProjectViewModel>();
            //model = _projectService.getAllOwnerProjectsByApplicationUserId(user);

            return View(_projectService.getAllProjectsByApplicationUserId(user));
        }
        */
        public ActionResult displayProjectFiles(int? id)
        {
            return RedirectToAction("listAllProjectFiles", "ProjectFile", new { id });
        }


        //// GET: getAllOwnerProjectsByApplicationUserId
        //[HttpGet]
        //public ActionResult listAllOwnerProjects()
        //{
        //    ApplicationUser user = new ApplicationUser
        //    {
        //        UserName = User.Identity.Name
        //    };

        //    // Same as in return, remove if it couses no problems.
        //    //List<ProjectViewModel> model = new List<ProjectViewModel>();
        //    //model = _projectService.getAllOwnerProjectsByApplicationUserId(user);

        //    return View(_projectService.getAllOwnerProjectsByApplicationUserId(user));
        //}

        //// GET: getAllMemberProjectsByApplicationUserId 
        //[HttpGet]
        //public ActionResult listAllMemberProjects()
        //{
        //    ApplicationUser user = new ApplicationUser
        //    {
        //        UserName = User.Identity.Name
        //    };

        //    // Same as in return, remove if it couses no problems.
        //    //List<ProjectViewModel> model = new List<ProjectViewModel>();
        //    //model = _projectService.getAllMemberProjectsByApplicationUserId(user);

        //    return View(_projectService.getAllMemberProjectsByApplicationUserId(user));
        //}

        // GET: CreateNewProject

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
            model._projectTypeId = Int32.Parse(collection["radioChoice"]);
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


    }
}