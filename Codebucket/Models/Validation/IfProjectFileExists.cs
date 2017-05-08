using Codebucket.Models.ViewModels;
using Codebucket.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Codebucket.Models.Validation
{
    public class IfProjectFileExists : ValidationAttribute
    {
        private ProjectFileService _projectFileService = new ProjectFileService();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var projectFile = (CreateProjectFileViewModel)validationContext.ObjectInstance;

            if (projectFile._projectFileName == null)
            {
                return new ValidationResult("File name is required!");
            }
            
            if (!_projectFileService.projectFileExists(projectFile._projectFileName, projectFile._projectID))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("A file with this name is already in Project!");
            }
        }
    }
}