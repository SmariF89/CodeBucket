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
    [ValidateInput(false)] ////////////////////////////////////// neeeded ? keep for a bit just in case!!!
    public class ProjectFileController : Controller
    {
        private ProjectFileService _projectFileService = new ProjectFileService();
        private ProjectService _projectService = new ProjectService();

        #region Create new file in current project.
        // GET: createNewProjectFile
        [HttpGet]
        public ActionResult createNewProjectFile(int? id)
        {

            CreateProjectFileViewModel model = new CreateProjectFileViewModel();

            model._projectID = id.Value;


            return View(model);
        }

        // POST: createNewProjectFile

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createNewProjectFile(CreateProjectFileViewModel model)
        {

            model._projectFileType = _projectFileService.getFileTypeByProjectId(model._projectID);
            model._projectFileData = "";

            if (!ModelState.IsValid)
            {
                CreateProjectFileViewModel viewModel = new CreateProjectFileViewModel();
                viewModel._projectFileName = model._projectFileName;
                viewModel._projectID = model._projectID;

                return View("createNewProjectFile", viewModel);
            }
            else
            {
                _projectFileService.addProjectFile(model);
                ProjectViewModel viewModel = _projectService.getProjectByProjectId(User.Identity.Name, model._projectID);
                //return RedirectToAction("displayProject", "ProjectFile", new { model._projectID });
                return View("displayProject", viewModel);
            }


        }
        #endregion

        #region Update file in current project.
        // GET: updateProjectFile
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult updateProjectFile(int id)
        {
            if (id != 0)
            {
                ProjectFileViewModel model = new ProjectFileViewModel();
                model = _projectFileService.getProjectFileByProjectFileId(id);
                return View(model);
            }
            return null;
        }
        [ValidateInput(false)]
        // POST: updateProjectFile
        [HttpPost]
        public ActionResult updateProjectFile(ProjectFileViewModel model)
        {
            if (model._projectFileData == null)
            {
                model._projectFileData = "";
            }
            if (model._id != 0)
            {
                _projectFileService.updateProjectFile(model);
                return View(model);
            }
            return HttpNotFound();
        }
        #endregion

        #region List all files in current project.
        [HttpGet]

        //The parameter was int? id if it matters TODO: Eyða fyrir skil
        public ActionResult displayProject(int id)
        {
            ProjectViewModel model = _projectService.getProjectByProjectId(User.Identity.Name, id);


            string owner = _projectFileService.getOwnerName(id);
            model._projectOwnerName = owner;

            return View(model);
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
        [ValidateAntiForgeryToken]
        public ActionResult addProjectMember(AddMemberViewModel model)
        {
            int currId = model._projectID;

            if (!ModelState.IsValid)
            {
                AddMemberViewModel viewModel = new AddMemberViewModel();
                viewModel._userName = model._userName;
                viewModel._projectID = model._projectID;

                return View("addProjectMember", viewModel);
            }
            else
            {
                _projectService.addProjectMember(model);
                ProjectViewModel model2 = _projectService.getProjectByProjectId(User.Identity.Name, currId);

                return View("displayProject", model2);
            }
        }
        #endregion

        [HttpGet]
        public ActionResult deleteProjectFile(int? id)
        {
            if (id != null)
            {
                ProjectFileViewModel model = new ProjectFileViewModel();
                model = _projectFileService.getProjectFileByProjectFileId(id.Value);
                
                return View(model);
            }

            return HttpNotFound();
        }


        [HttpPost, ActionName("deleteProjectFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectFileViewModel model = new ProjectFileViewModel();
            model = _projectFileService.getProjectFileByProjectFileId(id);

            if (model._projectFileName == null)
            {
                // TODO: Delete project.
            }

            int idOfProject = model._projectID;

            _projectFileService.deleteProjectFile(id);

            return RedirectToAction("displayProject" + "/" + idOfProject.ToString());
        }

        public ActionResult Chat()
        {
            return View();
        }

        //[HttpGet]
        //      public ActionResult showEditorForProjectFile(int? id)
        //      {
        //	if (id.HasValue)
        //	{
        //		ProjectFileViewModel model =  _projectFileService.getProjectFileById(id);
        //		return View(model);
        //	}
        //	return null;
        //      }

        //[HttpPost]
        //public ActionResult showEditorForProjectFile(ProjectFileViewModel model)
        //{
        //	if (ModelState.IsValid)
        //	{
        //		_projectFileService.updateProjectFile(model);
        //		//showEditorForProjectFile(model._id);
        //		return View(model);
        //	}
        //	return HttpNotFound();
        //}
    }


}