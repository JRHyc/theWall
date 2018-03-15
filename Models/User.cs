using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace wall.Models
{

    public abstract class BaseEntity{}

    public class User : BaseEntity
    {
        [Required]
        [MinLength(2, ErrorMessage="First name must contain at least two letters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        public string FirstName {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Last name must contain at least two letters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8, ErrorMessage="Password must contain at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Password and confirmation must match")]
        public string Confirm {get;set;}
    }

    public class LogUser : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password {get;set;}
    }

}