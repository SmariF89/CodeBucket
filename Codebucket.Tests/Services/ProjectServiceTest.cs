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

            #region Initialize projects.
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

            var p7 = new ProjectFileViewModel
            {
                _id = 1,
                _projectFileData = "dada",
                _projectID = 1,
                _projectFileName = "lab98"
            };
            mockDb._projectFileViewModel.Add(p7);

            var p8 = new ProjectViewModel
            {
                _id = 1,
                _isProjectOwner = true,
                _projectName = "String"
            };
            mockDb._projectViewModel.Add(p8);

            var p9 = new ApplicationUserViewModel
            {
                _applicationUserName = "Bjarki"
                //_applicationUserProjects ""
            };
            mockDb.UserViewModel.Add(p9);
            #endregion

            _service = new ProjectService(mockDb);
        }

        // Test.

        #region projectExists function.
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
        #endregion
    }
}
