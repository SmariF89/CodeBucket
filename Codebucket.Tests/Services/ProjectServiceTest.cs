using Codebucket.Models.Entities;
using Codebucket.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                _projectFileTypeId = 1
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

            _service = new ProjectService(mockDb);
        }

        [TestMethod]
        public void TestProjectExist()
        {
            // Arrange: 
            const int id = 1337;

            // Act:
            var result = _service.projectExist(id);

            // Assert:
            Assert.IsFalse(result);
        }
    }
}
