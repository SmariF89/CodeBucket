using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.ViewModels
{
    public class ConctactLogViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string _contactName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string _contactEmail { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string _contactMessage { get; set; }

    }
}