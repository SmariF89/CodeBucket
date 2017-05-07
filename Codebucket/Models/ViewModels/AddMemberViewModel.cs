using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.ViewModels
{
    public class AddMemberViewModel
    {
        [Required]
        [Display(Name ="Username")]
        public string _userName  { get; set; }
        public string _project { get; set; }
        public int _projectID { get; set; }
    }
}