using Hortifruti.Enums;

namespace Hortifruti.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public User(string id, string name, string password, Role role)
        {
            Id = id;
            Name = name;
            Password = password;
            Role = role;
        }

        public override bool Equals(object obj)
        {
            return obj is User user && Id == user.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return $"{Id} - Username: {Name} - Password: ****** - Role: {Role}";
        }
    }
}
