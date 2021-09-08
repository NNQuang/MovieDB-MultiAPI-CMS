using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Areas.Movie.Models
{
    public class UserRegisterModel
    {
        [Required, MinLength(5, ErrorMessage ="Username name must be greater than 4 characters.")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare("Password",
            ErrorMessage = "The password you entered do not match"),
            Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
}
