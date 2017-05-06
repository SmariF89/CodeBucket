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


		// GET: createNewProjectFile
		[HttpGet]
		public ActionResult createNewProjectFile()
		{
			var list = _projectFileService.getAllProjects();

			// Hver skrá getur bara tengst einu verkefni

			// Hvert verkefni getur haft margar skrár 

			ProjectFileViewModel file = new ProjectFileViewModel()
			{
				project = list
			};

            //var model = new ProjectFileViewModel();

            //var db = _projectFileService;

            //ViewBag.Files = new SelectList(db.getAllProjects, "ID", );

			//Viljum senda inn 
			//ProjectFileViewModel model = new ProjectFileViewModel();

			return View(file);
		}

		// POST: createNewProjectFile
		[HttpPost]
		public ActionResult createNewProjectFile(ProjectFileViewModel model)
		{
		    if(ModelState.IsValid)
			    {
                    //model.project = _projectFileService.getAllProjects();

                   _projectFileService.addProjectFile(model);
			    }
               // model.project = _projectFileService.getAllProjects();
                return RedirectToAction("listAllProjectFiles", "ProjectFile", new { currentProjectId });

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

		// GET: getProjectFileById
		[HttpGet]
		public ActionResult listAllProjectFiles(int? id)
		{
            currentProjectId = id;

            return View(_projectFileService.getAllProjectFilesByProjectId(currentProjectId));
           
		}


        public ActionResult Create()
        {
            return View();
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
            if (ModelState.IsValid)
            {
                _projectService.addProjectMember(model);
                return RedirectToAction("listAllProjectFiles", "ProjectFile", new { currentProjectId });
            }
            return HttpNotFound();
        }

		[HttpGet]
        public ActionResult showEditorForProjectFile(int? id)
        {
			if (id.HasValue)
			{
				ProjectFileViewModel model =  _projectFileService.getProjectFileById(id);
				return View(model);
			}
			return null;
        }

		[HttpPost]
		public ActionResult showEditorForProjectFile(ProjectFileViewModel model)
		{
			if (ModelState.IsValid)
			{
				_projectFileService.updateProjectFile(model);
				//showEditorForProjectFile(model._id);
				return View(model);
			}
			return HttpNotFound();
		}


		//// POST: getProjectFileById // probably not needed since only get is used.
		//[HttpPost]
		//public ActionResult getProjectFileById(int? id)
		//{
		//	ProjectFileViewModel model = _projectFileService.getProjectFileById(id);
		//	return View(model);
		//}

		/* private IEnumerable<Project> GetAllProjects()
         {

         } */

		/*public static IEnumerable<SelectListItem> ToSelectListItem(this IEnumerable<Project> projects, int selectedID)
        {
            return
                projects.OrderBy(project => project._projectName)
                .Select(project => new SelectListItem
                {
                    Selected = (project.ID == selectedID),
                    Text = project._projectName,
                    Value = project.ID.ToString()

                });

        }*/
	}
}