## Environment specifics
- Both API and Web projects are hosted on IIS.
- API runs on: http://www.apigaragesystem.com (NEVER localhost).
- Web runs on: http://www.webgaragesystem.com (NEVER localhost).
- NEVER generate localhost:xxxx URLs in any config, launchSettings, or appsettings.
- API base URL must come from Web.config/appSettings (Web Forms side) and appsettings.json (API side) — never hardcoded into .js files.
- Local dev resolves www.webgaragesystem.com / www.apigaragesystem.com via hosts file entries + IIS site bindings — do not assume DNS; confirm bindings exist before assuming a URL "just works."