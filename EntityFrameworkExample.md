# Entity Framework Core.NET9

## Summary: 

- 1.	Create your project
- 2.	Install the NuGet packages
- 3.	Install the EF application
- 4.	Create your Entities class (Class tables)
- 5.	Create the DbContext and DataExtensions classes
- 6.	Modify your Program.cs
- 7.	Create the DbContext, (Define your class tables that EF will create)
- 8.	Run the EF migration
- 9.	Run the EF update

---

```sh
>cd Projects
>dotnet new webapi -n bckGym
>cd bckGym
>dotnet add package Microsoft.EntityFrameworkCore.Sqlite
>dotnet add package Microsoft.EntityFrameworkCore.Design
>dotnet tool install --global dotnet-ef
>mkdir Entities
>cd Entities
>copy con Customer.cs
namespace bckGym.Entities;
public class Customer{
        public int CustomerId {get; set;}
        public required string Name {get; set;}
        public required string Weight {get; set;}
        public required string Height {get; set;}
        public required string Email {get; set;}
        public DateOnly BirthDate {get; set;}
        public DateTime Started {get; set;}
}^Z

>coy con Exercise.cs
namespace bckGym.Entities;

public class Exercise
{
  public int ExerciseId { get; set; }
  public required string? Name { get; set; }
  public required string Category { get; set; }
  public required string SubCategory { get; set; }
  public required string MuscleFocus { get; set; }
  public required string? Description { get; set; }
  public int Duration { get; set; } //INTEGER CHECK(duration > 0),
  //TEXT CHECK(difficulty IN ('easy', 'medium', 'hard')),
  public required string Difficulty { get; set; }
  //TIMESTAMP DEFAULT CURRENT_TIMESTAMP
  public DateTime CreatedAt { get; set; }
}^Z


>copy con CustomerGym.cs
namespace bckGym.Entities;

public class CustomerGym
{
    public int CustomerId { get; set; }
    public int ExerciseId { get; set; }
    // Navigation properties
    public Customer? Customer { get; set; }
    public Exercise? Exercise { get; set; }
}

>mkdir Data
>cd Data
>copy con DataExtensions.cs
using Microsoft.EntityFrameworkCore;
namespace bckGym.Data;

public static class DataExtensions
{
   public static void MigrateDb(this WebApplication app)
   {
      // using release, the scope when the block finishes
      using var scope = app.Services.CreateScope(); 
      var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
      dbContext.Database.Migrate();
   }
}
>copy con GymDbContext.cs

using Microsoft.EntityFrameworkCore;
using bckGym.Entities;

namespace bckGym.Data;

public class GymDbContext(DbContextOptions<GymDbContext> options) : DbContext(options)
{
   public DbSet<Customer> Customers => Set<Customer>();
   public DbSet<Exercise> Exercises => Set<Exercise>();
   public DbSet<CustomerGym> CustomerGyms => Set<CustomerGym>();

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      // Composite primary key ensures uniqueness
      modelBuilder.Entity<CustomerGym>()
          .HasKey(c => new { c.CustomerId, c.ExerciseId }); 

      modelBuilder.Entity<Exercise>()
        .ToTable(tb =>
        {
            // Ensure duration is positive
            tb.HasCheckConstraint("CK_Exercise_Duration", "Duration > 0"); 
            // Restrict values
            tb.HasCheckConstraint("CK_Exercise_Difficulty", 
                     "Difficulty IN ('easy', 'medium', 'hard')"); 
        });
   }
}
```

```sh
>cd ..
>dotnet ef migrations add InitialCreate --output-dir Data\Migrations
  ```

```sh
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
```

```sh
>dotnet ef migrations list
```

```sh
Build started...
Build succeeded.
20250528215857_InitialCreate (Pending)
```

```sh
>dotnet ef migrations script
```

