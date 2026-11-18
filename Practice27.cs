using System;
using System.Collections.Generic;

namespace LambdaExpressionsPractice
{
    // Клас Товар з полями/властивостями: назва та ціна
    class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return string.Format("Товар: {0,-18} | Ціна: {1,8:F2} грн", Name, Price);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створюємо список об'єктів класу Product
            List<Product> products = new List<Product>
            {
                new Product("Ноутбук", 32500.00m),
                new Product("Мишка бездротова", 850.00m),
                new Product("Клавіатура", 1900.00m),
                new Product("Монітор 27\"", 9400.00m),
                new Product("Навушники", 2300.00m),
                new Product("Веб-камера", 1450.00m)
            };

            Console.WriteLine("=== Початковий список товарів ===");
            PrintProducts(products);

            // =================================================================
            // ЗАГАЛЬНЕ ЗАВДАННЯ: Фільтрація через FindAll та лямбда-вираз
            // =================================================================
            decimal minPrice = 1000.00m;
            decimal maxPrice = 3000.00m;

            List<Product> filteredProducts = products.FindAll(p => p.Price >= minPrice && p.Price <= maxPrice);

            Console.WriteLine("\n=== ЗАГАЛЬНЕ ЗАВДАННЯ: Товари у діапазоні від " + minPrice + " до " + maxPrice + " грн ===");
            PrintProducts(filteredProducts);

            // =================================================================
            // ДОМАШНЄ ЗАВДАННЯ: Сортування за алфавітом через лямбда-вираз
            // =================================================================
            products.Sort((p1, p2) => string.Compare(p1.Name, p2.Name, StringComparison.CurrentCulture));

            Console.WriteLine("\n=== ДОМАШНЄ ЗАВДАННЯ: Список товарів за алфавітом назви ===");
            PrintProducts(products);
        }

        static void PrintProducts(List<Product> list)
        {
            foreach (Product item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
