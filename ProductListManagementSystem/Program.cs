
using ProductListManagementSystem;
using System.Globalization;

Console.WriteLine("================================");
Console.WriteLine("   PRODUCT MANAGEMENT SYSTEM");
Console.WriteLine("================================");
Console.WriteLine();

Console.WriteLine("Welcome to your product manager!");

ProductManager manager = new ProductManager();
manager.LoadProducts();

Console.WriteLine("===== LOGIN =====");
Console.WriteLine("1. Admin");
Console.WriteLine("2. User");
Console.Write("Choose your role: ");

string role = Console.ReadLine() ?? "";

bool running = true;
    
while (role != "1" && role != "2")
{
    Console.WriteLine("Invalid role. Please choose 1 for Admin or 2 for User.");
    Console.Write("Choose your role: ");
    role = Console.ReadLine() ?? "";
}

while (running)
{
    string choice = ShowMenu(role);

    if (role == "2" &&
        (choice == "1" || choice == "6" || choice == "7" ||
         choice == "9" || choice == "13"))
    {
        Console.WriteLine("Access denied. Admin privileges required.");
    }
    else if (choice == "8")
    {
        manager.SaveProducts();
        running = false;
        Console.WriteLine("Goodbye!");
    }
    else if (choice == "1")
    {
        AddProductToList(manager);
    }
    else if (choice == "2")
    {
        Console.WriteLine();
        Console.WriteLine("Product List:");

        manager.ShowProducts();

        Console.WriteLine($"Total price: {manager.CalculateTotal():F2} kr");
    }
    else if (choice == "3")
    {
        Console.Write("Enter product name: ");
        string name = Console.ReadLine() ?? "";

        Product? found = manager.FindProductByName(name);

        if (found != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Found: {found.Name} - {found.Price:F2} kr");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }
    else if (choice == "4")
    {
        Console.Write("Enter category: ");
        string category = Console.ReadLine() ?? "";

        List<Product> results = manager.FindProductsByCategory(category);

        if (results.Count > 0)
        {
            foreach (Product product in results)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{product.Name} - {product.Price:F2} kr");
                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("No products found.");
        }
    }
    else if (choice == "5")
    {
        Console.Write("Enter product ID: ");
        string idInput = Console.ReadLine() ?? "";
        if (int.TryParse(idInput, out int id))
        {
            Product? found = manager.FindProductById(id);
            if (found != null)
            {
                Console.WriteLine($"ID: {found.Id}");
                Console.WriteLine($"Category: {found.Category}");
                Console.WriteLine($"Name: {found.Name}");
                Console.WriteLine($"Price: {found.Price:F2} kr");
            }
            else
            {
                Console.WriteLine("No product found with that ID.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID. Please enter a number.");
        }
    }
    else if (choice == "6")
    {
        Console.Write("Enter the ID of the product to delete: ");
        string idInput = Console.ReadLine() ?? "";

        if (int.TryParse(idInput, out int id))
        {
            bool deleted = manager.DeleteProduct(id);

            if (deleted)
            {
                Console.WriteLine("Product successfully deleted.");
            }
            else
            {
                Console.WriteLine("No product found with that ID.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID. Please enter a number.");
        }
    }
    else if (choice == "7")
    {
        Console.Write("Enter the ID of the product to edit: ");
        string idInput = Console.ReadLine() ?? "";

        if (int.TryParse(idInput, out int id))
        {
            Product? product = manager.FindProductById(id);

            if (product == null)
            {
                Console.WriteLine("No product found with that ID.");
            }
            else
            {
                Console.Write("Enter new product name: ");
                string newName = Console.ReadLine() ?? "";

                Console.Write("Enter new product category: ");
                string newCategory = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(newName) ||
                    string.IsNullOrWhiteSpace(newCategory))
                {
                    Console.WriteLine("Name and category cannot be empty.");
                }
                else
                {
                    Console.Write("Enter new product price: ");
                    string priceInput = Console.ReadLine() ?? "";

                    decimal newPrice;

                    if (
                        !decimal.TryParse(priceInput, NumberStyles.Number, CultureInfo.CurrentCulture, out newPrice) &&
                        !decimal.TryParse(priceInput, NumberStyles.Number, CultureInfo.InvariantCulture, out newPrice)
                        || newPrice <= 0)
                    {
                        Console.WriteLine("Invalid price. Enter a number greater than zero.");
                    }
                    else
                    {
                        bool duplicate = manager.ProductExists(new Product
                        {
                            Name = newName,
                            Category = newCategory
                        }, id);

                        if (duplicate)
                        {
                            Console.WriteLine("A product with this name and category already exists.");
                        }
                        else
                        {
                            bool updated = manager.EditProduct(id, newName, newCategory, newPrice);

                            if (updated)
                            {
                                Console.WriteLine("Product successfully updated!");
                            }
                            else
                            {
                                Console.WriteLine("Product could not be updated.");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid ID. Please enter a number.");
        }
    }
    else if (choice == "9")
    {
        manager.ShowStatistics();
    }
    else if (choice == "10")
    {
        Console.Write("Enter minimum price: ");
        string minInput = Console.ReadLine() ?? "";

        Console.Write("Enter maximum price: ");
        string maxInput = Console.ReadLine() ?? "";

        if (
    decimal.TryParse(minInput, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal minPrice) &&
    decimal.TryParse(maxInput, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal maxPrice)
)
        {
            if (minPrice < 0 || maxPrice < minPrice)
            {
                Console.WriteLine("Invalid range. Maximum must be greater than or equal to minimum, and prices cannot be negative.");
            }
            else
            {
                List<Product> results = manager.FindProductsByPriceRange(minPrice, maxPrice);

                if (results.Count > 0)
                {
                    foreach (Product product in results)
                    {
                        Console.WriteLine($"{product.Name} - {product.Price:F2} kr");
                    }
                }
                else
                {
                    Console.WriteLine("No products found in that price range.");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid price. Please enter valid numbers.");
        }
    }
    else if (choice == "11")
    {
        Console.WriteLine("Products sorted by price (ascending):");
        manager.SortProductsAscending();
    }
    else if (choice == "12")
    {
        Console.WriteLine("Products sorted by price (descending):");
        manager.SortProductsDescending();
    }
    else if (choice == "13")
    {
        manager.ExportProductsToTxt();
    }
    else if (choice == "14")
    {
        manager.ShowProductsPaginated();
    }
    else
    {
        Console.WriteLine("Invalid option. Please choose 1-14.");
    }
}
string ShowMenu(string role)
{
    Console.WriteLine("===== PRODUCT MANAGER =====");

    if (role == "1")
    {
        Console.WriteLine("===== ADMIN MENU =====");
        Console.WriteLine("1. Add product");
        Console.WriteLine("2. Show products and total");
        Console.WriteLine("3. Search by name");
        Console.WriteLine("4. Search by category");
        Console.WriteLine("5. Search by ID");
        Console.WriteLine("6. Delete product");
        Console.WriteLine("7. Edit product");
        Console.WriteLine("9. Show statistics");
        Console.WriteLine("10. Filter products by price range");
        Console.WriteLine("11. Sort products ascending");
        Console.WriteLine("12. Sort products descending");
        Console.WriteLine("13. Export products to TXT");
        Console.WriteLine("14. Show products with pagination");
        Console.WriteLine("8. Exit");
    }
    else if (role == "2")
    {
        Console.WriteLine("===== USER MENU =====");
        Console.WriteLine("2. Show products and total");
        Console.WriteLine("3. Search by name");
        Console.WriteLine("4. Search by category");
        Console.WriteLine("5. Search by ID");
        Console.WriteLine("10. Filter products by price range");
        Console.WriteLine("11. Sort products ascending");
        Console.WriteLine("12. Sort products descending");
        Console.WriteLine("14. Show products with pagination");
        Console.WriteLine("8. Exit");
    }

    Console.Write("Choose an option: ");
    string choice = Console.ReadLine() ?? "";

    return choice;
}
void AddProductToList(ProductManager manager)
{
    Product newProduct = new Product();

    Console.Write("Enter product category: ");
    newProduct.Category = Console.ReadLine() ?? "";

    while (string.IsNullOrWhiteSpace(newProduct.Category))
    {
        Console.WriteLine("Category cannot be empty.");
        Console.Write("Enter product category: ");
        newProduct.Category = Console.ReadLine() ?? "";
    }

    Console.Write("Enter product name: ");
    newProduct.Name = Console.ReadLine() ?? "";

    while (string.IsNullOrWhiteSpace(newProduct.Name))
    {
        Console.WriteLine("Product name cannot be empty.");
        Console.Write("Enter product name: ");
        newProduct.Name = Console.ReadLine() ?? "";
    }

    Console.Write("Enter product price: ");

    decimal price;
    string priceInput = Console.ReadLine() ?? "";

    while (
    !decimal.TryParse(priceInput, NumberStyles.Number, CultureInfo.CurrentCulture, out price) &&
    !decimal.TryParse(priceInput, NumberStyles.Number, CultureInfo.InvariantCulture, out price)
    || price <= 0)
    {
        Console.WriteLine("Invalid price. Enter a number greater than zero.");
        Console.Write("Enter product price: ");
        priceInput = Console.ReadLine() ?? "";
    }

    newProduct.Price = price;

    if (manager.ProductExists(newProduct))
    {
        Console.WriteLine("This product already exists in that category.");
    }
    else
    {
        manager.AddProduct(newProduct);
        Console.WriteLine("Product added successfully.");
    }
}