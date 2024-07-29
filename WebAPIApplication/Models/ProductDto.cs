using System.ComponentModel.DataAnnotations;

namespace WebAPIApplication.Models
{
    public class ProductDto
    {
        [Required]
        [MinLength(2,ErrorMessage ="name trop court !!")]
        public string Name { get; set; } = "";
        [Required]
        [MinLength(2, ErrorMessage = "brand trop court !!")]
        public string Brand {  get; set; } = "";
        [Required]
        public string Category { get; set; } = "";
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = "";
    }
}
