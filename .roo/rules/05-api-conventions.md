## Web API specifics
- CORS: allow only http://www.webgaragesystem.com as an origin — never AllowAnyOrigin() or a wildcard, even in dev.
- DTOs only cross the API boundary — never return EF/Dapper entities directly.
- Async all the way down in the API layer; avoid async void except event handlers.
- Use Task<ActionResult<T>> for all endpoints.
- Dapper calls use parameterized queries only (@paramName) — never string-interpolated SQL, even for "simple" filters.