using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Fillager.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(256, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The 'Password' and 'Confirm Password' fields must match")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        
        public List<IdentityError> Errors { get; set; }
    }
}
