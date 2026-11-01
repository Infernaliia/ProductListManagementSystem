using ProductListManagementSystem;
namespace ProductListManagementSystem.Tests
{
    public class ProductManagerTests
    {
        [Fact]
        public void CalculateTotal_ReturnsCorrectTotal()
        {
            ProductManager manager = new ProductManager();
            manager.AddProduct(new Product { Name = "Keyboard", Category = "Electronics", Price = 300m });
            manager.AddProduct(new Product { Name = "Mouse", Category = "Electronics", Price = 150m });
            Assert.Equal(450m, manager.CalculateTotal());
        }
        [Fact]
        public void AddProduct_IncreasesProductCount()
        {
            ProductManager manager = new ProductManager();
            manager.AddProduct(new Product
            {
                Name = "Monitor",
                Category = "Electronics",
                Price = 1500m
            });
            Assert.Equal(1, manager.ProductCount);
        }
        [Fact]
        public void DeleteProduct_RemovesProduct()
        {
            ProductManager manager = new ProductManager();
            Product product = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 300m
            };

            manager.AddProduct(product);
            bool deleted = manager.DeleteProduct(product.Id);

            Assert.True(deleted);
            Assert.Equal(0, manager.ProductCount);
        }
        [Fact]
        public void FindProductById_ReturnsCorrectProduct()
        {
            ProductManager manager = new ProductManager();
            Product product = new Product
            {
                Name = "Mouse",
                Category = "Electronics",
                Price = 150m
            };

            manager.AddProduct(product);
            Product? found = manager.FindProductById(product.Id);
            Assert.NotNull(found);
            Assert.Equal("Mouse", found.Name);
        }
        [Fact]
        public void EditProduct_UpdatesProductDetails()
        {
            ProductManager manager = new ProductManager();

            Product product = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 300m
            };

            manager.AddProduct(product);

            bool updated = manager.EditProduct(
                product.Id,
                "Mechanical Keyboard",
                "Accessories",
                349.99m
            );

