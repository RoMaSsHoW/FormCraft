namespace FormCraft.Domain.Aggregates.UserAggregate.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
