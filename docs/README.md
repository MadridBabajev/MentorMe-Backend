## About the Project

This project was built as part of the recruitment process for my very first job as a software developer.

It shares a lot in common with my [**MentorMe**](https://github.com/MadridBabajev/MentorMe-Backend) project, mostly because the home task’s required tech stack and structure were close enough that I could reuse a solid template (thankfully).

The key difference is scope and polish: this one is more concise, neatly organized, and built with a bit more caution and attention to detail compared to MentorMe.

I defended this project through the technical interview round and secured my first position as a **Full-Stack Developer**, something I’m still genuinely proud of to this day.

At its core, this is a backend API for a shopping cart application, built with **ASP.NET Core** and structured around **Clean Architecture** principles.

The project demonstrates a layered architecture with clear separation of concerns:

- **Domain Layer** — Core entities
- **Data Access Layer (DAL)** — Repository pattern powered by Entity Framework Core
- **Business Logic Layer (BLL)** — Services and business rules
- **Public Layer** — RESTful API controllers guarded with JWT authentication

The backend is containerized with **Docker** and uses **PostgreSQL** as its database, so it’s quick to spin up locally across machines.

---

## Technologies Used

- **C# 12**
- **.NET 8.0**
- **ASP.NET Core Web API**
- **xUnit**
- **Entity Framework Core**
- **PostgreSQL**
- **Docker & Docker Compose**
- **JWT Authentication**
- **Swagger**
- **AutoMapper**

---

## Running the Project

### Prerequisites

1. **Install .NET SDK 8.0.401**  
   Download: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

2. **Install Docker Desktop**  
   Make sure Docker is running on your machine.

3. **Install EF Core Tools** (for migrations)
   ```bash
   dotnet tool install --global dotnet-ef
   # OR update if already installed
   dotnet tool update --global dotnet-ef
   ```

### Quick Start with Docker

The easiest way to run the project:

```bash
docker-compose up --build
```

Go to `App.DAL/Seeding/AppDataInit.cs` line 73 to view seeded users and authorize faster.

The API will be available at `http://localhost:8000`.

### Local Setup

If you prefer running outside Docker:

1. **Start the PostgreSQL container:**
   ```bash
   docker-compose up shopping-cart-postgres -d
   ```

2. **Apply database migrations:**
   ```bash
   dotnet ef database update --project App.DAL --startup-project WebApp
   ```

3. **Run the application:**
   ```bash
   cd WebApp
   dotnet run
   ```


**INFO: When the application is launching, it will try to seed the data**, which will fail if the database container is not running and it hasn't been migrated.

### Creating New Migrations

When you make changes to the domain entities:

```bash
dotnet ef migrations add <MigrationName> --project App.DAL --startup-project WebApp
dotnet ef database update --project App.DAL --startup-project WebApp
```

---

## API Endpoints

The API includes Swagger documentation. Once the application is running, visit:
- **Swagger UI:** `http://localhost:80/swagger`

Key endpoints include:
- **Shop Items** - CRUD operations for products
- **Identity** - User registration and JWT authentication

<img src="./docs/swagger-endpoints.png" alt="Conveyor GIF" width="1483" height="1262" />

---

## Application snippets

You can see the application in action with [Frontend](https://github.com/MadridBabajev/ShoppingCart-Frontend) running locally.

The snippets are also included in the Frontend's `README.md`.

---

## Related

- [**ShoppingCart Backend**](https://github.com/MadridBabajev/ShoppingCart-Backend)

---

_Madrid Babajev (08.02.2026)_
