# Working with Entity Framework (EF Core) and SQLite

## Overview
This guide provides step-by-step instructions to set up **Entity Framework Core** with an **SQLite** database in a minimal **.NET Web API** project.

## Summary of Steps
1. Create a minimal **Web API** project.
2. Install **NuGet** packages.
3. Configure **Entity Classes**.
4. Set up **DbContext** and dependency injection.
5. Run **EF Core Migrations** and update the database.
6. Optional: Configure database seeding.

---

## 🚀 Creating the Project
1. Navigate to your working directory:
   ```sh
   cd Projects
   ```

2. Create a **solution**:
   ```sh
   mkdir slnGym
   cd slnGym
   dotnet new solution
   ```

3. Generate a minimal **Web API** project:
   ```sh
   dotnet new webapi -n bckGym
   ```

4. Add the **project** to the solution:
   ```sh
   dotnet sln slnGym.sln add bckGym/bckGym.csproj
   ```

---

## 📦 Installing Dependencies
Move into your project directory:
```sh
cd bckGym
```

Install **SQLite** and **Entity Framework Core** packages:
```sh
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Install **EF Core global tools**:
```sh
dotnet tool install --global dotnet-ef
```

---

## 🏗️ Creating the Database Models
Create the **Entities** folder and define your table classes.

```sh
mkdir Entities
cd Entities
touch Customer.cs Exercise.cs CustomerGym.cs
```

### 📌 File: `Entities/Customer.cs`
```csharp
namespace bckGym.Entities;
public class Customer {
    public int CustomerId { get; set; }
    public required string Name { get; set; }
    public required string Weight { get; set; }
    public required string Height { get; set; }
    public required string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateTime Started { get; set; }
}
```

### 📌 File: `Entities/Exercise.cs`
```csharp
namespace bckGym.Entities;
public class Exercise {
    public int ExerciseId { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required string SubCategory { get; set; }
    public required string MuscleFocus { get; set; }
    public required string Description { get; set; }
    public int Duration { get; set; } // INTEGER CHECK(duration > 0)
    public required string Difficulty { get; set; } // CHECK(difficulty IN ('easy', 'medium', 'hard'))
    public DateTime CreatedAt { get; set; } // TIMESTAMP DEFAULT CURRENT_TIMESTAMP
}
```

### 📌 File: `Entities/CustomerGym.cs`
```csharp
namespace bckGym.Entities;
public class CustomerGym {
    public int CustomerId { get; set; }
    public int ExerciseId { get; set; }
    // Navigation properties
    public Customer? Customer { get; set; }
    public Exercise? Exercise { get; set; }
}
```

---

## 🗄️ Setting Up `DbContext`
Create a **Data** folder and add the `GymDbContext` class:

```sh
mkdir Data
cd Data
touch GymDbContext.cs DataExtensions.cs
```

### 📌 File: `Data/GymDbContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using bckGym.Entities;

namespace bckGym.Data;
public class GymDbContext(DbContextOptions<GymDbContext> options) : DbContext(options) {
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<CustomerGym> CustomerGyms => Set<CustomerGym>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<CustomerGym>().HasKey(c => new { c.CustomerId, c.ExerciseId });

        modelBuilder.Entity<Exercise>().ToTable(tb => {
            tb.HasCheckConstraint("CK_Exercise_Duration", "Duration > 0");
            tb.HasCheckConstraint("CK_Exercise_Difficulty", "Difficulty IN ('easy', 'medium', 'hard')");
        });
    }
}
```

### 📌 File: `Data/DataExtensions.cs`
```csharp
using Microsoft.EntityFrameworkCore;

namespace bckGym.Data;
public static class DataExtensions {
    public static void MigrateDb(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        dbContext.Database.Migrate();
    }
}
```

---

## 🔧 Configuring the Connection
Modify **Program.cs**:

```csharp
using bckGym.Data;

var connString = "Data Source=GymDb.db";
builder.Services.AddSqlite<GymDbContext>(connString);
```

Alternatively, configure **appsettings.json**:

```json
{
  "ConnectionStrings": {
    "GymDbConnection": "Data Source=GymDb.db"
  }
}
```

---

## 🛠️ Running EF Core Migrations
Generate the **initial migration**:
```sh
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
```

Verify migrations:
```sh
dotnet ef migrations list
```

Check generated SQL script:
```sh
dotnet ef migrations script
```

Apply the migration:
```sh
dotnet ef database update
```

---

## 🏃 Running the Project
Start the API using:
```sh
dotnet run
```

---

## 📦 Optional: Database Seeding
Modify **DbContext** to prepopulate some tables:

```csharp
modelBuilder.Entity<Customer>().HasData(
    new { CustomerId = 123456, Name = "Rosemiro Lateral Esquerdo", Weight = "65kg", Height = "1.75m", Email = "rosemiroesquerdo@palmeiras.com", BirthDate = DateOnly.FromDateTime(new DateTime(1974, 05, 19)), Started = DateOnly.FromDateTime(new DateTime(1993, 5, 22)) }
);
```

Create a **new migration**:
```sh
dotnet ef migrations add SeedTables --output-dir Data/Migrations
dotnet run
```

---

## 🛠️ Helpful EF Core Commands
### Reset the database
```sh
dotnet ef database drop --force
dotnet ef migrations remove
```

### Apply database updates
```sh
dotnet ef migrations add <MigrationName> --output-dir Data/Migrations
dotnet ef database update
```

### View migrations & SQL scripts
```sh
dotnet ef migrations list
dotnet ef migrations script
```

---

## 🏁 Conclusion
This guide covers **setting up, configuring, and managing** an **Entity Framework Core** database with **SQLite** in a minimal **.NET Web API** project.

---

## 📝 Notes
- Ensure you **exclude** `GymDb.db` from your Git repository.
- Install VS Code **SQLite extension** for database visualization.


