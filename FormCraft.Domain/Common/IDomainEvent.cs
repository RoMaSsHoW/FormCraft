namespace FormCraft.Domain.Common
{
    public interface IDomainEvent
    {
        DateTime OccuredOn { get; }
    }
}
