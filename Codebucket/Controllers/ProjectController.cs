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
    public class ProjectController : Controller
    {
        private ProjectFileService _projectFileService = new ProjectFileService(null);
        private ProjectService _projectService = new ProjectService(null);
        private UserService _userService = new UserService(null);

        #region Display all files.
        [HttpGet]
        public ActionResult DisplayProject(int? id)
        {
            if (id != null)
            {
                if (_userService.isProjectOwnerOrMember(User.Identity.Name, id.Value))
                {
                    ProjectViewModel model = _projectService.getProjectByProjectId(User.Identity.Name, id);

                    string owner = _userService.getOwnerName(id.Value);
                    model._projectOwnerName = owner;

                    return View(model);
                }
            }


            return HttpNotFound();
        }
        #endregion

        #region Create file.
        // GET: createNewProjectFile
        [HttpGet]
        public ActionResult CreateNewProjectFile(int? id)
        {
            if (id != null)
            {
                if (_projectService.projectExist(id.Value))
                {
                    if (_userService.isProjectOwner(User.Identity.Name, id.Value))
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
        public ActionResult CreateNewProjectFile(CreateProjectFileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                CreateProjectFileViewModel viewModel = new CreateProjectFileViewModel();
                viewModel._projectFileName = model._projectFileName;
                viewModel._projectID = model._projectID;
                return View("CreateNewProjectFile", viewModel);
            }
            else
            {
                model._projectFileType = _projectFileService.getFileTypeByProjectId(model._projectID);
                model._projectFileData = "";
                _projectFileService.addProjectFile(model);
                ProjectViewModel viewModel = _projectService.getProjectByProjectId(User.Identity.Name, model._projectID);
                return View("DisplayProject", viewModel);
            }
        }
        #endregion

        #region Edit file.
        // GET: EditProjectFile
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult EditProjectFile(int? id) // id -> nafn
        {
            if (id != null)
            {
                if (_projectFileService.doesProjectFileExist(id.Value))
                {
                    int projectId = _projectFileService.getProjectFileByProjectFileId(id.Value)._projectID;

                    if (_userService.isProjectOwnerOrMember(User.Identity.Name, projectId))
                    {
                        ProjectFileViewModel model = new ProjectFileViewModel();
                        model = _projectFileService.getProjectFileByProjectFileId(id.Value);
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index", "Overview");
        }

        [ValidateInput(false)]
        // POST: EditProjectFile
        [HttpPost]
        public ActionResult EditProjectFile(ProjectFileViewModel model) // FIXME:: model.isvalid check, need id != 0?
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
        
        #region Add member.
        // GET: AddProjectMember
        [HttpGet]
        public ActionResult AddProjectMember(int? projectID)
        {
            if (_projectService.projectExist(projectID))
            {
                if (_userService.isProjectOwner(User.Identity.Name, projectID.Value))
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
        public ActionResult AddProjectMember(AddMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                AddMemberViewModel viewModel = new AddMemberViewModel();
                viewModel._userName = model._userName;
                viewModel._projectID = model._projectID;
                return View("AddProjectMember", viewModel);
            }
            else
            {
                _userService.addProjectMember(model);
                ProjectViewModel model2 = _projectService.getProjectByProjectId(User.Identity.Name, model._projectID);
                return View("DisplayProject", model2);
            }
        }
        #endregion

        #region Delete file.
        [HttpGet]
        public ActionResult DeleteProjectFile(int? id)
        {
            if (id != null)
            {
                if (_projectFileService.doesProjectFileExist(id.Value))
                {
                    int projectId = _projectFileService.getProjectFileByProjectFileId(id.Value)._projectID;

                    if (_userService.isProjectOwner(User.Identity.Name, projectId))
                    {
                        ProjectFileViewModel model = new ProjectFileViewModel();
                        model = _projectFileService.getProjectFileByProjectFileId(id.Value);
                        return View(model);
                    }
                }
            }
            return HttpNotFound();
        }
        
        [HttpPost, ActionName("DeleteProjectFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProjectFileConfirmed(int id)
        {
            ProjectFileViewModel model = new ProjectFileViewModel();
            model = _projectFileService.getProjectFileByProjectFileId(id);
            _projectFileService.deleteProjectFile(id);
            return RedirectToAction("DisplayProject" + "/" + model._projectID.ToString());
        }
        #endregion

        #region Delete member.
        [HttpGet]
        public ActionResult DeleteProjectMember(int? projectMemberID)
        {
            if (projectMemberID != null)
            {
                if (_userService.isProjectMemberInAnyProject(projectMemberID.Value))
                {
                    ProjectMemberViewModel model = new ProjectMemberViewModel();
                    model = _userService.getProjectMemberByProjectMemberID(projectMemberID.Value);

                    if (_userService.isProjectOwner(User.Identity.Name, model._projectID))
                    {
                        return View(model);
                    }
                }
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("DeleteProjectMember")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProjectMemberConfirmed(int? projectMemberID)
        {
            if (projectMemberID != null)
            {
                if (_userService.isProjectMemberInAnyProject(projectMemberID.Value))
                {
                    ProjectMemberViewModel model = new ProjectMemberViewModel();
                    model = _userService.getProjectMemberByProjectMemberID(projectMemberID.Value);

                    if (_userService.isProjectOwner(User.Identity.Name, model._projectID))
                    {
                        _userService.deleteProjectMember(projectMemberID.Value);
                        return RedirectToAction("DisplayProject" + "/" + model._projectID.ToString());
                    }
                }
            }
            return HttpNotFound();
        }
        #endregion

        #region Leave project.
        [HttpGet]
        public ActionResult LeaveProject(int? projectID)
        {
            if (projectID != null)
            {
                if (_projectService.projectExist(projectID.Value))
                {
                    if (_userService.isProjectMember(User.Identity.Name, projectID.Value))
                    {
                        ProjectViewModel model = new ProjectViewModel();
                        model = _projectService.getProjectByProjectId(User.Identity.Name, projectID);
                        return View(model);
                    }
                }
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("LeaveProject")]
        [ValidateAntiForgeryToken]
        public ActionResult LeaveProjectConfirmed(int? projectID)
        {
            if (projectID != null)
            {
                if (_projectService.projectExist(projectID.Value))
                {
                    if (_userService.isProjectMember(User.Identity.Name, projectID.Value))
                    {
                        _userService.deleteProjectMemberByUserNameAndProjectID(User.Identity.Name, projectID.Value);
                        return RedirectToAction("Index", "Overview");
                    }
                }
            }
            return HttpNotFound();
        }
        #endregion 
    }
}