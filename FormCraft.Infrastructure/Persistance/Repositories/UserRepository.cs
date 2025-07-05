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

        public async Task<User> FindByEmail(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                throw new ArgumentException("Email address cannot be null or whitespace.", nameof(emailAddress));
            }

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email.EmailAddress == emailAddress);

            return user;
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }
    }
}
