﻿using Hortifruti.Enums;

namespace Hortifruti.Entities
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
            Id = (String.IsNullOrEmpty(id)) ? Guid.NewGuid().ToString()[..6] : id;
            Name = name;
            Price = price;
            Quantity = quantity;
            UnitOfMeasure = unitOfMeasure;
            ExpireDate = expireDate;
        }

        public string GetPrice(decimal weight)
        {
            decimal totalPrice = Price * weight;
            return $"{Name} - {weight} {UnitOfMeasure} - R$ {totalPrice}";
        }

        public bool IsExpirationDateApproaching() {
            double daysUntilExpire = (ExpireDate - DateTime.Now).TotalDays;
            return daysUntilExpire <= 5 && daysUntilExpire > 0;
        }

        public bool IsExpired() => (ExpireDate - DateTime.Now).TotalDays <= 0;

        public string GetDaysUntilExpiration()
        {
            int daysUntilExpiration = (ExpireDate - DateTime.Now).Days + 1;

            if (daysUntilExpiration <= 0)
                return "  (vencido)";
            else if (daysUntilExpiration <= 5)
                return $"  (vence em {daysUntilExpiration} dias)";
            else
                return "";
        }

        public string CheckLackOfProducts()
        {
            return Quantity == 0 ? $"(em falta)" : "";
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
            + $"\nQuantidade em estoque: {Quantity} {UnitOfMeasure}  {(CheckLackOfProducts())}" 
            + $"\nValidade: {ExpireDate:dd/MM/yyyy}  {(GetDaysUntilExpiration())}\n";
        }

        public string ShowOnCart()
        {
            return $"  {Id} - {Name}"
            + $"\n  {Quantity} {UnitOfMeasure} x R${Price}\n"
            + $"                                           R$ {Price * Quantity}\n";
        }
    }
}
