using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceFactory.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ECoupon Type { get; set; }

        public enum ECoupon { Percent, Amount }

        [Required]
        public double Discount{ get; set; }

        public double MinimumAmount { get; set; }

        public byte[] Image { get; set; }

        public bool IsActive { get; set; }

    }
}
