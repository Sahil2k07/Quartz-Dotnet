# Quartz.NET with Entity Framework and PostgreSQL Setup

This project demonstrates how to integrate Quartz.NET with Entity Framework (EF) for scheduling tasks, using PostgreSQL as the database.

## Tech Used

- **Quartz.NET**: A powerful and flexible job scheduling library for .NET.
- **Entity Framework Core**: An Object-Relational Mapping (ORM) framework for database access.
- **PostgreSQL**: A relational database system used for persistent storage.

## Steps to Setup the Application

### Step 1: Add Necessary Packages

Run the following commands in your terminal to add the required NuGet packages to your project:

```bash
# Add Quartz.NET for job scheduling
dotnet add package Quartz

# Add Quartz extensions for dependency injection in ASP.NET Core
dotnet add package Quartz.Extensions.DependencyInjection

# Add Quartz integration with ASP.NET Core
dotnet add package Quartz.AspNetCore

# Add Microsoft Entity Framework Core (EF Core) for ORM functionality
dotnet add package Microsoft.EntityFrameworkCore

# Add PostgreSQL support for EF Core
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Add EF Core tools for migration and database management
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

These packages are essential for setting up the Quartz scheduler, integrating EF Core with PostgreSQL, and managing database migrations.

### Step 2: Configure Database and Quartz

1. **Set up PostgreSQL Connection String**:  
   In the `.env` file, configure the connection string for your PostgreSQL database:

   ```dotenv
   DATABASE_URL="Host=localhost;Port=5432;Database=Quartz-Dotnet;Username=postgres;Password=password;"

   ```

2. **Set up Entity Framework Context**:  
   Create a `DbContext` class to interact with PostgreSQL.

   ```csharp
   public class ApplicationDbContext : DbContext
   {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       public DbSet<QuartzSchedule> QuartzSchedules { get; set; }
   }
   ```

### Step 3: Create Migrations and Apply

1. **Create the Initial Migration**:

   Run the following command to generate the first migration based on the DbContext you set up earlier:

   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. **Apply the Migration**:

   After generating the migration, apply it to your PostgreSQL database:

   ```bash
   dotnet ef database update
   ```

This will create the required tables in your PostgreSQL database.

### Step 4: Run the Application

Once the database is configured and migrations are applied, you can start the application:

```bash
dotnet run
```

This command will start the application and the Quartz scheduler will begin executing jobs as configured.

### Step 5: Verify Quartz Jobs Execution

Check the console output for job execution logs. The application will run Quartz jobs according to the schedule defined in the code.

---

### Troubleshooting

- **EF Core Issues**: If you face any issues with Entity Framework, ensure that the connection string is correctly configured, and that the PostgreSQL database is running.
- **Quartz Configuration**: If Quartz jobs are not being triggered as expected, verify the job scheduling settings and ensure that Quartz services are correctly set up in the dependency injection container.

---

### Conclusion

With the above steps, you've integrated Quartz.NET with Entity Framework Core using PostgreSQL. Now, the application can manage jobs with persistent storage while leveraging EF Core for data storage and migrations.

Feel free to modify the job configurations, scheduler, and database logic according to your application's needs!
