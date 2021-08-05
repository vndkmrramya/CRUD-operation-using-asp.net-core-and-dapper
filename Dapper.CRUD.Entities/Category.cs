using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.CRUD.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        
        [Column(TypeName ="varchar(250)")]
        public string Name { get; set; }
    }
}
