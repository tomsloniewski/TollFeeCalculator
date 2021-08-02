using System;
using System.ComponentModel.DataAnnotations;

namespace TollFeeCalculator.Models
{
    public class TollPay
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Plate { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
