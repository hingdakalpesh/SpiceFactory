using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpiceFactory.Models;
namespace SpiceFactory.Areas.Admin.ViewModels
{
    public class SubCategoryCreateViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public  SubCategory SubCategory { get; set; }
        public List<string> ExistingSubCategories { get; set; }
        public string StatusMessage { get; set; }
    }
}
