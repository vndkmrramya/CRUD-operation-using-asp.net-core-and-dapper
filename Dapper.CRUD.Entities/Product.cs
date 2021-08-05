using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.CRUD.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "varchar(250)")]
        [Required]
        public string Description { get; set; }
        public decimal? UnitPrice { get; set; }

        //foreign key
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
