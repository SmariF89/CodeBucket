using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebucket.Models.ViewModels
{
    public class ProjectViewModel
    {
        public string _projectName { get; set; }
        public List<ProjectFileViewModel> _projectFiles { get; set; }
        public List<ApplicationUserViewModel> _projectMembers { get; set; }
    }
}