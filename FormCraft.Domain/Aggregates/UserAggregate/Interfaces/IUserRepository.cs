namespace FormCraft.Domain.Aggregates.UserAggregate.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        User FindById(Guid userId);
        Task AddAsync(User user);
    }
}
