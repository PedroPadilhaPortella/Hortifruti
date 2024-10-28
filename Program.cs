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
                        WeighProducts();
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
                        Console.WriteLine("\nObrigado por acessar nosso Hortifruti!");
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

        static void WeighProducts()
        {
            Helpers.DisplayHeader($"        ESTACAO DE PESAGEM");

            Console.Write("\nConfira o peso e o preço dos seus produtos\n");

            Console.Write("Id: ");
            string id = Console.ReadLine();
            Product product = products.FirstOrDefault(p => p.Id == id);

            if (product == null) {
                Console.WriteLine("Produto nao encontrado.");
                return;
            }

            if (product.UnitOfMeasure == UnitOfMeasure.Unidades)
            {
                Console.WriteLine("Este produto e medido em unidades, nao faz sentido em pesa-lo.");
                return;
            }

            Console.WriteLine(product.Name);

            decimal weight;
            while (true)
            {
                Console.Write("Peso registrado na balanca: ");
                string input = Console.ReadLine();
                input = input.Replace(',', '.');

                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out weight)) break;

                Console.WriteLine("Peso inválido.");
            }

            Console.WriteLine(product.GetPrice(weight));
        }

        static void ManageStock()
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
                        AtualizarProduto();
                        Console.ReadKey();
                        break;
                    case "4":
                        RemoverProduto();
                        Console.ReadKey();
                        break;
                    case "0":
                        break;
                    default:
                        Console.WriteLine("\nOpcao selecionada e invalida!");
                        Console.WriteLine("Tente novamente ou digite 'sair' para sair.");
                        Helpers.Exit();
                        break;
                }
            }
        }

        static void ListarProdutos()
        {
            Helpers.DisplayHeader($"            PRODUTOS EM ESTOQUE");
            products.ForEach((product) =>
            {
                Console.WriteLine(product);
            });
        }

        static void AdicionarProduto()
        {
            Helpers.DisplayHeader($"        ADICIONAR PRODUTO AO ESTOQUE");

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

            UnitOfMeasure unitOfMeasure;
            while (true)
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

            Product product = new Product(name, price, quantity, unitOfMeasure, expireDate);
            products.Add(product);

            Console.WriteLine("Produto adicionado com sucesso!");
        }

        static void AtualizarProduto()
        {
            Helpers.DisplayHeader($"        ATUALIZAR PRODUTO DO ESTOQUE");

            Console.Write("Id: ");
            string id = Console.ReadLine();
            Product productToUpdate = products.FirstOrDefault(p => p.Id == id);

            if (productToUpdate == null) {
                Console.WriteLine("Produto nao encontrado.");
                return;
            }

            Console.Write("Nome (deixe vazio para nao alterar): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name)) {
                productToUpdate.Name = name;
            }

            Console.Write("Preco (deixe vazio para nao alterar): ");
            string priceInput = Console.ReadLine();
            if (decimal.TryParse(priceInput.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price)) {
                productToUpdate.Price = price;
            }

            Console.Write("Quantidade (deixe vazio para nao alterar): ");
            string quantityInput = Console.ReadLine();
            if (int.TryParse(quantityInput, out int quantity)) {
                productToUpdate.Quantity = quantity;
            }

            Console.Write("Unidade de medida (Unidades/Kg) (deixe vazio para não alterar): ");
            string unitInput = Console.ReadLine();
            if (Enum.TryParse(unitInput, true, out UnitOfMeasure unit) 
                && Enum.IsDefined(typeof(UnitOfMeasure), unit)) {
                productToUpdate.UnitOfMeasure = unit;
            }

            Console.Write("Data de validade (ex: 30/12/2024) (deixe vazio para não alterar): ");
            string expireDateInput = Console.ReadLine();
            if (DateTime.TryParseExact(expireDateInput, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime expireDate)) {
                productToUpdate.ExpireDate = expireDate;
            }

            Console.WriteLine("Produto atualizado com sucesso!");
            Console.ReadKey();
        }

        static void RemoverProduto()
        {
            Helpers.DisplayHeader($"          REMOVER PRODUTO DO ESTOQUE");

            Console.Write("Id: ");
            string id = Console.ReadLine();
            Product productToRemove = products.FirstOrDefault(p => p.Id == id);

            if (productToRemove == null)  {
                Console.WriteLine("Produto nao encontrado.");
                return;
            }

            Console.WriteLine(productToRemove);

            Console.Write("Deseja realmente remover este produto? (sim/nao): ");
            string resposta = Console.ReadLine()?.Trim().ToLower();

            if (resposta == "sim") {
                products.Remove(productToRemove);
                Console.WriteLine("Produto removido com sucesso!");
            }
            else {
                Console.WriteLine("Operação de remoção cancelada.");
            }

            Console.ReadKey();
        }
    }
}