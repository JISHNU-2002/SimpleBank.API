# 📌 I. SimpleBank API
## 1. Overview

The **SimpleBank API** is a RESTful web service built with **ASP.NET Core Web API** using a **Code-First approach**. It is designed to provide core banking functionalities such as **account management, transactions, authentication, and user profile handling**. The API follows modern architectural practices, including **JWT authentication**, **repository pattern**, and **DTO-based data exchange**, ensuring security, scalability, and maintainability.

---

## 2. Key Features

1. **Authentication & Authorization**

   * Secure login and registration using **ASP.NET Core Identity**
   * **Token** based connection ensuring
   * **JWT-based authentication** (Bearer Token) for client-server communication
   * Role-based access control (Admin, Manager Customer, etc.)

2. **Account Management**

   * Create new bank accounts linked to registered users
   * Supports multiple account types (e.g., Savings, Current)
   * Branch/IFSC code handling

3. **Transactions**

   * Transfer money between accounts
   * Maintain transaction history (debit, credit, transfers)
   * Balance tracking with validations

4. **User Profile Management**

   * View and update personal profile details
   * Retrieve user dashboard information (account number, balance, transactions)

5. **Role Management**

   * Add and remove roles for users
   * API endpoints to manage user permissions

---

## 3. Tech Stack

* **Framework:** ASP.NET Core Web API
* **Authentication:** JWT, ASP.NET Core Identity
* **Database:** SQL Server (EF Core – Code First)
* **Architecture:** API–Client separation with Repository & DTO pattern
* **Client Integration:** ASP.NET Core MVC client

---

## 4. Example Modules

* **AuthController** → Handles login, registration, and JWT token generation
* **AccountsController** → Manages bank accounts and balances
* **TransactionsController** → Handles transfers and transaction history
* **ProfileController** → Provides profile viewing and editing
* **RolesController** → Manages user roles and access levels

---

This API serves as the backend foundation for the **SimpleBank Client (MVC app)**, providing all necessary endpoints for a seamless banking experience.

---

# II. RESTful design
**SimpleBank API** is **RESTful** in design 

1. **Resource-based Endpoints**

   * Each core entity (Accounts, Transactions, Users, Roles) is represented as a resource with its own endpoint.
   * Example:

     * `POST /api/Auth/login` → authenticate user
     * `GET /api/Accounts/{id}` → get account details
     * `POST /api/Transactions/transfer` → make a transfer

2. **HTTP Verbs Semantics**

   * `GET` → retrieve resources (e.g., account balance, transaction history)
   * `POST` → create resources (e.g., register user, create transaction)
   * `PUT` / `PATCH` → update resources (e.g., update profile, edit account)
   * `DELETE` → remove resources (e.g., delete role, close account)

3. **Statelessness**

   * Authentication uses **JWT tokens**, meaning each request carries the necessary credentials.
   * The server does not store client session state — it only validates tokens.

4. **Structured Responses (DTOs)**

   * API responses are JSON-based, following REST principles for standardized communication.

5. **Separation of Concerns**

   * Backend (API) and frontend (MVC client) are independent.
   * The client consumes the API through HTTP requests, making the system extensible (e.g., mobile apps can also consume it).
---

# 🏛️ **III. SimpleBank – N-Tier Architecture**

An **N-Tier architecture** separates an application into multiple layers (tiers), each with a clear responsibility. In this project, the tiers are organized into four projects:

---

## 1. **Presentation Tier – `SimpleBank`**

* This is the **ASP.NET Core Web API project**.
* It contains the **controllers** that expose RESTful endpoints.
* The controllers **do not contain business logic** — they just:

  1. Accept requests (DTOs).
  2. Call the appropriate **Service layer methods**.
  3. Return responses (DTOs) back to the client.

Example: AccountController

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Migrations;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Interface;

namespace SimpleBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("GetDashboard/{accountNumber}")]
        public async Task<IActionResult> GetDashboard(string accountNumber)
        {
            return Ok(await _accountRepository.GetDashboardDataAsync(accountNumber));
        }

        [HttpGet("GetProfile/{formId}")]
        public async Task<IActionResult> GetProfile(int formId)
        {
            return Ok(await _accountRepository.GetProfileByFormId(formId));
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
        {
            return Ok(await _accountRepository.UpdateProfileByFormId(profileDto));
        }

        [HttpGet("GetAllUsersWithDetails")]
        public async Task<IActionResult> GetAllUsersWithDetails()
        {
            return Ok(await _accountRepository.GetAllUsersWithDetailsAsync());
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            return Ok(await _accountRepository.GetUserByIdAsync(userId));
        }

        [HttpPost("DeleteUserById/{userId}")]
        public async Task<IActionResult> DeleteUserById(string userId)
        {
            return Ok(await _accountRepository.DeleteUserByIdAsync(userId));
        }
    }
}

