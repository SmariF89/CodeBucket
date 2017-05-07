using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.ViewModels
{
    public class CreateProjectFileViewModel
    {
        [Required]
        [Display(Name ="File name")]
        public string _projectFileName { get; set; }

        [Display(Name = "File type")]
        public string _projectFileType { get; set; }

        [Display(Name = "File data")]
        public string _projectFileData { get; set; }

        
        public int _projectID { get; set; }
    }
}