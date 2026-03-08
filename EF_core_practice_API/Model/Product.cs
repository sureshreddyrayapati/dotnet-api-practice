using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EF_core_practice_API.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }= string.Empty;

        public int Stock {  get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