```

---

## 2. **Business Tier – `Service`**

* Implements the **business logic** of the bank.
* Uses **interfaces** to define contracts (e.g., `IAccountService`, `ITransactionService`).
* Applies **validations, rules, and workflows**.
* Calls the **Data Access Layer (Entity + Repository)** for persistence.

Example: AccountRepository

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Data;
using SBapi.Entity.Models;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Interface;

namespace SBapi.Service.Repository.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Account>> CreateAccountAsync(decimal initialBalance)
        {
            Result<Account> result = new Result<Account>();
            try
            {
                var value = await _context.Set<SequenceValue>()
                    .FromSqlRaw("EXEC dbo.GetNextAccountNumberSequenceValue")
                    .AsNoTracking().ToListAsync();

                var accountNumber = value.First().Value;

                Account newAccount = new Account
                {
                    AccountNumber = accountNumber.ToString(),
                    Balance = initialBalance,
                };

                await _context.AccountSet.AddAsync(newAccount);
                await _context.SaveChangesAsync();

                result.Response = newAccount;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new Errors
                {
                    ErrorCode = "DB500",
                    ErrorMessage = ex.Message
                });
            }
            return result;
        }
    }
}
```

---

## 3. **Data Access Tier – `SimpleBank.Entity`**

* Represents the **Domain + Data Layer**.
* Contains:

  * **Entity classes (EF Core Models)** → `Account`, `Transaction`, `AppUser`, `ApplicationForm`
  * **DbContext** → EF Core context for persistence

Example ApplicationDbContext: AppDbContext

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SBapi.Common.Dto;
using SBapi.Entity.Models;
using SBapi.Entity.Security;

namespace SBapi.Entity.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<AccountType>().HasIndex(x => x.TypeName).IsUnique();

            // Seed the database with a SuperAdmin user and role
            AppUser user = new()
            {
                Id = "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                UserName = "superadmin@gmail.com",
                NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                IsActive = true,
            };

            string password = "SuperAdmin@123";
            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, password);
            modelBuilder.Entity<AppUser>().HasData(user);

            IdentityRole role = new()
            {
                Id = "b1c2d3e4-f5g6-h7i8-j9k0-l1m2n3o4p5q6",
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = role.Id
            });

            // IFSC Sequence for Branches
            modelBuilder.Entity<SequenceValue>().HasNoKey().ToView(null);

            modelBuilder.HasSequence<int>("IFSCSequence", schema: "dbo")
                .StartsAt(005993)
                .IncrementsBy(1);
            modelBuilder.Entity<Branch>()
                .Property(b => b.IFSC)
                .HasMaxLength(50);

            // AccountNumber Sequence for Accounts
            modelBuilder.HasSequence<int>("AccountNumberSequence", schema: "dbo")
                .StartsAt(11235813)
                .IncrementsBy(1);
            modelBuilder.Entity<Account>()
                .Property(b => b.AccountNumber)
                .HasMaxLength(50);

            // ApplicationForm - Store Enum as String 
            modelBuilder.Entity<ApplicationForm>()
                .Property(f => f.Status)
                .HasConversion<string>()
                .HasMaxLength(20); 
        }

        public DbSet<Branch> BranchSet { get; set; }
        public DbSet<ApplicationForm> ApplicationFormSet { get; set; }
        public DbSet<AccountType> AccountTypeSet { get; set; }
        public DbSet<Account> AccountSet { get; set; }
        public DbSet<Transactions> TransactionsSet { get; set; }
    }
}
```


Example Entity: Account

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBapi.Entity.Models
{
    [Table("tblAccount")]
    public class Account
    {
        [Key]
        [MaxLength(20)]
        public required string AccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
```

---

## 4. **Shared Tier – `Common`**

* Provides **shared utilities** across all layers.
* Typically contains:

  * **DTOs (Data Transfer Objects)** for communication between layers
  * **Response Models** (e.g., `ApiResponse`)

Example DTO: UserResponse

```csharp
using System.ComponentModel.DataAnnotations;

namespace SBapi.Common.Dto
{
    public class UserResponse
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public bool IsActive { get; set; }

        // AccountNumber & FormId
        public required string AccountNumber { get; set; }
        public int FormId { get; set; }

        // roles is a collection of Roles, which contains RoleName
        public ICollection<Roles> roles  { get; set; } = new List<Roles>();
    }
    public class Roles
    {
      public required string RoleName { get; set; }
    }
}

```

Example Result Wrapper:

```csharp
namespace SBapi.Common.ErrorDto
{
    public abstract class Result
    {
        public List<Errors> Errors { get; set; } = new List<Errors>();
        public bool isError => Errors != null && Errors.Any();
    }

    public class Result<T> : Result
    {
        public T ?Response { get; set; }
        public string? WarningMessage { get; set; }
    }
}

```

---

## **Flow of Control (N-Tier)**

1. **Client (MVC / Postman / Mobile)** → sends request
2. **Presentation (SimpleBank)** → Controller receives request & calls Service
3. **Business (Service)** → Applies business rules & calls Repository
4. **Data (Entity)** → Repository interacts with DbContext → SQL Server
5. **Response** → Data returned → Service → Controller → Client

---

## Advantages of This N-Tier Design

* **Separation of Concerns** → Each project has a clear responsibility
* **Reusability** → Entities & DTOs can be reused across services
* **Testability** → Service layer can be unit tested with mocks
* **Maintainability** → Easier to update or replace one layer without affecting others

---

