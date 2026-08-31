## Layering — never break this
- UI (Web Forms .aspx/.aspx.cs) -> BAL -> DAL -> DB. No SQL, no SqlCommand, no business rules in code-behind. No business logic inside the DAL (DAL = data access only).
- Web API layer: Controller -> Service -> Repository (Dapper). Controllers contain no query logic; Repositories contain no business rules.
- JobCard creation (header + service lines + part lines + stock decrement) happens entirely through the API layer (POST /api/jobcards), not the Web Forms DAL — this is the deliberate contrast case against the ADO.NET/postback pattern used elsewhere. Web Forms JobCardDAL only covers listing, viewing, and status updates.

## DAL return types — DataTable only, never domain Entities

GarageManagement.Web.DAL (VB.NET) and GarageManagement.Web (C#, owns Entities/) are separate projects; GarageManagement.Web already references the DAL project, so the DAL cannot reference back (circular reference, will not compile).

Every DAL method returns DataTable (for reads) or raw output parameters (for writes) — never a typed Entity (Customer, Vehicle, etc.).
The BAL (same project as Entities/) is responsible for mapping DataTable rows into Entity objects. This also matches "DAL = data access only" — modeling is a BAL concern, not a DAL one.
See 09-language-usage.md for the project-reference direction this depends on.