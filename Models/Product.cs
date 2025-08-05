using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECart.Models
{
    [Table("tbl_products")]
    public class Product
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[Required]
        public string? ProductName { get; set; }
        //[Required]
        public int Quantity { get; set; }
        //[Required]
        public float Price { get; set; }
    }
}