            // Verify the changes
            Assert.True(updated);
            Assert.Equal("Mechanical Keyboard", product.Name);
            Assert.Equal("Accessories", product.Category);
            Assert.Equal(349.99m, product.Price);
        }
        [Fact]
        public void DeleteProduct_ReturnsFalseForInvalidId()
        {
            ProductManager manager = new ProductManager();
            bool deleted = manager.DeleteProduct(999);
            Assert.False(deleted);
            Assert.Equal(0, manager.ProductCount);
        }
        [Fact]
        public void FindProductById_ReturnsNullForInvalidId()
        {
            ProductManager manager = new ProductManager();
            Product? found = manager.FindProductById(999);
            Assert.Null(found);
        }
        [Fact]
        public void FindProductByName_ReturnsCorrectProduct()
        {
            ProductManager manager = new ProductManager();
            Product product = new Product
            {
                Name = "Monitor",
                Category = "Electronics",
                Price = 1500m
            };

            manager.AddProduct(product);
            Product? found = manager.FindProductByName("Monitor");
            Assert.NotNull(found);
            Assert.Equal(product.Id, found.Id);
            Assert.Equal("Monitor", found.Name);
        }
        [Fact]
        public void FindProductByName_IsCaseInsensitive()
        {
            ProductManager manager = new ProductManager();

            Product product = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 299
            };

            manager.AddProduct(product);

            Product? result = manager.FindProductByName("KEYBOARD");

            Assert.NotNull(result);
            Assert.Equal("Keyboard", result.Name);
        }
        [Fact]
        public void FindProductsByCategory_IsCaseInsensitive()
        {
            ProductManager manager = new ProductManager();

            Product product = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 299
            };

            manager.AddProduct(product);

            List<Product> results = manager.FindProductsByCategory("ELECTRONICS");

            Assert.Single(results);
            Assert.Equal("Keyboard", results[0].Name);
        }
        [Fact]
        public void ProductExists_IsCaseInsensitive()
        {
            ProductManager manager = new ProductManager();

            Product product1 = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 299
            };

            Product product2 = new Product
            {
                Name = "KEYBOARD",
                Category = "ELECTRONICS",
                Price = 399
            };

            manager.AddProduct(product1);

            Assert.True(manager.ProductExists(product2));
        }
        [Fact]
        public void GenericRepository_GetAll_ReturnsSeparateList()
        {
            GenericRepository<Product> repository = new GenericRepository<Product>();

            Product product = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 299
            };

            repository.Add(product);

            List<Product> retrievedItems = repository.GetAll();

            retrievedItems.Clear();

            Assert.Equal(1, repository.Count());
        }
        [Fact]
        public void FindProductByName_ReturnsNullForInvalidName()
        {
            ProductManager manager = new ProductManager();
            Product? found = manager.FindProductByName("Laptop");
            Assert.Null(found);
        }
        [Fact]
        public void FindProductsByCategory_ReturnsCorrectProducts()
        {
            ProductManager manager = new ProductManager();
            manager.AddProduct(new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 300m
            });

            manager.AddProduct(new Product
            {
                Name = "Chair",
                Category = "Furniture",
                Price = 800m
            });
            List<Product> results = manager.FindProductsByCategory("Electronics");
            Assert.Single(results);
            Assert.Equal("Keyboard", results[0].Name);
            Assert.Equal("Electronics", results[0].Category);
        }
        [Fact]
        public void FindProductsByPriceRange_ReturnsCorrectProducts()
        {
            ProductManager manager = new ProductManager();
            manager.AddProduct(new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 300m
            });

            manager.AddProduct(new Product
            {
                Name = "Mouse",
                Category = "Electronics",
                Price = 150m
            });

            manager.AddProduct(new Product
            {
                Name = "Chair",
                Category = "Furniture",
                Price = 800m
            });

            List<Product> results = manager.FindProductsByPriceRange(200m, 500m);
            Assert.Single(results);
            Assert.Equal("Keyboard", results[0].Name);
            Assert.Equal(300m, results[0].Price);
        }
        [Fact]
        public void ProductExists_ReturnsTrueForExistingProduct()
        {
            ProductManager manager = new ProductManager();
            Product product = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 300m
            };

            manager.AddProduct(product);
            bool exists = manager.ProductExists(product);

            Assert.True(exists);
        }
        [Fact]
        public void EditProduct_ReturnsFalseForInvalidId()
        {
            ProductManager manager = new ProductManager();
            bool updated = manager.EditProduct(
    999,
    "New Keyboard",
    "Electronics",
    350m
);
            Assert.False(updated);
            Assert.Equal(0, manager.ProductCount);
        }
        [Fact]
        public void GenericRepository_Add_IncreasesCount()
        {
            GenericRepository<Product> repository = new GenericRepository<Product>();
            Product product = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 300m
            };
            repository.Add(product);
            Assert.Equal(1, repository.Count());
        }
        [Fact]
        public void GenericRepository_GetAll_ReturnsAllItems()
        {
            GenericRepository<Product> repository = new GenericRepository<Product>();
            repository.Add(new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 300m
            });

            repository.Add(new Product
            {
                Name = "Chair",
                Category = "Furniture",
                Price = 800m
            });
            List<Product> results = repository.GetAll();
            Assert.Equal(2, results.Count);
            Assert.Equal("Keyboard", results[0].Name);
            Assert.Equal("Chair", results[1].Name);
        }
        [Fact]
        public void GenericRepository_Remove_RemovesItem()
        {
            GenericRepository<Product> repository = new GenericRepository<Product>();
            Product product = new Product
            {
                Name = "Keyboard",
                Category = "Electronics",
                Price = 300m
            };

            repository.Add(product);
            bool removed = repository.Remove(product);
            Assert.True(removed);
            Assert.Equal(0, repository.Count());
        }
        [Fact]
        public void GenericRepository_Remove_ReturnsFalseForMissingItem()
        {
            GenericRepository<Product> repository = new GenericRepository<Product>();
            Product product = new Product
            {
                Name = "Laptop",
                Category = "Electronics",
                Price = 12000m
            };
            bool removed = repository.Remove(product);
            Assert.False(removed);
            Assert.Equal(0, repository.Count());
        }
    }
}
