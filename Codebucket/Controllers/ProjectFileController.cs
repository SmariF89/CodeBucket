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

                return View("createNewProjectFile", viewModel);
            }
            else
            {
                _projectFileService.addProjectFile(model);
                ProjectViewModel viewModel = _projectService.getProjectByProjectId(model._projectID);
                //return RedirectToAction("displayProject", "ProjectFile", new { model._projectID });
                return View("displayProject", viewModel);
            }
        }
        #endregion

        #region Update file in current project.
        // GET: updateProjectFile
        [HttpGet]
        public ActionResult updateProjectFile(int id)
        {
            if (id != 0)
            {
                ProjectFileViewModel model = _projectFileService.getProjectFileByProjectId(id);
                return View(model);
            }
            return null;
        }

        // POST: updateProjectFile
        [HttpPost]
        public ActionResult updateProjectFile(ProjectFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                _projectFileService.updateProjectFile(model);
                return View(model);
            }
            return HttpNotFound();
        }
        #endregion

        [HttpGet]
        public ActionResult displayProject(int? id)
        {
            currentProjectId = id;
            ProjectViewModel model = _projectService.getProjectByProjectId(currentProjectId);


            if (_projectFileService.isProjectOwner(User.Identity.Name, id.Value))
            {
                model._isProjectOwner = true;
                
            }

            else
            {
                model._isProjectOwner = false;
                
            }
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
		[ValidateAntiForgeryToken]
		public ActionResult addProjectMember(AddMemberViewModel model)
		{
			int currId = model._projectID;

			if (!ModelState.IsValid)
			{
				AddMemberViewModel viewModel = new AddMemberViewModel();
				viewModel._userName = model._userName;

				return View("addProjectMember", viewModel);
			}

			else
			{

				_projectService.addProjectMember(model);
				ProjectViewModel model2 = _projectService.getProjectByProjectId(currId);

				return View("displayProject", model2);
			}
			
		}

		#endregion

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