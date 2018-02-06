namespace Domain.Seedwork
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }
    }
}