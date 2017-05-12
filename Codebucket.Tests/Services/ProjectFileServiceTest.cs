using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codebucket.Services;
using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;

namespace Codebucket.Tests.Services
{
    [TestClass]
    public class ProjectFileServiceTest
    {
        private ProjectFileService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

            #region Initialize files.
            var f1 = new ProjectFile
            {
                ID = 1,
                _projectID = 2,
                _projectFileName = "TestFile_01",
                _projectFileData = "...lorem ipsum...",
                _projectFileType = ".css",
                _aceExtension = "css"
            };
            mockDb._projectFiles.Add(f1);

            var f2 = new ProjectFile
            {
                ID = 2,
                _projectID = 2,
                _projectFileName = "TestFile_02",
                _projectFileData = "...lorem ipsum...",
                _projectFileType = ".css",
                _aceExtension = "css"
            };
            mockDb._projectFiles.Add(f2);

            var f3 = new ProjectFile
            {
                ID = 3,
                _projectID = 3,
                _projectFileName = "TestFile_03",
                _projectFileData = "...lorem ipsum...",
                _projectFileType = ".cs",
                _aceExtension = "csharp"
            };
            mockDb._projectFiles.Add(f3);

            var f4 = new ProjectFile
            {
                ID = 4,
                _projectID = 3,
                _projectFileName = "TestFile_04",
                _projectFileData = "...lorem ipsum...",
                _projectFileType = ".cs",
                _aceExtension = "csharp"
            };
            mockDb._projectFiles.Add(f4);
            #endregion 

            #region Initialize Projects.
            var p1 = new Project
            {
                ID = 1,
                _projectFileTypeId = 1,
                _projectName = "TestProject_01"
            };
            mockDb._projects.Add(p1);
            
            var p2 = new Project
            {
                ID = 2,
                _projectFileTypeId = 2,
                _projectName = "TestProject_02"
            };
            mockDb._projects.Add(p2);

            var p3 = new Project
            {
                ID = 3,
                _projectFileTypeId = 3,
                _projectName = "TestProject_03"
            };
            mockDb._projects.Add(p1);
            #endregion

            _service = new ProjectFileService(mockDb);
        }

        // Tests.

        #region getFiles function.
        [TestMethod]
        public void TestUpdateProjectFile()
        {
            // Arrange:
            CreateProjectFileViewModel model = new CreateProjectFileViewModel();
            model._projectID = 2;
            model._projectFileName = "AddTestFile_01";
            model._projectFileType = ".css";
            model._projectFileData = "hodor hodOr HODOr...hODOr";
            model._isUserProjectOwner = true;

            _service.addProjectFile(model);

            ProjectFileViewModel modelUpdate = new ProjectFileViewModel();
            modelUpdate._id = 0;
            modelUpdate._projectFileData = "bacon bacOn BACOn...bACOn";

            // Act:
            _service.updateProjectFile(modelUpdate);

            // Assert:
            Assert.AreEqual("bacon bacOn BACOn...bACOn", _service.getProjectFileByProjectFileId(0)._projectFileData);
        }

        [TestMethod]
        public void TestGetFileType()
        {
            // Arrange:
            const int id = 2;

            // Act:
            var result = _service.getFileTypeByProjectId(id);

            // Assert:
            Assert.AreEqual("css", result);
        }

        [TestMethod]
        public void TestGetAceExtensionByProjectId()
        {
            // Arrange:
            const int id = 3;

            // Act:
            var result = _service.getAceExtensionByProjectId(id);

            // Assert:
            Assert.AreEqual("csharp", result);
        }

        //The mockdatabase always makes the ID (Primary key) zero. That's why
        //That's why I send 0 into the doesProjectFileExist() method.
        [TestMethod]
        public void TestAddProjectFile()
        {
            // Arrange:
            CreateProjectFileViewModel model = new CreateProjectFileViewModel();
            model._projectID = 2;
            model._projectFileName = "AddTestFile_01";
            model._projectFileType = ".css";
            model._projectFileData = "hodor hodOr HODOr...hODOr";
            model._isUserProjectOwner = true;

            bool initialValue = _service.doesProjectFileExist(0);

            // Act:
            _service.addProjectFile(model);
            initialValue = _service.doesProjectFileExist(0);

            // Assert:
            Assert.IsTrue(initialValue);
        }

        [TestMethod]
        public void TestGetUniqueFile()
        {
            // Arrange:
            const int id = 3;

            // Act:
            var result = _service.getProjectFileByProjectFileId(id);

            // Assert:
            Assert.AreEqual("TestFile_03", result._projectFileName);
        }

        [TestMethod]
        public void TestGetUniqueFileFail()
        {
            // Arrange:
            const int id = -15;

            // Act:
            var result = _service.getProjectFileByProjectFileId(id);

            // Assert:
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetFiles()
        {
            // Arrange: 
            const int id = 3;

            // Act:
            var result = _service.getAllProjectFilesByProjectId(id);

            // Assert:
            Assert.AreEqual(2, result.Count);
        }
        #endregion

        #region getFilesFromNonExistantProject function.
        [TestMethod]
        public void TestGetFilesFromNonExistantProject()
        {
            // Arrange: 
            const int id = 1337;

            // Act:
            var result = _service.getAllProjectFilesByProjectId(id);

            // Assert:
            Assert.AreEqual(0, result.Count);
        }
        #endregion
    }
}
