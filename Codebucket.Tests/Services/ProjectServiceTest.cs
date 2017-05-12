using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using Codebucket.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebucket.Tests.Services
{
    [TestClass]
    public class ProjectServiceTest
    {
        private ProjectService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

            var p1 = new Project
            {
              
                ID = 1,
                _projectName = "Project_1",
                _projectFileTypeId = 1,
                

            };
            mockDb._projects.Add(p1);

            var p2 = new Project
            {
                ID = 2,
                _projectName = "Project_2",
                _projectFileTypeId = 2
            };
            mockDb._projects.Add(p2);

            var p3 = new Project
            {
                ID = 3,
                _projectName = "Project_3",
                _projectFileTypeId = 3
            };
            mockDb._projects.Add(p3);

            var p4 = new Project
            {
                ID = 4,
                _projectName = "Project_4",
                _projectFileTypeId = 4
            };
            mockDb._projects.Add(p4);

            var p5 = new ProjectFile
            {
                 ID = 1,
                _projectID = 1,
                _projectFileName = "lab1", 
                _projectFileType = "html",
                _aceExtension = "html",
                _projectFileData = "hello"
            };
            mockDb._projectFiles.Add(p5);

            var p6 = new ProjectOwner
            {
                ID = 1,
                _projectID = 1,
                _userName = "Bjarki"

            };
            mockDb._projectOwners.Add(p6);

            var p7 = new CreateProjectViewModel
            {
                
               _projectName = "Lab42",
               _projectTypeId = 1
            };

            var p8 = new ApplicationUserViewModel
            {
                _applicationUserName = "Bjarki"
                //_applicationUserProjects ""
            };
            mockDb.UserViewModel.Add(p8);

            _service = new ProjectService(mockDb);
        }

        public void addProject()
        {
            string owner = "Bjarki";
            CreateProjectViewModel model = new CreateProjectViewModel();
            model._projectName = "Lab42";
            model._projectTypeId = 1;
            //Arrange:

            _service.addProject(model, owner);
            ////not done

            //Assert:

        }

        //public void addProject()
        //{


        //    string owner = "Bjarki";
        //    CreateProjectViewModel model = new CreateProjectViewModel();
        //    model._projectName = "Lab42";
        //    model._projectTypeId = 1;
        //    //Arrange:
        //    const int id = 1337;


        //    _service.addProject(model, owner);
        //    ////not done

        //Assert:

        //    //Assert:
        //    Assert.IsFalse(result);
        //}



        [TestMethod]
        public void TestProjectExist()
        {
            // Arrange: 
            int id = 1;

            // Act:
            var result = _service.projectExist(id);

            // Assert:
            Assert.IsTrue(result);
        }
     
        //[TestMethod]
        //public void getProjectByProjectId()
        //{
        //    string user = "Bjarki";
        //    int id = 1;

        //    var result = _service.getProjectByProjectId(user, id);
        //    //is not working

        //    Assert.IsNotNull(result);
        //}
        //[TestMethod]
        //public void TestcreateNewProjectIsValid()
        //{
        //    string projectName = "Project_1";
        //    string username = "Bjarki";

        //    var result = _service.createNewProjectIsValid(projectName, username);

        //    Assert.IsFalse(result);

        //}

       

    }
}