```sh
Build started...
Build succeeded.
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Customers" (
    "CustomerId" INTEGER NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Weight" TEXT NOT NULL,
    "Height" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "BirthDate" TEXT NOT NULL,
    "Started" TEXT NOT NULL
);

CREATE TABLE "Exercises" (
    "ExerciseId" INTEGER NOT NULL CONSTRAINT "PK_Exercises" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NULL,
    "Category" TEXT NOT NULL,
    "SubCategory" TEXT NOT NULL,
    "MuscleFocus" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Duration" INTEGER NOT NULL,
    "Difficulty" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "CK_Exercise_Difficulty" CHECK (Difficulty IN ('easy', 'medium', 'hard')),
    CONSTRAINT "CK_Exercise_Duration" CHECK (Duration > 0)
);

CREATE TABLE "CustomerGyms" (
    "CustomerId" INTEGER NOT NULL,
    "ExerciseId" INTEGER NOT NULL,
    CONSTRAINT "PK_CustomerGyms" PRIMARY KEY ("CustomerId", "ExerciseId"),
    CONSTRAINT "FK_CustomerGyms_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("CustomerId") ON DELETE CASCADE,
    CONSTRAINT "FK_CustomerGyms_Exercises_ExerciseId" FOREIGN KEY ("ExerciseId") REFERENCES "Exercises" ("ExerciseId") ON DELETE CASCADE
);

CREATE INDEX "IX_CustomerGyms_ExerciseId" ON "CustomerGyms" ("ExerciseId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250528215857_InitialCreate', '9.0.5');

COMMIT;
```


```sh
>dotnet ef database update
```

```sh
Build started...
Build succeeded.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (42ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      PRAGMA journal_mode = 'wal';
info: Microsoft.EntityFrameworkCore.Migrations[20411]
      Acquiring an exclusive lock for migration application. See https://aka.ms/efcore-docs-migrations-lock for more information if this takes too long.
Acquiring an exclusive lock for migration application. See https://aka.ms/efcore-docs-migrations-lock for more information if this takes too long.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*) FROM "sqlite_master" WHERE "name" = '__EFMigrationsLock' AND "type" = 'table';
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE IF NOT EXISTS "__EFMigrationsLock" (
          "Id" INTEGER NOT NULL CONSTRAINT "PK___EFMigrationsLock" PRIMARY KEY,
          "Timestamp" TEXT NOT NULL
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      INSERT OR IGNORE INTO "__EFMigrationsLock"("Id", "Timestamp") VALUES(1, '2025-05-28 22:02:09.8748321+00:00');
      SELECT changes();
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
          "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
          "ProductVersion" TEXT NOT NULL
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT COUNT(*) FROM "sqlite_master" WHERE "name" = '__EFMigrationsHistory' AND "type" = 'table';
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT "MigrationId", "ProductVersion"
      FROM "__EFMigrationsHistory"
      ORDER BY "MigrationId";
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20250528215857_InitialCreate'.
Applying migration '20250528215857_InitialCreate'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE "Customers" (
          "CustomerId" INTEGER NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY AUTOINCREMENT,
          "Name" TEXT NOT NULL,
          "Weight" TEXT NOT NULL,
          "Height" TEXT NOT NULL,
          "Email" TEXT NOT NULL,
          "BirthDate" TEXT NOT NULL,
          "Started" TEXT NOT NULL
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE "Exercises" (
          "ExerciseId" INTEGER NOT NULL CONSTRAINT "PK_Exercises" PRIMARY KEY AUTOINCREMENT,
          "Name" TEXT NULL,
          "Category" TEXT NOT NULL,
          "SubCategory" TEXT NOT NULL,
          "MuscleFocus" TEXT NOT NULL,
          "Description" TEXT NULL,
          "Duration" INTEGER NOT NULL,
          "Difficulty" TEXT NOT NULL,
          "CreatedAt" TEXT NOT NULL,
          CONSTRAINT "CK_Exercise_Difficulty" CHECK (Difficulty IN ('easy', 'medium', 'hard')),
          CONSTRAINT "CK_Exercise_Duration" CHECK (Duration > 0)
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE "CustomerGyms" (
          "CustomerId" INTEGER NOT NULL,
          "ExerciseId" INTEGER NOT NULL,
          CONSTRAINT "PK_CustomerGyms" PRIMARY KEY ("CustomerId", "ExerciseId"),
          CONSTRAINT "FK_CustomerGyms_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("CustomerId") ON DELETE CASCADE,
          CONSTRAINT "FK_CustomerGyms_Exercises_ExerciseId" FOREIGN KEY ("ExerciseId") REFERENCES "Exercises" ("ExerciseId") ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX "IX_CustomerGyms_ExerciseId" ON "CustomerGyms" ("ExerciseId");
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
      VALUES ('20250528215857_InitialCreate', '9.0.5');
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      DELETE FROM "__EFMigrationsLock";
Done.
```

