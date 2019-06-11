using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCAppRegister.Models
{
    public class UserViewModel
    {
        [Display(Name ="UserId")]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(maximumLength:30,ErrorMessage ="The {0} must be at least {1} charcters long",MinimumLength =3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


    }
}