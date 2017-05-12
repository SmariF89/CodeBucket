using Codebucket.Models.Entities;
using Codebucket.Models.ViewModels;
using Codebucket.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Codebucket.Models;


namespace Codebucket.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private UserService _userService;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

            #region User initialized.
            var u1 = new ApplicationUser
            {
                Id = "1",
                UserName = "Alberta",
                Email = "Alberta@Alberta.is",
                PasswordHash = "Alberta1",
                SecurityStamp = "Alberta",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
            };
            mockDb.Users.Add(u1);
            #endregion

            #region Projects initialized.
            var pro1 = new Project
            {
                ID = 1,
                _projectFileTypeId = 1,
                _projectName = "pro1"
            };
            mockDb._projects.Add(pro1);

            var pro2 = new Project
            {
                ID = 2,
                _projectFileTypeId = 2,
                _projectName = "pro2"
            };
            mockDb._projects.Add(pro2);
            #endregion

            #region Project owners initialized.
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
            #endregion

            #region Project members initialized.
            var m1 = new ProjectMember
            {
                ID = 1,
                _projectID = 23,
                _userName = "Arni"
            };
            mockDb._projectMembers.Add(m1);

            var m2 = new ProjectMember
            {
                ID = 2,
                _projectID = 2,
                _userName = "Armann"
            };
            mockDb._projectMembers.Add(m2);

            var m3 = new ProjectMember
            {
                ID = 3,
                _projectID = 55,
                _userName = "Pesi pa"
            };
            mockDb._projectMembers.Add(m3);

            var m4 = new ProjectMember
            {
                ID = 4,
                _projectID = 27,
                _userName = "Halli Hall"
            };
            mockDb._projectMembers.Add(m4);

            var m5 = new ProjectMember
            {
                ID = 10,
                _projectID = 23,
                _userName = "Unnar L"
            };
            mockDb._projectMembers.Add(m5);

            var m6 = new ProjectMember
            {
                ID = 6,
                _projectID = 16,
                _userName = "David"
            };
            mockDb._projectMembers.Add(m6);

            var m7 = new ProjectMember
            {
                ID = 7,
                _projectID = 99,
                _userName = "Gunnar"
            };
            mockDb._projectMembers.Add(m7);

            var m8 = new ProjectMember
            {
                ID = 8,
                _projectID = 255,
                _userName = "Gugga"
            };
            mockDb._projectMembers.Add(m8);

            var m9 = new ProjectMember
            {
                ID = 9,
                _projectID = 4,
                _userName = "Birta"
            };
            mockDb._projectMembers.Add(m9);

            var m10 = new ProjectMember
            {
                ID = 10,
                _projectID = 87,
                _userName = "Jon"
            };
            mockDb._projectMembers.Add(m10);

            var m11 = new ProjectMember
            {
                ID = 11,
                _projectID = 87,
                _userName = "Svanhildur"
            };
            mockDb._projectMembers.Add(m11);
            #endregion

            _userService = new UserService(mockDb);
        }

        // Tests.

        #region getAllProjectMembersByProjectID function.
        [TestMethod]
        public void TestGetAllProjectMembersByProjectId1()
        {
            // Arrange:
            const int projectID = 87;

            // Act:
            List<ProjectMember> result = _userService.getAllProjectMembersByProjectId(projectID);

            // Assert:
            List<ProjectMember> expected = new List<ProjectMember>();

            expected.Add(new ProjectMember
            {
                ID = 10,
                _projectID = 87,
                _userName = "Jon"
            });

            expected.Add(new ProjectMember
            {
                ID = 11,
                _projectID = 87,
                _userName = "Svanhildur"
            });

            CollectionAssert.Equals(expected, result);
        }

        [TestMethod]
        public void TestGetAllProjectMembersByProjectId2()
        {
            // Arrange:
            const int projectID = 200;

            // Act:
            List<ProjectMember> result = _userService.getAllProjectMembersByProjectId(projectID);

            // Assert:          
            CollectionAssert.Equals(0, result);
        }

        [TestMethod]
        public void TestGetAllProjectMembersByProjectId3()
        {
            // Arrange:
            const int projectID = -2;

            // Act:
            List<ProjectMember> result = _userService.getAllProjectMembersByProjectId(projectID);

            // Assert:          
            CollectionAssert.Equals(0, result);
        }
        #endregion
        
        #region getOwnerName
        [TestMethod]
        public void testGetOwnerName1()
        {
            // Arrange:
            const int projectID = 9;
            const string expected = "Onagamade Palame";
            // Act:
            string result = _userService.getOwnerName(projectID);

            // Assert:
            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void testGetOwnerName2()
        {
            // Arrange:
            const int projectID = 1000;
            const string expected = null;
            // Act:
            var result = _userService.getOwnerName(projectID);

            // Assert:
            Assert.AreEqual(expected, result);
        }
        #endregion

        #region addProjectMember function.
        [TestMethod]
        public void TestAddProjectMember()
        {
            // Arrange:
            AddMemberViewModel model = new AddMemberViewModel
            {
                _projectID = 1,
                _projectName = "pro1",
                _userName = "Alberta"
            };

            // Act:
            bool check1 = _userService.isProjectMember(model._userName, model._projectID);
            _userService.addProjectMember(model);
            bool check2 = _userService.isProjectMember(model._userName, model._projectID);

            // Assert:  
            Assert.AreNotEqual(check1, check2);
        }
        #endregion

        #region isProjectOwnerOrMember function.
        [TestMethod]
        public void TestIsProjectOwnerOrMember1()
        {
            //Arrange:
            const int projectID = 2;
            const string userName = "Snorri";

            // Act:
            bool result = _userService.isProjectOwnerOrMember(userName, projectID);

            // Assert:
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsProjectOwnerOrMember2()
        {
            //Arrange:
            const int projectID = 23;
            const string userName = "Unnar L";

            // Act:
            bool result = _userService.isProjectOwnerOrMember(userName, projectID);

            // Assert:
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsProjectOwnerOrMember3()
        {
            //Arrange:
            const int projectID = 1000;
            const string userName = "Johnny Hacker";

            // Act:
            bool result = _userService.isProjectOwnerOrMember(userName, projectID);

            // Assert:
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsProjectOwnerOrMember4()
        {
            //Arrange:
            const int projectID = -1;
            const string userName = "Johnny Minus";

            // Act:
            bool result = _userService.isProjectOwnerOrMember(userName, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        #endregion

        #region isProjectOwner function test.
        [TestMethod]
        public void TestIsProjectOwner1()
        {
            // Arrange: 
            const string username = "Palli";
            const int projectID = 4;

            // Act:
            var result = _userService.isProjectOwner(username, projectID);

            // Assert:
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsProjectOwner2()
        {
            // Arrange: 
            const string username = "Palli";
            const int projectID = 1;

            // Act:
            var result = _userService.isProjectOwner(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestIsProjectOwner3()
        {
            // Arrange: 
            const string username = "";
            const int projectID = 0;

            // Act:
            var result = _userService.isProjectOwner(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestIsProjectOwner4()
        {
            // Arrange: 
            const string username = "Made up user";
            const int projectID = 999;

            // Act:
            var result = _userService.isProjectOwner(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestIsProjectOwner5()
        {
            // Arrange: 
            const string username = "Made up user";
            const int projectID = Int32.MaxValue;

            // Act:
            var result = _userService.isProjectOwner(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestIsProjectOwner6()
        {
            // Arrange: 
            const string username = "Made up user";
            const int projectID = Int32.MinValue;

            // Act:
            var result = _userService.isProjectOwner(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        #endregion

        #region isProjectMember function test.
        [TestMethod]
        public void TestisProjectMember()
        {
            // Arrange;
            const string username = "Gugga";
            const int projectID = 255;

            // Act:
            var result = _userService.isProjectMember(username, projectID);

            // Assert:
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void TestisProjectMember2()
        {
            // Arrange;
            const string username = "Jon";
            const int projectID = 15;

            // Act:
            var result = _userService.isProjectMember(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestisProjectMember3()
        {
            // Arrange;
            const string username = "";
            const int projectID = 0;

            // Act:
            var result = _userService.isProjectMember(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestisProjectMember4()
        {
            // Arrange;
            const string username = "Made up user";
            const int projectID = 999;

            // Act:
            var result = _userService.isProjectMember(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestisProjectMember5()
        {
            // Arrange;
            const string username = "Made up user";
            const int projectID = Int32.MaxValue;


            // Act:
            var result = _userService.isProjectMember(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestisProjectMember6()
        {
            // Arrange;
            const string username = "Made up user";
            const int projectID = Int32.MinValue;

            // Act:
            var result = _userService.isProjectMember(username, projectID);

            // Assert:
            Assert.IsFalse(result);
        }
        #endregion

        #region isProjectMemberInAnyProject function test.
        [TestMethod]
        public void TestIsProjectMemberInAnyProject1()
        {
            // Arrange:
            const int memberID = 8;

            // Act:
            bool result = _userService.isProjectMemberInAnyProject(memberID);

            // Assert:
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsProjectMemberInAnyProject2()
        {
            // Arrange:
            const int memberID = 20;

            // Act:
            bool result = _userService.isProjectMemberInAnyProject(memberID);

            // Assert:
            Assert.IsFalse(result);
        }
        

        [TestMethod]
        public void TestIsProjectMemberInAnyProject3()
        {
            // Arrange:
            const int memberID = 0;

            // Act:
            bool result = _userService.isProjectMemberInAnyProject(memberID);

            // Assert:
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsProjectMemberInAnyProject4()
        {
            // Arrange:
            const int memberID = -4;

            // Act:
            bool result = _userService.isProjectMemberInAnyProject(memberID);

            // Assert:
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestIsProjectMemberInAnyProject5()
        {
            // Arrange:
            const int memberID = 1000000;

            // Act:
            bool result = _userService.isProjectMemberInAnyProject(memberID);

            // Assert:
            Assert.IsFalse(result);
        }
        #endregion

        #region deleteProjectMemberByUserNameAndProjectID functions tests.
        [TestMethod]
        public void TestDeleteProjectMemberByUserNameAndProjectID1()
        {
            // Arrange:
            const int projectID = 2;
            const string userName = "Armann";

            // Act:
            bool check1 = _userService.isProjectMember(userName, projectID);
            _userService.deleteProjectMemberByUserNameAndProjectID(userName, projectID);
            bool check2 = _userService.isProjectMember(userName, projectID);

            // Assert:
            Assert.AreNotEqual(check1, check2);
        }
        #endregion

        #region getOwnerName function.
        [TestMethod]
        public void TestGetOwnerName1()
        {
            // Arrange:
            const int projectID = 1;

            // Act:
            string result = _userService.getOwnerName(projectID);

            // Assert:
            string expected = "David";

            Assert.AreEqual(expected, result);
        }
        #endregion      
    }
}
