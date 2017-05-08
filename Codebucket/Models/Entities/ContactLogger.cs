using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.Entities
{
    public class ContactLogger
    {
        [Key]
        public int ID { get; set; }
        public string _contactName { get; set; }
        public string _contactEmail { get; set; }
        public string _ContactMessage { get; set; }
    }
}