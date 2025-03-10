using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entity
{
    public class Category
    {
        [Key]
        public required string CategoryId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        // Navigation property for InCategory (one-to-many)
        public ICollection<InCategory>? InCategories { get; set; }
    }
}