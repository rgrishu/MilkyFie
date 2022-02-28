using System;
using System.ComponentModel.DataAnnotations;

namespace Milkyfie.Models
{
    public class DemoViewModel
    {
        [Required(ErrorMessage = "First Name can't be empty.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name can't be empty.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address can't be empty.")]
        [EmailAddress(ErrorMessage = "Please enter proper email address.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Date of Birth Name can't be empty.")]
        public DateTime? DateOfBirth { get; set; }
    }
}
