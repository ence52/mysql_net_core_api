namespace mysql_net_core_api.Core.Entitites
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public List<Product> Products { get; set; } = new();
    }
}
