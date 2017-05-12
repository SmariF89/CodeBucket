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
        private UserService _userService = new UserService(null);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            AddMemberViewModel member = (AddMemberViewModel)validationContext.ObjectInstance;

            if(member._userName == null)
            {
                return new ValidationResult("Username is required!");
            }
            else if(_userService.isProjectMember(member._userName, member._projectID))
            {
                return new ValidationResult("This user is already in this project!");
            }
            else if (_userService.isProjectOwner(member._userName, member._projectID))
            {
                return new ValidationResult("You are already an owner of this project!");
            }
            else if(_userService.userIsInDataBase(member._userName))
            {
                return ValidationResult.Success;
            }
            
            else
            {
                return new ValidationResult("Username does not exist!");
            }
        }
    }
}