## Error handling
- No empty catch blocks. No On Error Resume Next without an explicit reason comment.
- DAL/BAL: catch narrow, log, rethrow (don't swallow).
- Web API: exceptions surface as ProblemDetails (RFC 7807) via builder.Services.AddProblemDetails() + UseExceptionHandler middleware in Program.cs — consistent shape across all endpoints. No raw 500 stack traces returned to the client.
- All stored procedures log failures via GLOG.spLogDBErrorDetail and return @ErrorID OUTPUT; DAL code must check @ErrorID <> 0 after every call and throw an exception that includes it (e.g. "Database operation failed. Reference ID: {ErrorID}") rather than swallowing it.
- Several Get stored procedures (see db_script.sql) return @ErrorID OUTPUT alongside a SELECT result set — in ADO.NET, output params only populate after the SqlDataReader is closed. Read the reader fully and close it before reading @ErrorID.