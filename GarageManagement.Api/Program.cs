using GarageManagement.Api.Repositories;
using GarageManagement.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Per 05-api-conventions.md: DTOs only cross the boundary; controllers stay thin.
builder.Services.AddControllers();

// ProblemDetails(RFC 7807) — consistent error shape across every endpoint.
// Per 03-error-handling.md: no raw 500 stack traces returned to the client.
builder.Services.AddProblemDetails();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// CORS: allow only the Web Forms origin — never AllowAnyOrigin(), even in dev.
// Per 05-api-conventions.md / 08-environment.md.
// Add CORS policy for webgaragesystem.com only

const string WebOriginPolicy = "WebOriginPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(WebOriginPolicy, policy =>
    {
        policy.WithOrigins("http://www.webgaragesystem.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// JWT authentication — issued by AuthController.Login, consumed by every other
// endpoint. FallbackPolicy below requires an authenticated user by default on
// every action; AuthController.Login is the sole [AllowAnonymous] exception.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --- Dapper repositories and services -------------------------------------
// Uncomment each pair as that feature area's Repository/Service classes are
// actually built (per full-solution-file-scaffold.md). 
//
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
//
builder.Services.AddScoped<IServiceCatalogRepository, ServiceCatalogRepository>();
builder.Services.AddScoped<IServiceCatalogService, ServiceCatalogService>();
//
builder.Services.AddScoped<IJobCardRepository, JobCardRepository>();
builder.Services.AddScoped<IJobCardService, JobCardService>();
//
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// In Development, show the full exception (stack trace, message) so errors
// like a bad connection string or a missing stored procedure are actually
// visible instead of a generic ProblemDetails response. Production keeps the
// clean ProblemDetails-only behavior per 03-error-handling.md.

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseStatusCodePages();
}

// Use exception handler middleware for ProblemDetails
//app.UseExceptionHandler();
//app.UseStatusCodePages();

app.UseHttpsRedirection();
app.UseCors(WebOriginPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();