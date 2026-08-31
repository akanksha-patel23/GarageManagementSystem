Project Rules - Garage Management System

Full conventions live in .roo/rules/. This file is just an index.

01-layering-and-architecture.md — UI -> BAL -> DAL -> DB; Controller -> Service -> Repository
02-database-and-sql.md — stored procedures, transactions, connection handling, db_script.sql
03-error-handling.md — TRY/CATCH + GLOG.spLogDBErrorDetail pattern, ProblemDetails
04-webforms-conventions.md — ViewState, UpdatePanel/Wizard, validators
05-api-conventions.md — CORS, DTOs, async, Dapper
06-naming-and-style.md — CSS prefixes, casing, status constants
07-workflow-checks.md — pre-generation checklist, out-of-scope items
08-environment.md — IIS hosting, domains, no localhost
09-language-usage.md — C#/VB.NET project boundaries
File upload limits and GridView paging: see 04-webforms-conventions.md