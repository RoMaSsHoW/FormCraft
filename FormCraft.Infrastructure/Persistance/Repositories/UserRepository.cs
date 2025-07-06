using FormCraft.Domain.Aggregates.UserAggregate;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace FormCraft.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FormCraftDbContext _dbContext;

        public UserRepository(FormCraftDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> FindByEmailAsync(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new ArgumentException("Email address cannot be null or whitespace.", nameof(emailAddress));

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email.EmailAddress == emailAddress);

            return user;
        }

        public User FindById(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException($"User Id cannot be null", nameof(userId));

            var user = _dbContext.Users
                .FirstOrDefault(u => u.Id == userId);

            return user;
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }
    }
}
