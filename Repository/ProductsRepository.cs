using Hortifruti.Entities;
using Hortifruti.Enums;

namespace Hortifruti.Repository
{
    public static class ProductsRepository
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
        {
            new Product("Banana Nanica", 5.00m, 20, UnitOfMeasure.Kg, new DateTime(2024, 11, 15), "000001"),
            new Product("Maçã Gala", 7.50m, 30, UnitOfMeasure.Kg, new DateTime(2024, 11, 20), "000002"),
            new Product("Laranja Pera", 4.20m, 25, UnitOfMeasure.Kg, new DateTime(2024, 11, 18), "000003"),
            new Product("Tomate", 6.30m, 18, UnitOfMeasure.Kg, new DateTime(2024, 11, 12), "000004"),
            new Product("Alface", 2.50m, 50, UnitOfMeasure.Unidades, new DateTime(2024, 10, 30), "000005"),
            new Product("Cenoura", 3.80m, 40, UnitOfMeasure.Kg, new DateTime(2024, 11, 10), "000006"),
            new Product("Batata", 2.90m, 35, UnitOfMeasure.Kg, new DateTime(2024, 11, 25), "000007"),
            new Product("Pão Integral", 8.00m, 15, UnitOfMeasure.Unidades, new DateTime(2024, 10, 27), "000008"),
            new Product("Iogurte", 3.50m, 10, UnitOfMeasure.Unidades, new DateTime(2024, 11, 02), "000009"),
            new Product("Queijo Minas", 20.00m, 8, UnitOfMeasure.Kg, new DateTime(2024, 11, 22), "000010"),
            new Product("Energético Bali Hortelã", 6.00m, 0, UnitOfMeasure.Unidades, new DateTime(2030, 01, 01), "000011")
        };
        }
    }
}
