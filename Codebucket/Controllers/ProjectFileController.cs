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
    public class ProjectFileController : Controller
    {
        private ProjectFileService _projectFileService = new ProjectFileService();
        private ProjectService _projectService = new ProjectService();

        #region Create new file in current project.
        // GET: createNewProjectFile
        [HttpGet]
        public ActionResult createNewProjectFile(int? id)
        {
            if (id != null)
            {
                if (_projectService.projectExist(id.Value))
                {
                    if (_projectFileService.isProjectOwner(User.Identity.Name, id.Value))
                    {
                        CreateProjectFileViewModel model = new CreateProjectFileViewModel();
                        model._projectID = id.Value;

                        return View(model);
                    }
                }
            }

            return HttpNotFound();
        }

        // POST: createNewProjectFile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createNewProjectFile(CreateProjectFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                CreateProjectFileViewModel viewModel = new CreateProjectFileViewModel();
                viewModel._projectFileName = model._projectFileName;
                viewModel._projectID = model._projectID;

                return View("createNewProjectFile", viewModel);
            }
            else
            {
                model._projectFileType = _projectFileService.getFileTypeByProjectId(model._projectID);
                model._projectFileData = "";
                _projectFileService.addProjectFile(model);
                ProjectViewModel viewModel = _projectService.getProjectByProjectId(User.Identity.Name, model._projectID);

                return View("displayProject", viewModel);
            }
        }
        #endregion

        #region Update file in current project.
        // GET: updateProjectFile
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult updateProjectFile(int? id)
        {
            if (id != null)
            {
                if (_projectFileService.doesProjectFileExist(id.Value))
                {
                    int projectId = _projectFileService.getProjectFileByProjectFileId(id.Value)._projectID;

                    if (_projectFileService.isProjectOwnerOrMember(User.Identity.Name, projectId))
                    {
                        ProjectFileViewModel model = new ProjectFileViewModel();
                        model = _projectFileService.getProjectFileByProjectFileId(id.Value);

                        return View(model);
                    }
                }
            }
            

            return RedirectToAction("Index", "Project");
        }
        [ValidateInput(false)]
        // POST: updateProjectFile
        [HttpPost]
        public ActionResult updateProjectFile(ProjectFileViewModel model) // FIXME:: model.isvalid check, need id != 0?
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
        public ActionResult displayProject(int? id)
        {
            if (id != null)
            {
                if (_projectFileService.isProjectOwnerOrMember(User.Identity.Name, id.Value))
                {
                    ProjectViewModel model = _projectService.getProjectByProjectId(User.Identity.Name, id);

                    string owner = _projectFileService.getOwnerName(id.Value);
                    model._projectOwnerName = owner;

                    return View(model);
                }
            }
            

            return HttpNotFound();
        }
        #endregion

        #region Add member to current project.
        // GET: AddProjectMember
        [HttpGet]
        public ActionResult addProjectMember(int? projectID)
        {
            if (_projectService.projectExist(projectID))
            {
                if (_projectFileService.isProjectOwner(User.Identity.Name, projectID.Value))
                {
                    AddMemberViewModel model = new AddMemberViewModel();
                    model._projectID = projectID.Value;

                    return View(model);
                }
            }

            return HttpNotFound();
        }

        // POST: AddProjectMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addProjectMember(AddMemberViewModel model)
        {
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
                ProjectViewModel model2 = _projectService.getProjectByProjectId(User.Identity.Name, model._projectID);

                return View("displayProject", model2);
            }
        }
        #endregion

        #region Delete file from project.
        [HttpGet]
        public ActionResult deleteProjectFile(int? id)
        {
            if (id != null)
            {
                if (_projectFileService.doesProjectFileExist(id.Value))
                {
                    int projectId = _projectFileService.getProjectFileByProjectFileId(id.Value)._projectID;

                    if (_projectFileService.isProjectOwner(User.Identity.Name, projectId))
                    {
                        ProjectFileViewModel model = new ProjectFileViewModel();
                        model = _projectFileService.getProjectFileByProjectFileId(id.Value);

                        return View(model);
                    }
                }
            }

            return HttpNotFound();
        }


        [HttpPost, ActionName("deleteProjectFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectFileViewModel model = new ProjectFileViewModel();
            model = _projectFileService.getProjectFileByProjectFileId(id);

            int idOfProject = model._projectID;

            _projectFileService.deleteProjectFile(id);

            return RedirectToAction("displayProject" + "/" + idOfProject.ToString());
        }
        #endregion

        #region Delete member from project.
        [HttpGet]
        public ActionResult deleteProjectMember(int? projectMemberID)
        {
            if (projectMemberID != null)
            {
                if (_projectFileService.isProjectMemberInAnyProject(projectMemberID.Value))
                {
                    ProjectMemberViewModel model = new ProjectMemberViewModel();
                    model = _projectFileService.getProjectMemberByProjectMemberID(projectMemberID.Value);

                    if (_projectFileService.isProjectOwner(User.Identity.Name, model._projectID))
                    {
                        return View(model);
                    }
                }
            }

            return HttpNotFound();
        }

        [HttpPost, ActionName("deleteProjectMember")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProjectMemberConfirmed(int? projectMemberID)
        {
            if (projectMemberID != null)
            {
                if (_projectFileService.isProjectMemberInAnyProject(projectMemberID.Value))
                {
                    ProjectMemberViewModel model = new ProjectMemberViewModel();
                    model = _projectFileService.getProjectMemberByProjectMemberID(projectMemberID.Value);

                    if (_projectFileService.isProjectOwner(User.Identity.Name, model._projectID))
                    {
                        _projectFileService.deleteProjectMember(projectMemberID.Value);

                        return RedirectToAction("displayProject" + "/" + model._projectID.ToString());
                    }
                }
            }

            return HttpNotFound();
        }
        #endregion

        #region Leave project.
        [HttpGet]
        public ActionResult leaveProject(int? projectID)
        {
            if (projectID != null)
            {
                if (_projectService.projectExist(projectID.Value))
                {
                    if (_projectFileService.isProjectMember(User.Identity.Name, projectID.Value))
                    {
                        ProjectViewModel model = new ProjectViewModel();
                        model = _projectService.getProjectByProjectId(User.Identity.Name, projectID);

                        return View(model);
                    }
                }
            }

            return HttpNotFound();
        }

        [HttpPost, ActionName("leaveProject")]
        [ValidateAntiForgeryToken]
        public ActionResult LeaveProjectConfirmed(int? projectID)
        {
            if (projectID != null)
            {
                if (_projectService.projectExist(projectID.Value))
                {
                    if (_projectFileService.isProjectMember(User.Identity.Name, projectID.Value))
                    {
                        _projectFileService.deleteProjectMemberByUserNameAndProjectID(User.Identity.Name, projectID.Value);

                        return RedirectToAction("Index", "Project");
                    }
                }
            }

            return HttpNotFound();
        }
        #endregion 


        #region Chat.
        public ActionResult Chat()
        {
            return View();
        }
        #endregion
    }
}