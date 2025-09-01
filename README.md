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
﻿namespace SBapi.Common.ErrorDto
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


# 📂 IV. Main Files in SimpleBank API

---

## 1. **`appsettings.json`**

* Stores **configuration settings** for the API.
* Common sections:

  * **ConnectionStrings** → database connection for EF Core
  * **JWT Settings** → secret key, issuer, expiry for authentication
  * **Logging & Environment configs**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\SQLSERVER2019;Database=SimpleBankDB;User Id=sa;Password=Password123;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ApiSecurity": {
    "ClientId": "admin@gmail.com",
    "ClientSecret": "admin@123"
  },
  "Jwt": {
    "Issuer": "Jwt@Test",
    "Audience": "Jwt@Tester",
    "Key": "7c3f9030-2fd1-4e48-9bc4-8e3f4c61ad41",
    "Subject": "JWTServiceAccessToken"
  },
  "AllowedHosts": "*",
  "Serilog": {
    "Using": [ "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information"
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "logs/webapi-.log",
          "rollingInterval": "Day",
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} {Level:u3}] {Username} {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

- Keeps sensitive info (like DB connection, JWT keys) **outside code** → easier to change per environment (Dev/Prod).
- **Serilog** is used for Logging.


```csharp
"ApiSecurity": {
  "ClientId": "admin@gmail.com",
  "ClientSecret": "admin@123"
}
```

- This section defines static client credentials (like username & password for the API itself).
- Often used in Client Credentials Flow (machine-to-machine authentication).
- Here, a basic API authentication mechanism where requests must provide:
    - ClientId → like a username (here, admin@gmail.com)
    - ClientSecret → like a password (admin@123)
    
- When a client (MVC app) requests a JWT token, it first sends these credentials.
- The API checks if ClientId and ClientSecret match the config.
- If valid → the API issues a JWT token for further requests.

## **JWT Authentication**
- This section defines the configuration for JWT (JSON Web Tokens), which the API uses for stateless authentication.
- ApiSecurity → like a gatekeeper (client id & secret) → only trusted apps can request tokens.
- Jwt → defines the actual token authentication system → how tokens are created and validated for each request.

```csharp
"Jwt": {
  "Issuer": "Jwt@Test",
  "Audience": "Jwt@Tester",
  "Key": "7c3f9030-2fd1-4e48-9bc4-8e3f4c61ad41",
  "Subject": "JWTServiceAccessToken"
}
```

**Keys explained**
- Issuer (Jwt@Test) → The authority that issues the token (your API).
- Audience (Jwt@Tester) → Who the token is intended for (your client apps).
- Key → The secret key used to sign and validate tokens (must be long and secure).
- Subject (JWTServiceAccessToken) → Optional claim describing the token’s purpose.

**Work Flow**
- When a user/client logs in successfully:
    - API generates a JWT signed with the Key.
    - The JWT includes claims (like AccountNumber, Role).
- The client must send this token in the Authorization: Bearer <token> header for every request.
- The API verifies the token’s signature using the same Key, and checks Issuer & Audience before processing the request.


**JWT Authentication setup in `Program.cs`**

```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };
});
```


* **`AddAuthentication`** → Enables authentication in the pipeline.
* **`DefaultAuthenticateScheme` & `DefaultChallengeScheme`** → Tells ASP.NET to use **JWT Bearer Authentication** (instead of cookies, OAuth, etc.).
* **`AddJwtBearer`** → Configures **how tokens will be validated**.

Validation rules set here:

* **`ValidateIssuer = true`** → The token’s issuer (who created it) must match config.
* **`ValidateAudience = true`** → The token must be intended for your API (audience check).
* **`ValidateLifetime = true`** → Expired tokens will be rejected.
* **`ValidateIssuerSigningKey = true`** → Token signature must match the configured secret key.
* **`ValidIssuer` & `ValidAudience`** → Read from `appsettings.json` (`Jwt` section).
* **`IssuerSigningKey`** → Symmetric key created from your secret string (`Jwt:Key`).

This ensures that **only tokens issued by your API** (with correct secret, issuer, audience, and still valid) are accepted.


**Token generation method (`GetTokenAsync`)**

```csharp
public async Task<string> GetTokenAsync(UserRequest userRequest)
{
    var username = _configuration["ApiSecurity:ClientId"];
    var password = _configuration["ApiSecurity:ClientSecret"];

    if (!(userRequest.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) 
          && userRequest.Password == password))
    {
        throw new UnauthorizedAccessException("Invalid username or password.");
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        expires: DateTime.UtcNow.AddMinutes(10),
        signingCredentials: signingCredentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

**Work Flow**

1. **Check API credentials (`ApiSecurity`)**

   * Reads `ClientId` and `ClientSecret` from config (`appsettings.json`).
   * Compares with values sent in `UserRequest`.
   * If wrong → throw `UnauthorizedAccessException`.
        - This ensures **only trusted clients** can ask for a token.

2. **Generate JWT token (`Jwt` section)**

   * **Key** → Uses the same `Jwt:Key` as configured in `Program.cs`.
   * **SigningCredentials** → Signs the token with **HMAC-SHA256**.
   * **JwtSecurityToken** → Creates a token with:

     * **Issuer** (`Jwt:Issuer`)
     * **Audience** (`Jwt:Audience`)
     * **Expiry** → 10 minutes
     * **Signing credentials**

3. **Return Token**

   * Uses `JwtSecurityTokenHandler` to **serialize the token into a string**.
   * This string is what the client will use in `Authorization: Bearer <token>` header.

---

**Complete Work Flow of JWT Authentication**

1. **Client requests token** → Calls an endpoint like `/api/auth/token` with `ClientId` & `ClientSecret`.
2. **`GetTokenAsync` runs** → Verifies client → Issues JWT signed with your `Jwt:Key`.
3. **Client calls protected API** → Sends `Authorization: Bearer <token>`.
4. **JWT Middleware (`Program.cs`)** → Automatically validates token against `Issuer`, `Audience`, `Lifetime`, and `Key`.
5. **If valid** → Request passes through to controller. If not → 401 Unauthorized.

---
\
**In short**:

* **`Program.cs` setup** = Gatekeeper → Defines how incoming tokens will be verified.
* **`GetTokenAsync`** = Token factory → Issues tokens signed with your secret key after validating API client credentials.
---

## 2. **`Program.cs`**

* The **entry point** of the application.
* Configures **services** (DI container) and **middleware pipeline**.

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SBapi.Entity.Data;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Implementation;
using SBapi.Service.Repository.Interface;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Entity Framework Core with SQL Server - AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity with AppUser and IdentityRole - Password Rules
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
});

// Configure Serilog for logging
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };
});

builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IAuthorizeRepository, AuthorizeRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IApplicationFormRepository, ApplicationFormRepository>();
builder.Services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

```

This is where **all services are wired up** and the app is bootstrapped.

---

## 3. **Controllers** (e.g., `AccountsController.cs`)

* Define **RESTful endpoints**.
* Call the **Service layer** (not the DB directly).
* Handle **input validation** and return **responses (DTOs)**.

Example: AuthorizeController 

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SBapi.Common.Dto;
using SBapi.Common.ErrorDto;
using SBapi.Entity.Security;
using SBapi.Service.Repository.Interface;

namespace SimpleBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeRepository _authorizeRepository;
        private readonly UserManager<AppUser> _userManager;

        public AuthorizeController(IAuthorizeRepository authorizeRepository,
            UserManager<AppUser> userManager)
        {
            _authorizeRepository = authorizeRepository;
            _userManager = userManager;
        }

        [HttpPost("AuthorizeUser")]
        public async Task<IActionResult> AuthorizeUser(UserRequest userRequest)
        {
            return Ok(await _authorizeRepository.AuthorizeUser(userRequest));
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterRequestDto request)
        {
            return Ok(await _authorizeRepository.RegisterUser(request));
        }
    }
}
```

They are the **entry point for clients** → define what the API exposes.

---

## 4. **`AppDbContext.cs`**

* Inherits from `DbContext` (EF Core).
* Represents the **database session**.
* Defines **DbSets** (tables).
* Configures relationships, keys, constraints.

```csharp
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<ApplicationForm> ApplicationForms { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Account>()
            .HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountId);
    }
}
```

Acts as the **bridge between C# classes and SQL tables**.

---

## 5. **Models (Entities)**

* Represent **database tables** (Domain objects).
* Used by EF Core for persistence.

Example: Branch

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBapi.Entity.Models
{
    [Table("tblBranch")]
    public class Branch
    {
        [Key]
        [MaxLength(20)]
        public required string IFSC { get; set; }
        [MaxLength(50)]
        public string BranchName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;
    }
}
```

