using Hortifruti.Entidades;

namespace Hortifruti.Repository
{
    public static class UsersRepository
    {
        public static List<User> GetUsers()
        {
            return new List<User>
            {
                new User("1", "pedro", "pedro123", Role.GERENTE),
                new User("2", "daiane", "daiane123", Role.CAIXA),
                new User("3", "daniel", "daniel123", Role.ESTOQUISTA),
            };
        }
    }
}
