using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileLoad.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Enter first name")]
       
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter Last name")]
        public string LastName { get; set; }
        [Display(Name = "Office Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
          ErrorMessage = "Invalid email format")]
        [Required(ErrorMessage = "Enter Email ID")]
        public string Email { get; set; }
        
    }
}
