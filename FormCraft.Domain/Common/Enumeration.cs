using System.Reflection;

namespace FormCraft.Domain.Common
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

        public static T FromId<T>(int id) where T : Enumeration =>
            Parse<T, int>(id, e => e.Id == id, "id");

        public static T FromName<T>(string name) where T : Enumeration =>
            Parse<T, string>(name, e => e.Name == name, "name");

        private static T Parse<T, K>(K value, Func<T, bool> predicate, string valueType) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);
            if (matchingItem is null)
                throw new InvalidOperationException($"'{value}' is not a valid {valueType} for {typeof(T).Name}");
            return matchingItem;
        }
    }
}
