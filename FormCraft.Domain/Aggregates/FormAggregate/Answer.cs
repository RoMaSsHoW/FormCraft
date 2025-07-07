using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using FormCraft.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormCraft.Domain.Aggregates.FormAggregate
{
    public abstract class Answer : Entity
    {
        protected Answer() { }

        public Guid QuestionId { get; protected set; }
        public Guid AuthorId { get; protected set; }

        [NotMapped]
        public abstract QuestionType Type { get; }

        protected bool UserIsAuthorOrAdmin(ICurrentUserService currentUserService)
        {
            var userId = currentUserService.GetUserId();
            var userRole = currentUserService.GetRole();

            if (userId != Guid.Empty && !string.IsNullOrEmpty(userRole))
            {
                return userId == AuthorId || Role.FromName<Role>(userRole) == Role.Admin;
            }

            throw new UnauthorizedAccessException("User unauthorized");
        }
    }
}
