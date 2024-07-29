using System.ComponentModel.DataAnnotations;

namespace WebAPIApplication.Models
{
    public class ProductDto
    {
        [Required]
        [MinLength(2,ErrorMessage ="trop court !!")]
        public string Name { get; set; } = "";
        [Required]
        public string Brand {  get; set; } = "";
        [Required]
        public string Category { get; set; } = "";
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = "";
    }
}
