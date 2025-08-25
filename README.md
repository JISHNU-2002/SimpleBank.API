# 📌 SimpleBank API – Overview

The **SimpleBank API** is a RESTful web service built with **ASP.NET Core Web API** using a **Code-First approach**. It is designed to provide core banking functionalities such as **account management, transactions, authentication, and user profile handling**. The API follows modern architectural practices, including **JWT authentication**, **repository pattern**, and **DTO-based data exchange**, ensuring security, scalability, and maintainability.

---

## 🔑 Key Features

1. **Authentication & Authorization**

   * Secure login and registration using **ASP.NET Core Identity**
   * **JWT-based authentication** for client-server communication
   * Role-based access control (Admin, User, etc.)

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

### ⚙️ Tech Stack

* **Framework:** ASP.NET Core Web API
* **Authentication:** JWT, ASP.NET Core Identity
* **Database:** SQL Server (EF Core – Code First)
* **Architecture:** API–Client separation with Repository & DTO pattern
* **Client Integration:** ASP.NET Core MVC client

---

### 📂 Example Modules

* **AuthController** → Handles login, registration, and JWT token generation
* **AccountsController** → Manages bank accounts and balances
* **TransactionsController** → Handles transfers and transaction history
* **ProfileController** → Provides profile viewing and editing
* **RolesController** → Manages user roles and access levels

---

👉 This API serves as the backend foundation for the **SimpleBank Client (MVC app)**, providing all necessary endpoints for a seamless banking experience.

---
