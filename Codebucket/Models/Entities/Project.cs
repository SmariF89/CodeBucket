using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.Entities
{
    public class Project
    {
        [Key]
        public int ID { get; set; }
        public string _projectName { get; set; }
    }
}