They are the **foundation of the domain** (banking data).

---

## 6. **Repositories (`Repos`)**

* Provide **data access layer (DAL)**.
* Encapsulate EF Core queries (no direct DbContext in controllers).
* Follow **Repository Pattern**.

Example:

```csharp
public interface IAccountRepository
{
    Task<Account> GetByAccountNumberAsync(string accountNumber);
    Task CreateAsync(Account account);
    Task SaveChangesAsync();
}

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    public AccountRepository(AppDbContext context) => _context = context;

    public async Task<Account> GetByAccountNumberAsync(string accountNumber) =>
        await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

    public async Task CreateAsync(Account account) =>
        await _context.Accounts.AddAsync(account);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
```

Separates **data logic** from **business logic** → easy testing & maintenance.

---

## 7. **DTOs (Data Transfer Objects)**

* Lightweight objects used for **communication** between layers.
* Prevents exposing raw entity models (security).
* Tailored to client needs.

Example:

```csharp
public class TransferDto
{
    public string FromAccount { get; set; }
    public string ToAccount { get; set; }
    public decimal Amount { get; set; }
}

public class AccountDto
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
}
```

Provide a **clean contract** between API and Client, separate from DB schema.

---

# V. API Testing with Swagger and Postman

### 1. Swagger Setup

