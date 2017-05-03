using Codebucket.Models;
using Codebucket.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebucket.Services
{
    public class ApplicationUserService
    {
        private ApplicationDbContext _db;

        public ApplicationUserService()
        {
            _db = new ApplicationDbContext();
        }

        public ApplicationUserViewModel getApplicationUserById(int? id)
        {
            return null;
        }

        public bool isProjectOwner(int? id)
        {
            return false;
        }

        public bool isProjectMember(int? id)
        {
            return false;
        }
    }
}