using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebucket.Models.ViewModels
{
    public class CreateProjectFileViewModel
    {
        public string _projectFileName { get; set; }
        public string _projectFileType { get; set; }
        public string _projectFileData { get; set; }
        public int _projectID { get; set; }
    }
}