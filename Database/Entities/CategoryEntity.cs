namespace Finance.Models
{
    public class CategoryEntity
    {
        public string Code { get; set; }
        public string? ParentCode { get; set; }
        public string Name { get; set; }
        public CategoryEntity ParentCategory { get; set; }
        public ICollection<CategoryEntity> SubCategories { get; set; }
    }
}
