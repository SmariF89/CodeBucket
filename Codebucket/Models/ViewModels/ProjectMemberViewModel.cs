using Codebucket.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebucket.Models.ViewModels
{
    public class ProjectMemberViewModel
    {
        public int _projectID { get; set; }

        public int _id { get; set; }

        public string _userName { get; set; }          
    }
}