using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Codebucket.Models.ViewModels
{
    public class CreateProjectViewModel
    {
        
        public string _projectName { get; set; }
        public List<SelectListItem> projectType { get; set; }
        public int _projectTypeId { get; set; }
    }
}