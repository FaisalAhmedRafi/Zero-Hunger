# Zero Hunger

A web application that connects **restaurants with surplus food** to an **NGO's collection team**, so good food gets delivered to people who need it instead of being thrown away. Built with ASP.NET MVC 5 and Entity Framework (Database First).

Inspired by the UN Sustainable Development Goal 2: *Zero Hunger*.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [How It Works](#how-it-works)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Database Schema](#database-schema)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Known Limitations](#known-limitations)
- [Roadmap](#roadmap)

---

## Overview

Restaurants often have leftover food that is still safe to eat. Zero Hunger gives them a simple way to post a **collection request** (what food is available and until when). NGO staff see every incoming request in a dashboard, accept it, assign an employee to pick it up, and mark it as delivered once the food has been collected.

There are two types of users:

| Role | What they do |
|------|--------------|
| **Restaurant** | Creates a food collection request with a food type and an "available until" date/time |
| **Employee (NGO staff)** | Reviews requests, accepts them, assigns a collector, and confirms delivery |

---

## Features

- Role-based login (Employee / Restaurant)
- Restaurants can create food collection requests
- Employee dashboard listing all requests with restaurant name, address, food, deadline, status and assigned employee
- Assign a collection employee to each request
- Request status workflow: **Pending → Accepted → Delivered**
- Clean separation using DTOs and AutoMapper between the database entities and the views

---

## How It Works

```
Restaurant                         NGO Employee
    |                                    |
    |  1. Creates request                |
    |  (food + available until)          |
    |----------------------------------> |  Status: Pending
    |                                    |
    |                                    |  2. Selects employee, clicks "Accept"
    |                                    |  Status: Accepted
    |                                    |
    |                                    |  3. Food collected, clicks "Confirm"
    |                                    |  Status: Delivered
```

---

## Tech Stack

- **Framework:** ASP.NET MVC 5 on .NET Framework 4.7.2
- **Language:** C#
- **ORM:** Entity Framework 5 (Database First, EDMX model)
- **Database:** Microsoft SQL Server
- **Mapping:** AutoMapper 10
- **Front end:** Razor views, Bootstrap 5.2.3, jQuery 3.7
- **IDE:** Visual Studio 2022

---

## Project Structure

```
Zero-Hunger/
├── WebApplication4.sln
└── WebApplication4/
    ├── App_Start/          # Route, bundle and filter configuration
    ├── Controllers/
    │   ├── HomeController.cs
    │   ├── LoginController.cs       # Login for employees and restaurants
    │   └── DashboardController.cs   # View/create/update collection requests
    ├── DTOs/               # Data transfer objects (CollectRequest, Employee, Food, Restaurant)
    ├── EF/                 # Entity Framework model (Model1.edmx) and generated entities
    ├── Models/             # Login view model
    ├── Views/
    │   ├── Home/           # Index, About, Contact
    │   ├── Login/          # Login page
    │   ├── Dashboard/      # ViewRequest (employee), CreateRequest (restaurant)
    │   └── Shared/         # _Layout
    ├── Content/            # CSS (Bootstrap + Site.css)
    ├── Scripts/            # JavaScript libraries
    └── Web.config          # App configuration and connection string
```

---

## Database Schema

The app uses a SQL Server database named **`NGO`** with four tables:

| Table | Columns |
|-------|---------|
| **Foods** | `f_id` (PK, identity), `f_name` |
| **Resturants** | `r_id` (PK, identity), `r_name`, `r_address`, `food_id` (FK → Foods) |
| **Employees** | `e_id` (PK, identity), `e_name`, `e_location`, `e_designation` |
| **CollectRequests** | `creq_id` (PK, identity), `creq_r_id` (FK → Resturants), `creq_f_id` (FK → Foods), `creq_max_time`, `creq_status`, `creq_e_id` (FK → Employees, nullable) |

> The table name `Resturants` is spelled this way in the code and the EDMX model. Keep the spelling if you create the tables yourself.

### Relationships

- A **Restaurant** has many **CollectRequests**
- A **Food** is referenced by many **Restaurants** and many **CollectRequests**
- An **Employee** can be assigned to many **CollectRequests**

---

## Getting Started

### Prerequisites

- Windows with **Visual Studio 2022** (ASP.NET and web development workload)
- **.NET Framework 4.7.2** developer pack
- **SQL Server** (Express or LocalDB is fine) and SQL Server Management Studio

### 1. Clone the repository

```bash
git clone https://github.com/<your-username>/Zero-Hunger.git
cd Zero-Hunger
```

### 2. Create the database

The repository does not include a SQL script, so create the database and tables manually:

```sql
CREATE DATABASE NGO;
GO
USE NGO;
GO

CREATE TABLE Foods (
    f_id   INT IDENTITY(1,1) PRIMARY KEY,
    f_name VARCHAR(50) NOT NULL
);

CREATE TABLE Employees (
    e_id          INT IDENTITY(1,1) PRIMARY KEY,
    e_name        VARCHAR(50) NOT NULL,
    e_location    VARCHAR(50) NOT NULL,
    e_designation VARCHAR(50) NOT NULL
);

CREATE TABLE Resturants (
    r_id      INT IDENTITY(1,1) PRIMARY KEY,
    r_name    VARCHAR(50) NOT NULL,
    r_address VARCHAR(50) NOT NULL,
    food_id   INT NOT NULL,
    CONSTRAINT FK_Resturants_Foods FOREIGN KEY (food_id) REFERENCES Foods(f_id)
);

CREATE TABLE CollectRequests (
    creq_id       INT IDENTITY(1,1) PRIMARY KEY,
    creq_r_id     INT NOT NULL,
    creq_f_id     INT NOT NULL,
    creq_max_time DATETIME NOT NULL,
    creq_status   VARCHAR(50) NOT NULL,
    creq_e_id     INT NULL,
    CONSTRAINT FK_CollectRequests_Resturants FOREIGN KEY (creq_r_id) REFERENCES Resturants(r_id),
    CONSTRAINT FK_CollectRequests_Foods      FOREIGN KEY (creq_f_id) REFERENCES Foods(f_id),
    CONSTRAINT FK_CollectRequests_Employees  FOREIGN KEY (creq_e_id) REFERENCES Employees(e_id)
);
```

### 3. Add sample data

The demo login (see [Usage](#usage)) expects an **employee with ID 2** and a **restaurant with ID 4**, so insert enough rows to create those IDs:

```sql
INSERT INTO Foods (f_name) VALUES ('Rice'), ('Biryani'), ('Bread'), ('Curry');

INSERT INTO Employees (e_name, e_location, e_designation) VALUES
('Employee One', 'Dhaka', 'Collector'),
('Employee Two', 'Dhaka', 'Collector'),
('Employee Three', 'Dhaka', 'Collector');

INSERT INTO Resturants (r_name, r_address, food_id) VALUES
('Restaurant A', 'Address A', 1),
('Restaurant B', 'Address B', 2),
('Restaurant C', 'Address C', 3),
('Restaurant D', 'Address D', 4);
```

### 4. Configure the connection string

Open `WebApplication4/Web.config` and check the `NGOEntities` connection string. By default it points to a local SQL Server instance using Windows authentication:

```
data source=.;initial catalog=NGO;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True
```

Change `data source=.` if you use a named instance (for example `.\SQLEXPRESS` or `(localdb)\MSSQLLocalDB`).

### 5. Run the app

1. Open `WebApplication4.sln` in Visual Studio.
2. Restore NuGet packages (right-click the solution → **Restore NuGet Packages**).
3. Press **F5** (or **Ctrl+F5**) to run with IIS Express.

---

## Usage

### Login

Go to `/Login/Login` and sign in with one of the demo accounts:

| Role | User ID | Password |
|------|---------|----------|
| Employee | `2` | `1234` |
| Restaurant | `4` | `1234` |

### As a restaurant

1. Open `/Dashboard/CreateRequest/4` (the number is the restaurant ID).
2. Pick the date/time until which the food is available and select the food type.
3. Submit. The request is saved with the status **Pending**.

### As an employee

1. Log in as an employee to open the request dashboard (`/Dashboard/ViewRequest`).
2. Choose an employee from the dropdown and click **Accept**. The status becomes **Accepted**.
3. After pickup, click **Confirm**. The status becomes **Delivered**.

---

## Known Limitations

This project is a learning/demo build, so a few things are intentionally simple:

- **Hard-coded login:** credentials are checked against fixed values in `LoginController` instead of a users table. There is no password hashing, session handling or `[Authorize]` protection on the dashboard.
- **Registration is not implemented:** the Register button on the login page does nothing yet.
- **Restaurant redirect:** after a restaurant logs in, the redirect to `CreateRequest` does not pass the restaurant ID, so open `/Dashboard/CreateRequest/<id>` directly.
- **Placeholder pages:** Home, About and Contact still contain the default ASP.NET template content.
- **No SQL script** is bundled. The schema above has to be created manually.

---

## Roadmap

- [ ] Proper authentication and authorization (ASP.NET Identity or cookie auth, hashed passwords)
- [ ] Restaurant and employee registration
- [ ] Redirect restaurants to their own dashboard after login
- [ ] Restaurant view to track the status of their own requests
- [ ] Quantity, pickup address and contact details on requests
- [ ] Input validation and error handling
- [ ] Replace the default template pages with real home/about/contact content
- [ ] Include a database script or EF migrations

---

## License

This project is for educational purposes. Add a license of your choice (for example MIT) before distributing.
