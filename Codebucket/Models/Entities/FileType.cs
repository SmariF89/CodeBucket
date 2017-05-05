using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.Entities
{
    public class FileType
    {
        [Key]
        public int ID { get; set; }
        public string _description { get; set; }
        public string _extension { get; set; }
    }
}