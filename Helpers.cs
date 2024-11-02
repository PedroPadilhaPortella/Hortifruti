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

        public static bool PaymentProcessment()
        {
            Console.Write("\nQual será a forma de pagamento?\n Aceitamos Pix / Credito / Debito / VA / VR: ");
            string paymentMethod = Console.ReadLine();

            // TODO: process payments
            switch(paymentMethod)
            {
                case "Pix":
                    Console.WriteLine("Pix");
                    break;
                case "Credito":
                case "Debito":
                case "VA":
                case "VR":
                    Console.WriteLine("Outros");
                    break;
            }

            return true;
        }
    }
}
