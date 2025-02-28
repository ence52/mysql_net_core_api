namespace mysql_net_core_api.Core.Entitites
{
    public class Category:IEntity<int>
    {
        public int Id { get; set; } 

        public string Name { get; set; } = string.Empty;

        public List<Product> Products { get; set; } = new();
    }
}
