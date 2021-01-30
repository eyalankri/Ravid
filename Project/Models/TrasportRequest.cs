using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ravid.Models
{
    public class TrasportRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TrasportRequestId { get; set; }

        [Required]
        [Display(Name = "לתאריך")]       
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ForDate { get; set; }


        [Display(Name = "הערה")]
        [Column(TypeName = "ntext")]
        public string Comment { get; set; }

        [Display(Name = "מספר משטחים")]   
        public int NumberOfPlates { get; set; }

        
        [Display(Name = "משלוח אל")]
        [Column(TypeName = "nvarchar(400)")]
        public string DeliveryFor { get; set; }


        [Display(Name = "נוצרה בתאריך")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "סטטוס")]
        public string TrasportRequestStatus { get; set; }

        public Guid UserId { get; set; }



    }
}
