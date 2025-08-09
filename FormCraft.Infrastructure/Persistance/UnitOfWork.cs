using FormCraft.Application.Common.Persistance;
using FormCraft.Domain.Common;
using MassTransit;

namespace FormCraft.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FormCraftDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public UnitOfWork(
            FormCraftDbContext dbContext, 
            IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = _dbContext.ChangeTracker
                .Entries<Entity>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            foreach (var entry in _dbContext.ChangeTracker.Entries<Entity>())
                entry.Entity.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
                await _publishEndpoint.Publish(domainEvent.GetType(), domainEvent, cancellationToken);

            return result > 0;
        }
    }
}
