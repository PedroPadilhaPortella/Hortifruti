using Hortifruti.Entities;
using Hortifruti.Repository;

namespace Hortifruti
{
    public static class Authentication
    {
        public static User Login()
        {
            User loggedUser = null;
            List<User> users = UsersRepository.GetUsers();

            while (loggedUser == null)
            {

                Helpers.DisplayHeader();

                Console.Write("Digite seu nome de Usuário: ");
                string username = Console.ReadLine();

                Console.Write("Digite sua senha: ");
                string password = Console.ReadLine();

                users.ForEach((user) => {
                    if (user.Password == password && user.Name == username)
                    {
                        loggedUser = user;
                    }
                });

                if (loggedUser == null)
                {
                    Console.WriteLine("Nome de Usuário ou Senha inválidos!");
                    Console.WriteLine("Tente novamente ou digite 'sair' para sair.");
                    Helpers.Exit();
                    Console.Clear();
                }
                else
                {
                    Helpers.DisplayHeader($"         Login bem sucedido, {loggedUser.Name}!");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                }
            }
            return loggedUser;
        }
    }
}
