using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebucket.Models.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string _applicationUserName { get; set; }
        public List<ProjectViewModel> _applicationUserProjects { get; set; }
    }
}