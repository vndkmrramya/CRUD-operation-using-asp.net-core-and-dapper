using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? UnitPrice { get; set; }
        public int CategoryId { get; set; }

        
        [Column("Name")]
        public string Category { get; set; }
    }
}
