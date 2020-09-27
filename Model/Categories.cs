using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NorthwindConsole.Model
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public int CategoryId { get; set; }
        [Required(ErrorMessage = "YO - Enter the name!")]
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
