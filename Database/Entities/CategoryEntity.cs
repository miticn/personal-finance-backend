using Transaction.Database.Entities;

namespace Finance.Models
{
    public class CategoryEntity
    {
        public string Code { get; set; }
        public string? ParentCode { get; set; }
        public string Name { get; set; }

        public CategoryEntity Parent { get; set; }
        public ICollection<CategoryEntity> Children { get; set; }
    }
}
