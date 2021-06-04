using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Core.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Excerpt { get; set; }
        
        public string Description { get; set; }
        
        public string ImageFile { get; set; }
        
        public decimal Price { get; set; }
        
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

    }
}
