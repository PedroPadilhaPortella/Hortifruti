using Hortifruti.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hortifruti.Repository
{
    public static class ProductsRepository
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
        {
            new Product("Banana Nanica", 5.00m, 20, UnitOfMeasure.Kg, new DateTime(2024, 11, 15)),
            new Product("Maçã Gala", 7.50m, 30, UnitOfMeasure.Kg, new DateTime(2024, 11, 20)),
            new Product("Laranja Pera", 4.20m, 25, UnitOfMeasure.Kg, new DateTime(2024, 11, 18)),
            new Product("Tomate", 6.30m, 18, UnitOfMeasure.Kg, new DateTime(2024, 11, 12)),
            new Product("Alface", 2.50m, 50, UnitOfMeasure.Unidades, new DateTime(2024, 10, 30)),
            new Product("Cenoura", 3.80m, 40, UnitOfMeasure.Kg, new DateTime(2024, 11, 10)),
            new Product("Batata", 2.90m, 35, UnitOfMeasure.Kg, new DateTime(2024, 11, 25)),
            new Product("Pão Integral", 8.00m, 15, UnitOfMeasure.Unidades, new DateTime(2024, 10, 29)),
            new Product("Iogurte", 3.50m, 10, UnitOfMeasure.Unidades, new DateTime(2024, 11, 02)),
            new Product("Queijo Minas", 20.00m, 8, UnitOfMeasure.Kg, new DateTime(2024, 11, 22))
        };
        }
    }
}
