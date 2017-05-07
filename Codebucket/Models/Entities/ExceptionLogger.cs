using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.Entities
{
    public class ExceptionLogger
    {
        [Key]
        public int ID { get; set; }
        public string _userName { get; set; }
        public string _exceptionMessage { get; set; }
        public string _controllerName { get; set; }
        public string _actionName { get; set; }
        public string _exceptionStackTrace { get; set; }
        public DateTime _logTime { get; set; }    
    }
}