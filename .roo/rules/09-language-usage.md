## Language usage: C# and VB.NET
- GarageManagement.Web (Web Forms) is a Web Application Project (.csproj). MSBuild only supports one code-behind language per project — .aspx.cs and .aspx.vb files CANNOT be mixed inside GarageManagement.Web.
- To use both languages, the DAL lives in its own VB.NET Class Library project: GarageManagement.Web.DAL (.vbproj), referenced by GarageManagement.Web (C#).
--- All DAL classes (CustomerDAL.vb, VehicleDAL.vb, ServiceCategoryDAL.vb, ServiceCatalogDAL.vb, PartDAL.vb, TechnicianDAL.vb, JobCardDAL.vb, InvoiceDAL.vb) and DBHelper.vb belong in this project.
--- BAL, Entities, and all .aspx/.aspx.cs code-behind stay in C# inside GarageManagement.Web.
- Do not introduce a third mixed-language project, or move any other layer to VB.NET, without explicit confirmation.
- The Web API project (GarageManagement.Api) stays C# only — Dapper repositories, services, and controllers are not affected by this split.

## Project reference direction
- GarageManagement.Web (C#) must have a project reference to GarageManagement.Web.DAL (VB.NET) so BAL classes can call DAL classes directly.
-GarageManagement.Web.DAL must NOT reference GarageManagement.Web back — doing so creates a circular reference and will not compile.
- Consequence: the DAL cannot know about Entity types (Customer, Vehicle, etc.), since those live in GarageManagement.Web. DAL methods return DataTable/output parameters only; the BAL maps rows into Entities. See 01-layering-and-architecture.md for the full rule.