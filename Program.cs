using Hortifruti.Entities;
using Hortifruti.Enums;
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
                        ManageCashier();
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
                        DonateProducts();
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

        static void ManageCashier()
        {
            string operacao = null;
            while (operacao != "0")
            {
                Helpers.DisplayHeader($"           Caixa do Hortifruti");
                Console.WriteLine("(1) - Realizar uma venda");
                Console.WriteLine("(0) - Voltar");
                Console.Write("Selecione uma Opcao: ");
                operacao = Console.ReadLine();

                switch (operacao)
                {
                    case "1":
                        MakeSale();
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

        static void MakeSale()
        {
            List<Product> productsSale = new List<Product>();

            while(true)
            {
                Helpers.DisplayHeader($"           Caixa do Hortifruti\n\n           Realizar uma venda");
                Console.Write("Leia o Código de barras do produto ou digite o Id do Produto: ");
                string id = Console.ReadLine();
                Product product = products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    Console.WriteLine("Produto nao encontrado.");
                    Console.ReadKey();
                    continue;
                }

                decimal quantity;
                while (true)
                {
                    Console.Write($"Quantidade (em {product.UnitOfMeasure}): ");
                    string input = Console.ReadLine();
                    input = input.Replace(',', '.');

                    if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out quantity)
                        && quantity > 0) break;

                    Console.WriteLine("Quantidade inválido. Por favor, insira um valor válido.");
                }

                if (product.Quantity < quantity)
                {
                    Console.WriteLine($"Infelizmente não temos essa Quantidade de {product.Name}(s) em estoque. Tente uma quantidade menos.");
                    Console.ReadKey();
                    continue;
                }

                Product productSale = new Product(product.Name, product.Price, quantity, product.UnitOfMeasure, product.ExpireDate, id: product.Id);

                productsSale.Add(productSale);

                Console.WriteLine($"Produto adicionado com sucesso: {productSale.Quantity} {productSale.UnitOfMeasure} de {productSale.Name}");
                Console.ReadKey();

                Console.Write("\nAdicionar mais produtos (sim/nao)? ");
                string option = Console.ReadLine();

                if(option == "nao")
                {
                    break;
                }
            }

            Console.WriteLine("\nSua compra está sendo finalizada, aguarde...");
            Thread.Sleep(1000);

            Helpers.DisplayHeader($"           Caixa do Hortifruti\n\n           Seu Carrinho");
            productsSale.ForEach((product) => Console.WriteLine(product.ShowOnCart()));

            bool isSuccess = Helpers.PaymentProcessment();

            if (!isSuccess) {
                Console.WriteLine("Ocorreu um erro no processamento do pagamento de sua compra, o Gerente será acionado.");
                return;
            }

            productsSale.ForEach((productSale) =>
            {
                products.ForEach((product) =>
                {
                    product.Quantity -= productSale.Quantity;
                });
            });

            Console.WriteLine("\nSua compra está sendo finalizada, aguarde...");


            Helpers.DisplayHeader($"           Hortifruti");
            Console.WriteLine("Compra realizada com sucesso, obrigado!");
            Console.ReadKey();
        }

        static void WeighProducts()
        {
            Helpers.DisplayHeader($"            ESTACAO DE PESAGEM");

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
                Console.WriteLine("Este produto é medido em unidades, nao faz sentido em pesa-lo.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(product);

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
            products.ForEach((product) => Console.WriteLine(product));
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

            Console.WriteLine();
            Console.WriteLine(productToUpdate);

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


        public static void DonateProducts()
        {
            string operacao = null;
            while (operacao != "0")
            {
                Helpers.DisplayHeader($"           Produtos para Doar");
                Console.WriteLine("(1) - Listar produtos para doar para a caridade");
                Console.WriteLine("(2) - Listar produtos para doar para a agricultura familiar");
                Console.WriteLine("(0) - Voltar");
                Console.Write("Selecione uma Opcao: ");
                operacao = Console.ReadLine();

                switch (operacao)
                {
                    case "1":
                        ListProductsCloseToExpire();
                        break;
                    case "2":
                        ListProductsExpired();
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

        public static void ListProductsCloseToExpire()
        {
            Helpers.DisplayHeader($"        PRODUTOS PROXIMOS DA VALIDADE");
            List<Product> productsToExpire = products.Where((product) => 
                product.IsExpirationDateApproaching()).ToList();

            if(productsToExpire.Count == 0) {
                Console.WriteLine("\nNão há produtos próximos da validade no momento.");
                Console.ReadKey();
                return;
            }

            productsToExpire.ForEach((product) => Console.WriteLine(product));

            Donate();

            Console.ReadKey();
        }

        public static void ListProductsExpired()
        {
            Helpers.DisplayHeader($"          PRODUTOS VENCIDOS");

            List<Product> productsExpired = products.Where((product) => product.IsExpired()).ToList();

            if (productsExpired.Count == 0) {
                Console.WriteLine("\nNão há produtos próximos da validade no momento.");
                Console.ReadKey();
                return;
            }

            productsExpired.ForEach((product) => Console.WriteLine(product));

            Donate();

            Console.ReadKey();
        }

        public static void Donate()
        {
            Console.Write("\nDeseja doar algum produto no momento? (sim/nao)?");
            string option = Console.ReadLine();

            if (option == "nao") return;

            Console.Write("\nDigite o Id do produto a ser doado: ");
            string id = Console.ReadLine();

            Product productToDonate = products.FirstOrDefault(p => p.Id == id);

            if (productToDonate == null) {
                Console.WriteLine("Produto nao encontrado.");
                return;
            }

            Console.Write("\nDigite o nome da entidade para qual o item será doado: ");
            string institution = Console.ReadLine();

            products.Remove(productToDonate);

            string message = $"\n{productToDonate.Quantity} {productToDonate.UnitOfMeasure} "
                + $"de {productToDonate.Name} serão doados para a entidade {institution}.";
            Console.WriteLine(message);
        }
    }
}