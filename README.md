# Product List Management System

## Overview

The Product List Management System is a C# console application developed to manage products through a menu-driven interface.

Users can add, display, search, edit, delete, sort, and filter products.

## Features

- Add and display products.
- Prevent duplicate products.
- Validate product names, categories, and prices.
- Search products by name, category, and ID.
- Edit and delete existing products.
- Calculate the total value of products.
- Sort products by price in ascending and descending order.
- Filter products by price range.
- Display product statistics.
- Save and load product data using JSON.
- Export product information to a TXT file.
- Paginate product listings.
- Admin and User access modes.
- Generic repository implementation.

## Project Structure

- `Product.cs` – Defines the Product class and its properties.
- `ProductManager.cs` – Contains product management functionality.
- `GenericRepository.cs` – Provides generic collection management.
- `Program.cs` – Contains the application menu and main program flow.
- `ProductListManagementSystem.Tests` – Contains automated unit tests.

## Testing

The project includes automated tests using xUnit.

The tests cover product management, searching, editing, deleting, duplicate detection, and generic repository functionality.

**Latest test result: 21 passed, 0 failed.**

## How to Run

1. Open the solution in Visual Studio.
2. Build the solution.
3. Run the `ProductListManagementSystem` project.
4. Follow the instructions displayed in the console.

## Technologies

- C#
- .NET
- Visual Studio
- LINQ
- JSON serialization
- xUnit
