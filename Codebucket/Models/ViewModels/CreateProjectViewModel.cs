using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Models.ViewModels
{
    public class CreateProjectViewModel
    {
        [Required]
        [Display(Name ="Project Name")]
        public string _projectName { get; set; }

        public List<SelectListItem> _projectType { get; set; }

        [Required]
        public int _projectTypeId { get; set; }
    }
}