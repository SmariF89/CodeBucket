﻿using Codebucket.Models.ViewModels;
using Codebucket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Controllers
{
    public class ProjectFileController : Controller
    {
        private ProjectFileService _projectFileService = new ProjectFileService();

        // GET: ProjectFile
        public ActionResult Index()
        {
            return View();
        }

        // GET: createNewProjectFile
        [HttpGet]
        public ActionResult createNewProjectFile()
        {
            ProjectFileViewModel model = new ProjectFileViewModel();

            return View(model);
        }

        // POST: createNewProjectFile
        [HttpPost]
        public ActionResult createNewProjectFile(ProjectFileViewModel model)
        {
            _projectFileService.addProjectFile(model);

            return RedirectToAction("Index", "Home");
        }

        // GET: updateProjectFile
        [HttpGet]
        public ActionResult updateProjectFile(int? id)
        {
            return null;
        }

        // POST: updateProjectFile
        [HttpPost]
        public ActionResult updateProjectFile(ProjectFileViewModel model)
        {
            return null;
        }
    }
}