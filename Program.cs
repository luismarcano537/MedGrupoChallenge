using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MedgrupoChallenge.Infraesctructure.Data;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var dbServer = GetRequiredEnvironmentVariable("DB_SERVER");
var dbPort = GetRequiredEnvironmentVariable("DB_PORT");
var dbName = GetRequiredEnvironmentVariable("DB_NAME");
var dbUser = GetRequiredEnvironmentVariable("DB_USER");
var dbPassword = GetRequiredEnvironmentVariable("DB_PASSWORD");
var dbTrustCertificate = Environment.GetEnvironmentVariable("DB_TRUST_CERTIFICATE") ?? "True";

static string GetRequiredEnvironmentVariable(string name)
{
    var value = Environment.GetEnvironmentVariable(name);

    if (string.IsNullOrWhiteSpace(value))
        throw new InvalidOperationException($"Environment variable '{name}' was not configured.");

    return value;
}

var connectionString =
    $"Server={dbServer},{dbPort};" +
    $"Database={dbName};" +
    $"User Id={dbUser};" +
    $"Password={dbPassword};" +
    $"TrustServerCertificate={dbTrustCertificate};";

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(connectionString); });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

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

app.Run();