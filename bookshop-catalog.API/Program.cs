using bookshop_catalog.Extensions;
using bookshop_catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Setup EF Core
builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<BookContext>(options =>
    {
        // Get password from environment to avoid hard coding plain text credentials
        var sqlPassword = Environment.GetEnvironmentVariable("SQL_PASSWORD");
        var connectionString = string.Format(configuration.GetConnectionString("DefaultConnection"), sqlPassword);

        options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Apply EF Migrations and seed DB if required
app.SetupDatabaseMigrations<BookContext>(context =>
{
    new BookCatalogDbSeeder().SeedAsync(context).Wait();
});

app.Run();
