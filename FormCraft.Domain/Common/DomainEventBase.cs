namespace FormCraft.Domain.Common;

public class DomainEventBase : IDomainEvent
{
    protected DomainEventBase()
    {
        OccuredOn = DateTime.Now;
    }
    
    public DateTime OccuredOn { get; }
}