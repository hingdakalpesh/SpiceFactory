using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpiceFactory.Models;

namespace SpiceFactory.Areas.Customer.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Coupon> Coupons { get; set; }
    }
}
