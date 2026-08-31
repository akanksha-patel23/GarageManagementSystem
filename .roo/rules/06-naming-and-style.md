## Naming & style
- CSS: custom classes prefixed msc- (e.g. msc-dashboard, msc-jobcard-wizard).
- Consistent casing: PascalCase for public members in both C# and VB.NET — no language-specific casing exceptions. camelCase for JS/jQuery variables.
- .NET Core APIs default to camelCase JSON; jQuery consuming it should expect that.
- No magic strings for Status values (Pending/InProgress/Completed/Cancelled) — use constants or an enum-backed key shared across layers.