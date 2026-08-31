\# Gym System MVC



Gym management system built with \*\*ASP.NET Core MVC\*\* and \*\*.NET 9\*\*, following an \*\*N-Tier Architecture\*\*. Built during training at Route Academy.



\## Architecture



The solution is split into 3 independent Class Libraries:



\- \*\*GymSystem\*\* (Presentation) — Controllers \& Views

\- \*\*GymSystem.BLL\*\* (Business Logic) — Services \& ViewModels

\- \*\*GymSystem.DAL\*\* (Data Access) — Entities, Repositories \& DbContext



\### Patterns



\- \*\*Generic Repository Pattern\*\* with specialized repositories (`IMembershipRepository`, `ISessionRepository`, `IBookingRepository`)

\- \*\*Unit of Work\*\* to aggregate repositories via `GetRepository<TEntity>()`

\- \*\*AutoMapper\*\* for all entity ↔ ViewModel mapping, centralized in a single `MappingProfile`

\- \*\*Result Pattern\*\* — a `Result` record (`Success`, `Error`, `Kind`) returned from Create/Update/Delete operations instead of a plain `bool`, so failures carry a clear reason



\## Roles \& Permissions



Built on \*\*ASP.NET Core Identity\*\* with 4 roles, each restricted at the individual Action level rather than just the Controller:



| Role | Access |

|---|---|

| \*\*SuperAdmin\*\* | Full access, including adding and deleting Trainers |

| \*\*Admin\*\* | Full access except adding or deleting Trainers (can edit) |

| \*\*Receptionist\*\* | Add Members, manage Bookings \& Memberships, view-only on Plans/Sessions |

| \*\*Member\*\* | Own account only — subscription status and upcoming sessions via `/MyAccount` |



\## Features



\- \*\*Members\*\* — CRUD, photo upload, separate health record page

\- \*\*Trainers\*\* — CRUD

\- \*\*Plans\*\* — CRUD, Active/Inactive toggle (blocked while members are currently subscribed)

\- \*\*Sessions\*\* — CRUD, with a computed status (`Upcoming` / `Ongoing` / `Completed`)

\- \*\*Bookings\*\* — book/cancel, attendance tracking (`Mark Attended`) for ongoing sessions

\- \*\*Memberships\*\* — subscribe a member to a plan, end date computed automatically

\- \*\*Home dashboard\*\* — live stats computed from the database



All of it enforced with real business rules — e.g. a plan with active members can't be edited or deleted, a session that's already started can't be booked.



\## Tech Stack



ASP.NET Core MVC · .NET 9 · Entity Framework Core · SQL Server · ASP.NET Core Identity · AutoMapper



\## Getting Started



1\. Clone the repo

2\. Add your SQL Server connection string to `GymSystem/appsettings.Development.json`:

&#x20;  ```json

&#x20;  {

&#x20;    "ConnectionStrings": {

&#x20;      "DefaultConnection": "Server=YOUR\_SERVER;Database=GymSystem;Trusted\_Connection=True;TrustServerCertificate=True"

&#x20;    }

&#x20;  }

&#x20;  ```

3\. Run migrations: `Update-Database` (or `dotnet ef database update`)

4\. Run the project — a SuperAdmin account is seeded automatically on first launch

