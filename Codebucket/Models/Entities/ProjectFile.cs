using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.Entities
{
    public class ProjectFile
    {
        [Key]
        public int ID { get; set; }
        public int  _projectID { get; set; }
        public string _projectFileName { get; set; }
        public string _projectFileType { get; set; }
        public string _projectFileData { get; set; }
    }
}