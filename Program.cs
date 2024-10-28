using Hortifruti.Entidades;
using Hortifruti.Repository;
using System.Globalization;

namespace Hortifruti
{
    class Program
    {
        public static List<Product> products = ProductsRepository.GetProducts();

        static void Main(string[] args)
        {
            User loggedUser = Authentication.Login();

            DisplayMenu(loggedUser);
        }

        static void DisplayMenu(User loggedUser)
        {
            string operacao = null;
            while(operacao != "0") {
                Helpers.DisplayHeader($"             MENU DE OPERACOES");
                if(loggedUser.Role == Role.GERENTE || loggedUser.Role == Role.CAIXA)
                    Console.WriteLine("(1) - Caixa");

                Console.WriteLine("(2) - Estacao de Pesagem");

                if (loggedUser.Role == Role.GERENTE || loggedUser.Role == Role.ESTOQUISTA)
                    Console.WriteLine("(3) - Estoque");

                if (loggedUser.Role == Role.GERENTE)
                    Console.WriteLine("(4) - Doacao de Produtos");

                Console.WriteLine("(0) - Sair");
                Console.Write("Selecione uma Opcao: ");
                operacao = Console.ReadLine();
                

                switch (operacao)
                {
                    case "1":
                        Console.WriteLine("Caixa");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("Estacao de Pesagem");
                        Console.ReadKey();
                        break;
                    case "3":
                        ManageStock();
                        break;
                    case "4":
                        Console.WriteLine("Doacao de Produtos");
                        Console.ReadKey();
                        break;
                    case "0":
                        Console.WriteLine("Obrigado por acessar nosso Hortifruti!");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opcao selecionada e invalida!");
                        Console.WriteLine("Tente novamente ou digite 'sair' para sair.");
                        Helpers.Exit();
                        break;
                }
            }
        }

        public static void ManageStock()
        {
            string operacao = null;
            while (operacao != "0")
            {
                Helpers.DisplayHeader($"           Gerenciador de Estoque");
                Console.WriteLine("(1) - Listar todos os produtos");
                Console.WriteLine("(2) - Adicionar produto ao estoque");
                Console.WriteLine("(3) - Atualizar produto do estoque");
                Console.WriteLine("(4) - Remover produto do estoque");
                Console.WriteLine("(0) - Voltar");
                Console.Write("Selecione uma Opcao: ");
                operacao = Console.ReadLine();

                switch (operacao)
                {
                    case "1":
                        ListarProdutos();
                        Console.ReadKey();
                        break;
                    case "2":
                        AdicionarProduto();
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.WriteLine("Atualizar produto do estoque");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.WriteLine("Remover produto do estoque");
                        Console.ReadKey();
                        break;
                    case "0":
                        break;
                    default:
                        Console.WriteLine("Opcao selecionada e invalida!");
                        Console.WriteLine("Tente novamente ou digite 'sair' para sair.");
                        Helpers.Exit();
                        break;
                }
            }
        }

        public static void ListarProdutos()
        {
            Helpers.DisplayHeader($"            PRODUTOS EM ESTOQUE");
            products.ForEach((product) =>
            {
                Console.WriteLine(product);
            });
        }

        public static void AdicionarProduto()
        {
            Helpers.DisplayHeader($"          ADICIONAR PRODUTO AO ESTOQUE");

            Console.Write("Id: ");
            string id = Console.ReadLine();

            Console.Write("Nome: ");
            string name = Console.ReadLine();

            decimal price;
            while (true)
            {
                Console.Write("Preço (ex: 19.99): ");
                string input = Console.ReadLine();
                input = input.Replace(',', '.');

                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out price)
                    && price > 0) break;

                Console.WriteLine("Preço inválido. Por favor, insira um valor válido.");
            }

            decimal quantity;
            while (true)
            {
                Console.Write("Quantidade: ");
                string input = Console.ReadLine();
                input = input.Replace(',', '.');

                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out quantity)
                    && quantity > 0) break;

                Console.WriteLine("Quantidade inválido. Por favor, insira um valor válido.");
            }

            UnitOfMeasure unitOfMeasure = default;
            while(true)
            {
                Console.Write("Unidade de medida (Unidades/Kg): ");
                string input = Console.ReadLine();

                if (Enum.TryParse(input, true, out unitOfMeasure) 
                    && Enum.IsDefined(typeof(UnitOfMeasure), unitOfMeasure)) break;

                Console.WriteLine("Unidade de medida inválida. Por favor, insira 'Unidades' ou 'Kg'.");
            }

            DateTime expireDate;
            while (true)
            {
                Console.Write("Data de validade (ex: 30/12/2024): ");
                string input = Console.ReadLine();

                if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, DateTimeStyles.None, out expireDate) && expireDate > DateTime.Now) break;
                
                Console.WriteLine("Data inválida. Por favor, insira no formato 'dd/MM/yyyy'");
            }

            Product product = new Product(name, price, quantity, unitOfMeasure, expireDate, id: id);
            products.Add(product);
        }
    }
}