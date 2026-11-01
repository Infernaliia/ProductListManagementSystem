using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace ProductListManagementSystem
{
    public class ProductManager
    {
        private List<Product> Products => repository.GetAll();
        private GenericRepository<Product> repository =
            new GenericRepository<Product>();
        public int ProductCount => Products.Count;
        private Dictionary<string, ConsoleColor> CategoryColors = 
            new Dictionary<string, ConsoleColor>();
        private int nextId = 1;
        public void AddProduct(Product product)
        {
            product.Id = nextId;
            nextId++;

            repository.Add(product);
        }
        public void ShowProducts()
        {
            foreach (Product product in Products.OrderBy(product => product.Id))    
            {
                Console.ForegroundColor = GetCategoryColor(product.Category);

                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Category: {product.Category}");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: {product.Price:F2} kr");

                Console.ResetColor();
                Console.WriteLine();
            }
        }
        public void ShowProductsPaginated()
        {
            int pageSize = 5;
            int page = 0;

            var sortedProducts = Products.OrderBy(p => p.Id).ToList();

            while (page * pageSize < sortedProducts.Count)
            {
                var productsOnPage = sortedProducts
                    .Skip(page * pageSize)
                    .Take(pageSize);

                foreach (Product product in productsOnPage)
                {
                    Console.WriteLine($"{product.Id}: {product.Name} - {product.Price:F2} kr");
                }

                page++;

                if (page * pageSize < sortedProducts.Count)
                {
                    Console.WriteLine("Press Enter to see the next page...");
                    Console.ReadLine();
                }
            }
        }
        private ConsoleColor GetCategoryColor(string category)
        {
            if (!CategoryColors.ContainsKey(category))
            {
                ConsoleColor[] colors =
                {
                ConsoleColor.Cyan,
                ConsoleColor.Yellow,
                ConsoleColor.Green,
                ConsoleColor.Magenta,
                ConsoleColor.Blue,
                ConsoleColor.DarkYellow,
                ConsoleColor.DarkCyan,
                ConsoleColor.Red
            };

                Random random = new Random();

                CategoryColors[category] = colors[random.Next(colors.Length)];
            }

            return CategoryColors[category];
        }
        public void ExportProductsToTxt()
        {
            List<string> lines = new List<string>();

            lines.Add("===== PRODUCT LIST =====");
            lines.Add("");

            foreach (Product product in Products.OrderBy(p => p.Id))
            {
                lines.Add($"ID: {product.Id}");
                lines.Add($"Category: {product.Category}");
                lines.Add($"Name: {product.Name}");
                lines.Add($"Price: {product.Price:F2} kr");
                lines.Add("");
            }
            lines.Add($"Total price: {CalculateTotal():F2} kr");

            File.WriteAllLines("products.txt", lines);

            Console.WriteLine("Products successfully exported to products.txt");
        }
        public void SortProductsAscending()
        {
            foreach (Product product in Products.OrderBy(p => p.Price))
            {
                Console.WriteLine($"ID: {product.Id}");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: {product.Price:F2} kr");
            }
        }
        public void SortProductsDescending()
        {
            foreach (Product product in Products.OrderByDescending(p => p.Price))
            {
                Console.WriteLine($"ID: {product.Id}");
                Console.WriteLine($"Category: {product.Category}");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: {product.Price:F2} kr");
                Console.WriteLine();
            }
        }
        public decimal CalculateTotal()
        {
            return Products.Sum(product => product.Price);
        }
        public bool ProductExists(Product product)
        {
            return Products.Any(p =>
                p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase) &&
                p.Category.Equals(product.Category, StringComparison.OrdinalIgnoreCase));
        }
        public bool ProductExists(Product product, int excludedId)
        {
            return Products.Any(p =>
                p.Id != excludedId &&
                p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase) &&
                p.Category.Equals(product.Category, StringComparison.OrdinalIgnoreCase));
        }
        public Product? FindProductByName(string name)
        {
            return Products.FirstOrDefault(p =>
             p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public Product? FindProductById(int id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }
        public bool DeleteProduct(int id)
        {
            Product? product = FindProductById(id);

            if (product == null)
            {
                return false;
            }

            repository.Remove(product);
            return true;
        }
        public bool EditProduct(int id, string name, string category, decimal price)
        {
            Product? product = FindProductById(id);

            if (product == null)
            {
                return false;
            }

            Product updatedProduct = new Product
            {
                Name = name,
                Category = category
            };

            if (ProductExists(updatedProduct, id))
            {
                return false;
            }

            product.Name = name;
            product.Category = category;
            product.Price = price;

            return true;
        }
        public List<Product> FindProductsByCategory(string category)
        {
            return Products
        .Where(p => p.Category.Equals(
            category, StringComparison.OrdinalIgnoreCase))
        .ToList();
        }
        public List<Product> FindProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return Products
             .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
             .OrderBy(p => p.Price)
             .ToList();
        }
        public void SaveProducts()
        {
            string json = JsonSerializer.Serialize(Products);
            File.WriteAllText("products.json", json);
        }
        public void LoadProducts()
        {
            if (Products.Count > 0)
            {
                return;
            }

            if (File.Exists("products.json"))
            {
                string json = File.ReadAllText("products.json");

                List<Product> loadedProducts =
                    JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();

                foreach (Product product in loadedProducts)
                {
                    repository.Add(product);
                }
            }

            nextId = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
        }
        public void ShowStatistics()
        {
            Console.WriteLine("===== PRODUCT STATISTICS =====");

            Console.WriteLine($"Total products: {Products.Count}");

            Console.WriteLine("Products per category:");

            foreach (var group in Products.GroupBy(p => p.Category))
            {
                Console.WriteLine($"{group.Key}: {group.Count()}");
            }

            Console.WriteLine($"Total inventory value: {Products.Sum(p => p.Price):F2} kr");

            decimal averagePrice = Products.Count > 0
                ? Products.Average(p => p.Price)
                : 0;

            Console.WriteLine($"Average product price: {averagePrice:F2} kr");
            if (Products.Count > 0)
            {
                Product mostExpensive = Products.OrderByDescending(p => p.Price).First();

                Console.WriteLine($"Most expensive product: {mostExpensive.Name} - {mostExpensive.Price:F2} kr");
            }
            if (Products.Count > 0)
            {
                Product cheapest = Products.OrderBy(p => p.Price).First();

                Console.WriteLine($"Cheapest product: {cheapest.Name} - {cheapest.Price:F2} kr");
            }
        }
    }
}