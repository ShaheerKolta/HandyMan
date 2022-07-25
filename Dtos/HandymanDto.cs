using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HandyMan.Models;
using Microsoft.EntityFrameworkCore;

namespace HandyMan.Dtos
{
    public class HandymanDto
    {
        [Key]
        public int Handyman_SSN { get; set; } // is required ??
        [Required]
        [StringLength(50)]
        public string Handyman_Name { get; set; }
        
        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        [StringLength(11)]
        public string Handyman_Mobile { get; set; }

        
        [Required]
        public int Handyman_Fixed_Rate { get; set; }
        public bool? Approved { get; set; }
        public bool? Open_For_Work { get; set; }
        [Unicode(false)]
        public string? Handyman_Photo { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Handyman_ID_Image { get; set; }
        [Unicode(false)]
        public string? Handyman_Criminal_Record { get; set; }
        [StringLength(16,MinimumLength = 8)]
        [Unicode(false)]
        [Required]
        public string Password { get; set; }

        public virtual Craft Craft { get; set; }
        
        public virtual ICollection<Request>? Requests { get; set; }
        
        public virtual ICollection<Schedule>? Schedules { get; set; }

        
        public virtual ICollection<Region>? Regions { get; set; }
    }
}
