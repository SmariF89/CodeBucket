using Codebucket.Models.Entities;
using Codebucket.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//using Microsoft.AspNet.Identity;

////public class ApplicationUser
////{
////    [Key]
////    public int ID { get; set; }
////    public string UserName { get; set; }
////}


namespace Codebucket.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private UserService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

            var p1 = new ProjectOwner
            {
                ID = 1,
                _projectID = 1,
                _userName = "David"
            };
            mockDb._projectOwners.Add(p1);

            var p2 = new ProjectOwner
            {
                ID = 2,
                _projectID = 2,
                _userName = "Helgi"
            };
            mockDb._projectOwners.Add(p2);

            var p3 = new ProjectOwner
            {
                ID = 3,
                _projectID = 3,
                _userName = "Jon"
            };
            mockDb._projectOwners.Add(p3);

            var p4 = new ProjectOwner
            {
                ID = 4,
                _projectID = 4,
                _userName = "Palli"
            };
            mockDb._projectOwners.Add(p4);

            var p5 = new ProjectOwner
            {
                ID = 5,
                _projectID = 3,
                _userName = "Omar"
            };
            mockDb._projectOwners.Add(p5);

            var p6 = new ProjectOwner
            {
                ID = 6,
                _projectID = 1,
                _userName = "Kalli"
            };
            mockDb._projectOwners.Add(p6);

            var p7 = new ProjectOwner
            {
                ID = 7,
                _projectID = 2,
                _userName = "Snorri"
            };
            mockDb._projectOwners.Add(p7);

            var p8 = new ProjectOwner
            {
                ID = 8,
                _projectID = 9,
                _userName = "Onagamade Palame"
            };
            mockDb._projectOwners.Add(p8);

            var p9 = new ProjectOwner
            {
                ID = 9,
                _projectID = 12,
                _userName = "Allah Achmed"
            };
            mockDb._projectOwners.Add(p9);

            var p10 = new ProjectOwner
            {
                ID = 10,
                _projectID = 23,
                _userName = "Palli"
            };
            mockDb._projectOwners.Add(p10);
            _service = new UserService(mockDb);
        }

        [TestMethod]
        public void TestisProjectOwner()
        {
            // Arrange: 
            const string username = "Palli";
            const int projectID = 4;

            // Act:
            var result = _service.isProjectOwner(username, projectID);

            // Assert:
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void TestisProjectOwner2()
        {
            // Arrange: 
            const string username = "Palli";
            const int projectID = 1;

            // Act:
            var result = _service.isProjectOwner(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
    }
}
