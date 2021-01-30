using Microsoft.AspNetCore.Mvc.Rendering;
using Ravid.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ravid.Dtos
{


    public class UserDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [EmailAddress(ErrorMessage = "יש לרשום כתובת אימייל חוקית")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "אימייל")]
         
        public string Email { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "סיסמה")]
        public string Password { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "טלפון 1")]
        public string Phone1 { get; set; }

        [Display(Name = "טלפון 2")]
        public string Phone2 { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "חברה")]
        public string Company { get; set; }

        [Display(Name = "הערה")]
        public string Comment { get; set; }

         
        [Display(Name = "תפקיד")]
        public List<Role> RoleList { get; set; }

        [Display(Name = "תפקיד")]
        public string RoleId { get; set; }
    }
}
