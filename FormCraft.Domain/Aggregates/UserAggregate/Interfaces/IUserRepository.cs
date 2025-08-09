namespace FormCraft.Domain.Aggregates.UserAggregate.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByRefreshTokenAsync(string refreshToken);
        Task<User> FindById(Guid userId);
        Task AddAsync(User user);
    }
}