ASP.NET Core automatically integrates **Swagger (Swashbuckle)** when enabling it. In your `Program.cs`

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```


This will expose **Swagger UI** at:

```
https://localhost:<port>/swagger/index.html
```

---

### 2. Add JWT Support in Swagger

To test protected endpoints in Swagger, configure it to accept JWT tokens:

```csharp
// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
});
```

Now in Swagger UI, an **Authorize button** is available → paste `Bearer <token>` → all `[Authorize]` endpoints will work.

---

### 3. Testing Flow in Swagger

1. Call **`/api/Token/GetToken`** → enter username/password from `ApiSecurity`.
2. Copy the token returned.
3. Click **Authorize** → paste:

   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
   ```
4. Authorize Button -> Value: (Paste the token) -> Authorize
5. Call secured APIs (like `/api/account/{id}`) → you’ll get results instead of `401 Unauthorized`.

![Testing_Swagger](https://github.com/JISHNU-2002/SimpleBank.API/blob/master/SimpleBank/Images/SimpleBank_Swagger.png)

---

### 4. Testing with Postman

Prefer **Postman** for manual testing:

#### Step 1: Get Token

* **POST** `https://localhost:<port>/api/Token/GetToken`
* Body (JSON):

  ```json
  {
    "userName": "admin@gmail.com",
    "password": "admin@123"
  }
  ```
* Response:

  ```json
  "eyJhbGciOiJIUzI1NiIsInR..."
  ```
#### Step 2: Authorize Token
- Authorization -> Auth Type -> Bearer Token -> (Paste the token)

#### Step 3: Call Protected API

* **GET** `https://localhost:<port>/api/account/{id}`

![Testing_Postman](https://github.com/JISHNU-2002/SimpleBank.API/blob/master/SimpleBank/Images/SimpleBank_Postman.png)

---

With this, **authentication flow is fully tested** in both Swagger and Postman.

---

