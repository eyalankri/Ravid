using Microsoft.AspNetCore.Mvc.Rendering;
using Ravid.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ravid.Dto
{
    public class TrasportRequestDto
    {
        public Guid TrasportRequestId { get; set; }

        [Required]
        [Display(Name = "לתאריך")]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ForDate { get; set; }

        [Required]
        [Display(Name = "משטחים")]
        public int NumberOfPlates { get; set; }

        [Required]
        [Display(Name = "משלוח אל")]      
        public string DeliveryFor { get; set; }


        [Required]
        [Display(Name = "נפתחה בתאריך")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "הערה")]
        public string Comment { get; set; }

        [Display(Name = "סטטוס")]
        public string TrasportRequestStatus { get; set; }

        [Display(Name = "לקוח")]
        public string Company { get; set; }

        public Guid UserId { get; set; }

        [Display(Name = "תפקיד")]
        public Dictionary<int, string> TrasportRequestStatusDic { get; set; }

      //  public SelectList TrasportRequestStatusList2 { get; set; }
    }
}
