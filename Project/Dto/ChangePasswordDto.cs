using System;
using System.ComponentModel.DataAnnotations;

namespace Ravid.Dto
{
    public class changePasswordDto
    {
        [Required]
        public Guid? UserId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
