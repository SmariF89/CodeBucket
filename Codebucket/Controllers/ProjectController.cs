using Codebucket.Models.ViewModels;
using Codebucket.Services;
using System.Web.Mvc;

namespace Codebucket.Controllers
{
    public class ProjectController : Controller
    {
        /// <summary>
        /// Initializes new instances of services, used to call on service functions and get data or remove from Db.
        /// </summary>
        private ProjectFileService _projectFileService = new ProjectFileService(null);
        private ProjectService _projectService = new ProjectService(null);
        private UserService _userService = new UserService(null);

        #region Display all files.
        /// <summary>
        /// Displays a selected project to user by project ID, providing project id is valid to current user. 
        /// User has to be a member or a owner or the project, if not redirect to HttpNotFound.
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>ActionResult</returns>
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
        /// <summary>
        /// If project id is valid and user is the owner, send 'CreateProjectFileViewModel' to view 
        /// else return HttpNotFound.
        /// </summary>
        /// <param name="id">Proejct ID</param>
        /// <returns>ActionResult</returns>
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

        /// <summary>
        /// If model from is valid forward the model to service layer and add to Db and display the project view.
        /// Else it form is not valid return the values from model back to 'CreateNewProjectFile' view.
        /// </summary>
        /// <param name="model">CreateProjectFileViewModel</param>
        /// <returns>ActionResult</returns>
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
        /// <summary>
        /// If file id is valid, it sends 'ProjectFileViewModel' to view. Else return to overview/index.
        /// </summary>
        /// <param name="id">File ID</param>
        /// <returns>ActionResult</returns>
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult EditProjectFile(int? id)
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

        /// <summary>
        /// If model data is null replace it with empty string instead and if model id is not equal to 0
        /// send model forward to service layer and update the new data to Db.
        /// </summary>
        /// <param name="model">ProjectFileViewModel</param>
        /// <returns>ActionResult</returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditProjectFile(ProjectFileViewModel model)
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
        /// <summary>
        /// If project id is valid return a 'AddMemberViewModel' to view. Else return HttpNotFound.
        /// </summary>
        /// <param name="projectID">Project ID</param>
        /// <returns>ActionResult</returns>
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

        /// <summary>
        /// If model is valid, send the model forward to service layer and add project member to Db. 
        /// Else display current project again.
        /// </summary>
        /// <param name="model">AddMemberViewModel</param>
        /// <returns>ActionResult</returns>
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
        /// <summary>
        /// If model is valid, return a 'ProjectFileViewModel' to view. Else return HttpNotFound.
        /// </summary>
        /// <param name="id">File ID</param>
        /// <returns>ActionResult</returns>
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

        /// <summary>
        /// Sends the file id forward to service layer and removes the file from Db and then redirects the action
        /// to display the current project again.
        /// </summary>
        /// <param name="id">File ID</param>
        /// <returns>ActionResult</returns>
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
        /// <summary>
        /// If project member id is valid, return a 'ProjectMemberViewModel' to view. Else reutrn HttpNotFound.
        /// </summary>
        /// <param name="projectMemberID">Project Member ID</param>
        /// <returns>ActionResult</returns>
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

        /// <summary>
        /// If project member id is valid, forward the project member id to service layer and remove the member from Db.
        /// Else return HttpNotFound.
        /// </summary>
        /// <param name="projectMemberID">Project Member ID</param>
        /// <returns>ActionResult</returns>
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
        /// <summary>
        /// If project ID is valid, return a 'ProjectViewModel' to view. Else return HttpNotFound.
        /// </summary>
        /// <param name="projectID">Project ID</param>
        /// <returns>ActionResult</returns>
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

        /// <summary>
        /// If project ID is valid, forward username and project ID to service layer and removes the member from project 
        /// member Db. Else return HttpNotFound.
        /// </summary>
        /// <param name="projectID">Project ID</param>
        /// <returns>ActionResult</returns>
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