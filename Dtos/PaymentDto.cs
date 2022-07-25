using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HandyMan.Models;
using Microsoft.EntityFrameworkCore;

namespace HandyMan.Dtos
{
    public class PaymentDto
    {
        public int Payment_ID { get; set; }

        public int Request_ID { get; set; }
        public bool? Payment_Status { get; set; }
        public bool Method { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Payment_Date { get; set; }
        public int Payment_Amount { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Transaction_ID { get; set; }
    }
}
