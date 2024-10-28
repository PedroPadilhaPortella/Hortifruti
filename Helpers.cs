namespace Hortifruti
{
    public static class Helpers
    {
        public static void DisplayHeader(string message = null)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║         Bem-vindo ao Hortifruti!       ║");
            if (message != null) Console.WriteLine(message);
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
        }

        public static void Exit()
        {
            Console.Write("_ ");
            string sair = Console.ReadLine();
            if (sair == "sair")
            {
                Console.WriteLine("\nObrigado por acessar nosso Hortifruti!");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
    }
}
