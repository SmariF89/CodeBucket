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
        public int _id { get; set; }
        public string _projectName { get; set; }
        public int _projectFileTypeId { get; set; }
        public bool _isProjectOwner { get; set; }
        public string _thumbnailUrl { get; set; }
        //public List<SelectListItem> _projectType { get; set; }       //We put this on ice
        public List<ProjectFileViewModel> _projectFiles { get; set; }
        public List<ProjectMemberViewModel> _projectMembers { get; set; }
    }
}