using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceFactory.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Item")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Image { get; set; }

        [DisplayName("Spice Level")]
        public ESpicy SpiceLevel { get; set; }
        
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        [DisplayName("Sub-category")]
        public virtual SubCategory SubCategory { get; set; }


        [Range(1, 100, ErrorMessage = "Price must be between 1 to 100 £.")]
        public double Price { get; set; }
        public enum ESpicy
        {
            NA = 0,
            Mild = 1,
            Hot = 2,
            VeryHot = 3
        }

    }
}
