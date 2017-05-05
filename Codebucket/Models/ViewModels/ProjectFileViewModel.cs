using Codebucket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Models.ViewModels
{
    public class ProjectFileViewModel
    {
        public IEnumerable<Project> project { get; set; }

        //public IEnumerable<SelectListItem> projects { get; set; }
        public ProjectFile _projectFile { get; set; }
        public string _projectFileName { get; set; }
        public string _projectFileType { get; set; }
        public string _projectFileData { get; set; }

        public int ProjectID { get; set; }
    }
}