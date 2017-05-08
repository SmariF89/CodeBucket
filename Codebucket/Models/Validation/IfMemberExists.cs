using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Codebucket.Services;
using Codebucket.Models.ViewModels;

namespace Codebucket.Models.Validation
{
    public class IfMemberExists : ValidationAttribute
    {
        private ProjectFileService _projectFileService = new ProjectFileService();
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            AddMemberViewModel member = (AddMemberViewModel)validationContext.ObjectInstance;

            if(member._userName == null)
            {
                return new ValidationResult("User name is required!");
            }
            else if(_projectFileService.isProjectMember(member._userName, member._projectID))
            {
                return new ValidationResult("This user is already in this project!");
            }
            else if(_projectFileService.userIsInDataBase(member._userName))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Member does not exist!");
            }
        }
    }
}