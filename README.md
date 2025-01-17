
# SahaflarPazari

An N-layered ASP.NET MVC application for an online used-books marketplace. The project includes **Identity**-based authentication (instead of a custom `RoleProvider`), a **clean** Domain-Infrastructure-Application-Web separation, and **AJAX** endpoints for dynamic interactions like address management, file uploads, etc.

## Table of Contents
- [Overview](#overview)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Setup & Installation](#setup--installation)
- [Important Changes](#important-changes)
- [Usage](#usage)
- [Known Issues](#known-issues)
- [License](#license)



## Overview
**SahaflarPazari** is designed to manage books, addresses, user profiles, orders, and more, implementing a multi-layer architecture. We have replaced older `RoleProvider` logic with **ASP.NET Identity** for robust authentication and role handling, introduced **AJAX** for smooth front-end interactions, and established a consistent approach to file uploads (like book images).


## Architecture

### **Solution Structure**
```
SahaflarPazari
├─ SahaflarPazari.Domain
│   ├─ Entities (Address, Book, User, etc.)
│   └─ Interfaces (IUnitOfWork, IRepository, etc.)
├─ SahaflarPazari.Infrastructure
│   ├─ Data (DbContext classes, IdentityDbContext, EF Migrations)
│   ├─ Identity (ApplicationUserManager, SignInManager, etc.)
│   └─ Repositories (Repository<T>, UnitOfWork, custom repos)
├─ SahaflarPazari.Application (optional layer for business services)
└─ SahaflarPazari.Web (ASP.NET MVC)
    ├─ Controllers (Account, Book, Profile, etc.)
    ├─ Models (ViewModels, DTOs)
    ├─ Views (Razor pages)
    └─ Scripts & CSS (AJAX calls, Bootstrap, etc.)
```

1. **Domain** – Core entities, domain interfaces.
2. **Infrastructure** – EF contexts, Identity, Repositories, migrations.
3. **Application** – (Optional) service logic or use-cases (not always used).
4. **Web** – MVC controllers, Razor Views, front-end assets (JS, CSS).



## Prerequisites
- **.NET Framework 4.7.2** (or higher) installed
- **SQL Server** or compatible DB
- **Visual Studio** 2017/2019/2022 (or relevant .NET developer tools)
- **NuGet** packages restored


## Setup & Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/YourUsername/SahaflarPazari.git
   ```

2. **Open** the solution in **Visual Studio**.

3. **Restore NuGet packages** (automatically or via Package Manager Console).

4. **Apply EF migrations**:
   ```powershell
   Update-Database -ProjectName Infrastructure -StartupProjectName SahaflarPazari
   ```
   This creates the required tables (e.g., `Books`, `Addresses`, `AspNetUsers`, `AspNetRoles`, etc.).

5. **Build** and **run** the project. The default start URL (e.g. `localhost:12345`) launches the MVC app.


## Important Changes

1. **Removed `RoleProvider`**  
   - We now rely on **ASP.NET Identity** (`ApplicationUserManager`, `ApplicationSignInManager`, `ApplicationUser`) with the standard Identity tables (`AspNetUsers`, `AspNetRoles`, etc.).
2. **Address CRUD** via **AJAX**  
   - Endpoints in `ProfileController` handle `AddAddress`, `UpdateAddressInfo`, `DeleteAddress`.
3. **UnitOfWork** & Repositories  
   - All DB operations go through `_unitOfWork`. 
   - For example, `_unitOfWork.Books.AddAsync(book)`, `_unitOfWork.CommitAsync()`.


## Usage

1. **Run** the project.  
2. **Register** a new user or log in with existing credentials.  
3. **Add** addresses under Profile → Addresses.  
4. **Add** books from Books → AddBook (upload a cover image).  
5. **Admin** role can see `Admin` panel to manage users or complaints (depending on your setup).


## Known Issues
- If the **`bin\roslyn\csc.exe`** file is missing, reinstall `Microsoft.CodeDom.Providers.DotNetCompilerPlatform`.
- Deploying to **IIS** may require explicit writing rights on `~/Content/img/Kitap_Resimleri` for image uploads.
- Certain Identity errors might return `string.Empty` instead of mapping to a property field. We handle them as general errors.

---
