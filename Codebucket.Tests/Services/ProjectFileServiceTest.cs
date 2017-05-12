using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codebucket.Services;
using Codebucket.Models.Entities;

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
                _projectID = 3,
                _projectFileName = "TestFile_01",
                _projectFileData = "...lorem ipsum...",
                _projectFileType = ".html",
                _aceExtension = "html"
            };
            mockDb._projectFiles.Add(f1);

            var f2 = new ProjectFile
            {
                ID = 2,
                _projectID = 3,
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
                _projectFileType = ".js",
                _aceExtension = "javascript"
            };
            mockDb._projectFiles.Add(f4);
            #endregion 

            #region Initialize Project.
            var p1 = new Project
            {
                ID = 3,
                _projectFileTypeId = 1,
                _projectName = "TestProject_01"
            };
            mockDb._projects.Add(p1);
            #endregion

            _service = new ProjectFileService(mockDb);
        }

        #region getFiles function.
        [TestMethod]
        public void TestGetFiles()
        {
            // Arrange: 
            const int id = 3;

            // Act:
            var result = _service.getAllProjectFilesByProjectId(id);

            // Assert:
            Assert.AreEqual(4, result.Count);
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
