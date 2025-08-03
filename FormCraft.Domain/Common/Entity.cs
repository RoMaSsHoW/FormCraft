﻿namespace FormCraft.Domain.Common
{
    public abstract class Entity
    {
        private List<IDomainEvent> _domainEvents = new ();
        
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (Entity)obj;

            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity? left, Entity? right) 
            => left is null ? right is null : left.Equals(right);

        public static bool operator !=(Entity? left, Entity? right) 
            => !(left == right);
        
        public void ClearDomainEvents() 
            => _domainEvents.Clear();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= [];
            _domainEvents.Add(domainEvent);
        }
    }
}
