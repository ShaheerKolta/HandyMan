using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HandyMan.Models;
using Microsoft.EntityFrameworkCore;

namespace HandyMan.Dtos
{
    public class RegionDto
    {
        public int Region_ID { get; set; }
        [Required]
        [StringLength(30)]
        public string Region_Name { get; set; }

        public virtual ICollection<ClientDto>? Clients { get; set; }

        [ForeignKey("Region_ID")]
        [InverseProperty("Regions")]
        public virtual ICollection<HandymanDto>? Handyman_SSNs { get; set; }
    }
}
