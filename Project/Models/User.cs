using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ravid.Models
{
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }


        [MaxLength(50)]
        [Required(ErrorMessage = "שדה חובה")]
        [EmailAddress(ErrorMessage = "יש לרשום כתובת אימייל חוקית")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "אימייל")]
        public string Email { get; set; }

        [Required]        
        [Column(TypeName = "char(64)")]
        [Display(Name = "סיסמה")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "טלפון 1")]
        public string Phone1 { get; set; }

        [Column(TypeName = "varchar(20)")]
        [Display(Name = "טלפון 2")]
        public string Phone2 { get; set; }

        [Required]
        [Display(Name = "נוצר בתאריך")]
        public DateTime DateRegistered { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [MaxLength(300)]
        [Display(Name = "חברה")]
        public string Company { get; set; }

        [Display(Name = "הערה")]
        [Column(TypeName = "ntext")]
        public string Comment { get; set; }


        public virtual ICollection<UserInRole> UserInRoles { get; set; }
        public virtual ICollection<TrasportRequest> TrasportRequest { get; set; }



    }
}
