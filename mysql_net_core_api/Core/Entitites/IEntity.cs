namespace mysql_net_core_api.Core.Entitites
{
    public interface IEntity<Tid>
    {
        public Tid Id { get; set; }
    }
}
