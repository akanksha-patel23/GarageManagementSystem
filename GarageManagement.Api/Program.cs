using GarageManagement.Api.Repositories;
using GarageManagement.Api.Services;
using System.Runtime.ConstrainedExecution;

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

// Configure the HTTP request pipeline.
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

//app.UseHttpsRedirection();
app.UseCors(WebOriginPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
