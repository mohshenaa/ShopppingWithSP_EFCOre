# ShoppingWithSP_EFCore

**ShoppingWithSP_EFCore** is a sample e-commerce project built using **ASP.NET Core** with **Entity Framework Core** that demonstrates integration with **Stored Procedures** for data access and operations.

This project applies EF Core for object-relational mapping while executing business logic via stored procedures — blending modern ORM features with traditional database programming patterns. :contentReference[oaicite:1]{index=1}

---

## 🛒 Overview

This application showcases:

- A **shopping platform** with products and cart management
- Usage of **Entity Framework Core** for working with data models
- Calling **Stored Procedures** from EF Core for key operations
- Modular structure built with best practices in ASP.NET Core MVC/ Razor Pages

It is designed for learning how to integrate **Stored Procedures** with EF Core in a real-world scenario. :contentReference[oaicite:2]{index=2}

---

## 📦 Tech Stack

| Technology | Version |
|------------|---------|
| ASP.NET Core | .Net08 |
| Entity Framework Core | latest |
| C# | .NET Language |
| HTML / CSS / JS | Frontend UI |
| SQL Server | Database |

*(Update versions based on project settings)*

---

## 📁 Repository Structure
ShoppingWithSP_EFCore/
├── ShoppingWithSP.sln
├── Controllers/
├── Models/
├── Views/
├── StoredProcedures/
├── wwwroot/
├── appsettings.json
├── Program.cs
└── README.md

## 🚀 Setup & Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/mohshenaa/ShopppingWithSP_EFCOre.git

2. Restore NuGet Packages

dotnet restore

3. Configure Database

Update your appsettings.json with your SQL Server connection string.

Ensure stored procedures are included or created in your database.

4. Apply Migrations (if using code-first)

dotnet ef database update

5. Run the application

dotnet run

🧠 Key Concepts Demonstrated

✔ Calling Stored Procedures from EF Core
✔ MVC-based shopping operations
✔ Model definitions and DbContext configurations
✔ Dependency Injection and service configuration

📌 Notes
This project is intended for learning and demonstration purposes.
Stored procedures help offload some business logic to the database and illustrate mixed usage with ORM approaches.



   
