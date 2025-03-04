using mysql_net_core_api.Core.Enums;

namespace mysql_net_core_api.Core.Entitites
{
    public class CategoryEntity
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public CategoryEntity ParentCategory { get; set; }
        public CategoryGenderEnum CategoryGender { get; set; }

        public ICollection<ProductEntity> Products { get; set; } 
    }
}
