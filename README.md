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

