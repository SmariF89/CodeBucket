using Codebucket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Codebucket.Models.ViewModels
{
    public class ProjectViewModel
    {
        public string _projectName { get; set; }
        public int _id { get; set; }
        public List<SelectListItem> _projectType { get; set; }
        public List<ProjectFileViewModel> _projectFiles { get; set; }
        public List<ProjectMember> _projectMembers { get; set; }
        public int _projectTypeId { get; set; }
        public bool _isProjectOwner { get; set; }
    }
}