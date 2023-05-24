namespace Host.Api.Core.DomainObjects
{
    public abstract class Entity
    {
        public long Id { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Entity()
        {
            CreatedAt = DateTime.Now;
        }
    }
}