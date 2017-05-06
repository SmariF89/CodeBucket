using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using Codebucket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Controllers
{
    [ValidateInput(false)]
    public class ProjectFileController : Controller
    {
        private ProjectFileService _projectFileService = new ProjectFileService();
        private ProjectService _projectService = new ProjectService();
        private int? currentProjectId;

        #region Create new file in current project.
        // GET: createNewProjectFile
        [HttpGet]
        public ActionResult createNewProjectFile()
        {
            List<Project> list = _projectFileService.getAllProjects();

            ProjectFileViewModel file = new ProjectFileViewModel()
            {
                project = list
            };

            return View(file);
        }

        // POST: createNewProjectFile
        [HttpPost]
        public ActionResult createNewProjectFile(ProjectFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                _projectFileService.addProjectFile(model);
            }

            return RedirectToAction("displayProject", "ProjectFile", new { currentProjectId });
        }
        #endregion

        #region Update file in current project.
        // GET: updateProjectFile
        [HttpGet]
        public ActionResult updateProjectFile(int? id)
        {
            return View();
        }

        // POST: updateProjectFile
        [HttpPost]
        public ActionResult updateProjectFile(ProjectFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                _projectFileService.updateProjectFile(model);
                return RedirectToAction("updateProjectFile", "ProjectFile");
            }
            return HttpNotFound();
        }
        #endregion

        [HttpGet]
        public ActionResult displayProject(int? id)
        {
            currentProjectId = id;
            ProjectViewModel model = _projectService.getProjectByProjectId(id);

            return View(model);
        }

        #region List all files in current project.
        // GET: getProjectFileById
        [HttpGet]
        public ActionResult listAllProjectFiles(int? id)
        {
            currentProjectId = id;

            return View(_projectFileService.getAllProjectFilesByProjectId(currentProjectId));

        }
        #endregion

        #region Add member to current project.   
        // GET: AddProjectMember
        [HttpGet]
        public ActionResult addProjectMember(int? id)
        {
            AddMemberViewModel model = new AddMemberViewModel();
            model._projectID = id.Value;

            return View(model);
        }

        // POST: AddProjectMember
        [HttpPost]
        public ActionResult addProjectMember(AddMemberViewModel model)
        {
            int? currId = model._projectID;

            if (ModelState.IsValid)
            {
                _projectService.addProjectMember(model);
                ProjectViewModel model2 = _projectService.getProjectByProjectId(currId);

                return View("displayProject" , model2);
            }
            return HttpNotFound();
        }
        #endregion


    }
}