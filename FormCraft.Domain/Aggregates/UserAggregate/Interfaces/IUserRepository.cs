namespace FormCraft.Domain.Aggregates.UserAggregate.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmail(string email);
        Task AddAsync(User user);
    }
}
