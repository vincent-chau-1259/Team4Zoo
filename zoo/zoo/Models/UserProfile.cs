using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zoo.Models
{
    public class UserProfile

        {

            public string newUsername { get; set; }

            [DataType(DataType.Password)]

            public string oldPassword { get; set; }

            [DataType(DataType.Password)]

            [MaxLength(8)]

            [Required]

            public string newPassword { get; set; }

            public string PwdErrorMessage { get; set; }

            public string UserNameErrorMessage { get; set; }

            public string PhoneErrorMessage { get; set; }

            public string EmailErrorMessage { get; set; }

            public string newEmail { get; set; }

            [Required(ErrorMessage = "Mobile Number is required.")]

            public string newPhone { get; set; }

        }
    }
