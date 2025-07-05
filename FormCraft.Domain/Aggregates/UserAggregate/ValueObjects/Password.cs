using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Domain.Aggregates.UserAggregate.ValueObjects
{
    public class Password : ValueObject
    {
        public Password() { }

        private Password(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public string PasswordHash { get; }

        public static Password Create(string password, IPasswordHasher passwordHasher)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password cannot be null or whitespace.");

            var passwordHash = passwordHasher.Hash(password);
            return new Password(passwordHash);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return PasswordHash;
        }
    }
}
