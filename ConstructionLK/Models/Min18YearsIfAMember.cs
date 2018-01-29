using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionLK.Models
{
    public class Min18YearsIfAMember:ValidationAttribute    
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var serviceProvider = (ServiceProvider)validationContext.ObjectInstance;
            if (serviceProvider.TypeId==2||serviceProvider.MembershipTypeId == MembershipType.Unknown || serviceProvider.MembershipTypeId == MembershipType.PayAsYouGo)
            {
                return ValidationResult.Success;
            }
            if (serviceProvider.DateOfBirth == null)
            {
                return new ValidationResult("Date of Birth is Required.");
            }
            var age = DateTime.Today.Year - serviceProvider.DateOfBirth.Value.Year;

            return (age >= 18) ? ValidationResult.Success : new ValidationResult("You should be at least 18 years old");
        }   
    }
}