using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hortifruti.Entidades
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public DateTime ExpireDate { get; set; }

        public Product(string name, decimal price, decimal quantity, UnitOfMeasure unitOfMeasure, DateTime expireDate, string id = null)
        {
            Id = (String.IsNullOrEmpty(id)) ?  Guid.NewGuid().ToString()[..6]: id;
            Name = name;
            Price = price;
            Quantity = quantity;
            UnitOfMeasure = unitOfMeasure;
            ExpireDate = expireDate;
        }

        public bool IsExpirationDateApproaching() => (ExpireDate - DateTime.Now).TotalDays <= 5;

        public string GetDaysUntilExpiration()
        {
            int daysUntilExpiration = (ExpireDate - DateTime.Now).Days;

            if (daysUntilExpiration < 0)
                return "  (vencido)";
            else if (daysUntilExpiration <= 5)
                return $"  (vence em {daysUntilExpiration} dias)";
            else
                return "";
        }

        public override bool Equals(object obj)
        {
            return obj is Product product && Id == product.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return $"Id {Id} - {Name}" 
            + $"\nPreco R$ {Price} por {UnitOfMeasure}"
            + $"\nQuantidade em estoque: {Quantity} {UnitOfMeasure}" 
            + $"\nValidade: {ExpireDate:dd/MM/yyyy}" + $"{(GetDaysUntilExpiration())}\n";
        }
    }
}
