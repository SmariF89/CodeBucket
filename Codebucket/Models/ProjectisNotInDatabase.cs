using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Codebucket.Models.Entities;
using Codebucket.Services;
using Codebucket.Models.ViewModels;

namespace Codebucket.Models
{
    public class ProjectisNotInDatabase : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProjectService _service = new ProjectService();
            string data = value as string;

            if(data == "")
            {
                return new ValidationResult("Project Name is required!");
            }

            else if (_service.createNewProjectIsValid(data))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("This project already exists!!!!!");
            }
        }
    }
}