using FormCraft.Domain.Aggregates.UserAggregate;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public UserRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> FindByEmail(string emaiAddress)
        {
            var email = new Email(emaiAddress);

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }
    }
}
