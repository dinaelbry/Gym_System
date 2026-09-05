# Gym System MVC

A gym management system built with ASP.NET Core MVC and .NET 9, following an N-Tier Architecture.

This project was developed during my training at Route Academy, with a focus on clean architecture, separation of concerns, design patterns, and real-world business rules.

## Architecture

The solution is divided into three independent Class Libraries:

- GymSystem — Presentation Layer
  - Controllers
  - Views

- GymSystem.BLL — Business Logic Layer
  - Services
  - ViewModels
  - Business rules

- GymSystem.DAL — Data Access Layer
  - Entities
  - Repositories
  - DbContext

## Design Patterns & Practices

### Generic Repository Pattern

A generic repository is used for common data access operations, with specialized repositories for specific entities:

- IMembershipRepository
- ISessionRepository
- IBookingRepository

### Unit of Work

The Unit of Work coordinates repository operations and provides access to repositories through:

`GetRepository<TEntity>()`

This keeps database operations organized and helps maintain a single transaction scope.

### AutoMapper

AutoMapper is used for Entity ↔ ViewModel mapping.

All mappings are centralized in a single `MappingProfile`.

### Result Pattern

Create, Update, and Delete operations return a `Result` record instead of a simple `bool`.

The result contains:

- Success
- Error
- Kind

This allows the application to return meaningful failure reasons and keeps business logic separate from controller logic.

## Authentication & Authorization

The system uses ASP.NET Core Identity with four roles.

Authorization is applied at the individual Action level to provide more precise access control.

| Role | Access |
|------|--------|
| SuperAdmin | Full access, including adding and deleting Trainers |
| Admin | Full access except adding or deleting Trainers. Can edit Trainers |
| Receptionist | Add Members, manage Bookings and Memberships, and view Plans and Sessions |
| Member | Access to their own account, subscription status, and upcoming sessions |

## Features

### Members

- Member CRUD operations
- Profile photo upload
- Separate health record page

### Trainers

- Trainer CRUD operations
- Role-based access control

### Plans

- Plan CRUD operations
- Active / Inactive status
- Business rules prevent editing or deleting plans with active memberships

### Sessions

- Session CRUD operations
- Automatic session status calculation:
  - Upcoming
  - Ongoing
  - Completed

### Bookings

- Book a session
- Cancel a booking
- Track attendance
- Mark members as attended for ongoing sessions

### Memberships

- Subscribe members to plans
- Automatically calculate membership end dates
- Validate membership-related business rules

### Dashboard

The home dashboard displays live statistics calculated directly from the database.

## Business Rules

The system enforces business rules at the application level.

Examples:

- A plan with active members cannot be edited or deleted.
- A session that has already started cannot be booked.
- Attendance can only be marked for ongoing sessions.
- Access to Trainer operations depends on the user's role.
- Members can only access their own account information.

## Tech Stack

- ASP.NET Core MVC
- .NET 9
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- AutoMapper
- C#

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
