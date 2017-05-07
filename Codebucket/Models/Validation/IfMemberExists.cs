using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.Validation
{
    public class IfMemberExists : ValidationAttribute
    {
        // 
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            return null;


        }
    }
}