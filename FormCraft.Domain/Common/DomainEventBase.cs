namespace FormCraft.Domain.Common
{
    public abstract class DomainEventBase : IDomainEvent
    {
        protected DomainEventBase()
        {
            OccuredOn = DateTime.UtcNow;
        }

        public DateTime OccuredOn { get; protected set; }
    }
}
