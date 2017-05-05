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

		// GET: ProjectFile
		public ActionResult Index()
		{
			return View();
		}

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
				return RedirectToAction("Index", "Home");
			}
           // model.project = _projectFileService.getAllProjects();
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

		// GET: getProjectFileById
		[HttpGet]
		public ActionResult getProjectFileById(int? id)
		{
            return View(_projectFileService.getAllProjectFilesByProjectId(1));
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