using Codebucket.Models.Entities;
using Codebucket.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Models.ViewModels
{
    public class ProjectFileViewModel
    {
        public int _id { get; set; }
        
        public IEnumerable<Project> project { get; set; }
		public IEnumerable<SelectListItem> projects { get; set; }
        public ProjectFile _projectFile { get; set; }
        
        [IfProjectFileExists]
        [Display(Name = "File name")]
        [Required]
        public string _projectFileName { get; set; }
        
        [Required]
        [Display(Name = "Type")]
        public string _projectFileType { get; set; }

        public string _aceExtension { get; set; }

        [Required]
        [Display(Name ="Data")]
        public string _projectFileData { get; set; }
        public int _projectID { get; set; }
    }
}