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
            return null;
        }

        // POST: CreateNewProject
        [HttpPost]
        public ActionResult createNewProject(ProjectViewModel model)
        {
            return null;
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