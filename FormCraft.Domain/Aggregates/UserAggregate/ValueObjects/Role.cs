using FormCraft.Domain.Common;

namespace FormCraft.Domain.Aggregates.UserAggregate.ValueObjects
{
    public class Role : Enumeration
    {
        public static readonly Role Admin = new(1, nameof(Admin));
        public static readonly Role User = new(2, nameof(User));

        public Role(int id, string name) : base(id, name)
        {
        }
    }
}
