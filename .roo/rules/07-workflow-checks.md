## Before generating new code, always check
- Does a stored procedure already exist for this table/action? Do not create a new stored procedure without explicitly asking.
- Does this change affect a shared stored procedure/table used by another feature? (impact check)
- Is test/seed data available to actually exercise this code path?
- Does the response shape match what the existing jQuery caller expects (especially around ProblemDetails/error responses)?

## Out of scope unless explicitly asked
- No ORM in the Web Forms/DAL layer (raw ADO.NET only, by design).
- No architecture changes, no introducing new NuGet packages, without confirmation.
- No authentication/authorization on API endpoints unless explicitly requested — don't add JWT/Identity scaffolding proactively.