```sh
>dir
```

```sh
 Volume in drive D is New Volume
 Volume Serial Number is 4A13-1E33

 Directory of D:\Projects\slnGym\bckGym

29/05/2025  10:02 am    <DIR>          .
29/05/2025  09:22 am    <DIR>          ..
29/05/2025  09:22 am               127 appsettings.Development.json
29/05/2025  09:22 am               151 appsettings.json
29/05/2025  09:25 am               679 bckGym.csproj
29/05/2025  09:22 am               125 bckGym.http
29/05/2025  09:32 am    <DIR>          bin
29/05/2025  09:58 am    <DIR>          Data
29/05/2025  09:28 am    <DIR>          Entities
29/05/2025  10:02 am            40,960 GymDb.db
29/05/2025  09:32 am    <DIR>          obj
29/05/2025  09:49 am             1,264 Program.cs
29/05/2025  09:22 am    <DIR>          Properties
               6 File(s)         43,306 bytes
               7 Dir(s)  659,577,188,352 bytes free
```

```sh
>code .
```

![image](https://github.com/user-attachments/assets/aa3d519c-8c5e-4493-9d98-99fc37736a8d)

 





## Some helpful commands:

### To clean up and start from scratch 
```sh
>dotnet ef database drop --force
```

```sh
>dotnet ef migrations remove
```

### This creates the migration scripts:

```sh
>dotnet ef migrations add InitialCreate --output-dir Data\Migrations
```

### Note: You need to run this command for every change you make in the entity classes and give it a different name. For example, if you add a new field to a particular table, you can run this command again, but instead of InitialCreate, you should name it differently, for example: You can use the name, AddingFieldNameToTableCustomer, indicating the kind of change you made in your class Customer.cs. 
If you wish to return to the status that was in place before you added this field, you can simply remove this migration by running the command:

```sh
	>dotnet ef migrations remove
```

### This will remove the last command you ran with the “migrations” command. Once you remove the migration, run the ‘update’ command.


### List your migrations:

```sh
>dotnet ef migrations list
```

### List your scripts. At this point, only the Migration tables were created:

```sh
>dotnet ef migrations script
```

```sh
Build started...
Build succeeded.
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
	    "ProductVersion" TEXT NOT NULL
	);

BEGIN TRANSACTION;
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250528024445_InitialCreate', '9.0.5');

COMMIT;
```

### This runs the scripts created with the previous command:

```sh
>dotnet ef database update
```

```sh
Build started...
Build succeeded.
info: Microsoft.EntityFrameworkCore.Migrations[20411]
      Acquiring an exclusive lock for migration application. See https://aka.ms/efcore-docs-migrations-lock for more information if this takes too long.
Acquiring an exclusive lock for migration application. See https://aka.ms/efcore-docs-migrations-lock for more information if this takes too long.
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20250528024445_InitialCreate'.
Applying migration '20250528024445_InitialCreate'.
Done.
```


### Recheck your scripts:

```sh
>dotnet ef migrations script
```











