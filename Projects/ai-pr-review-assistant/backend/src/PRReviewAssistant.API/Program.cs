using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Endpoints;
using PRReviewAssistant.API.Infrastructure.Data;
using PRReviewAssistant.API.Infrastructure.Repositories;
using PRReviewAssistant.API.Infrastructure.Services;
using PRReviewAssistant.API.Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ── Exception Handling ───────────────────────────────────────────────────────
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ── EF Core + PostgreSQL ─────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── MediatR ──────────────────────────────────────────────────────────────────
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// ── FluentValidation ─────────────────────────────────────────────────────────
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// ── Repositories ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

// ── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAiAnalysisService, MockAiAnalysisService>();

// ── CORS ─────────────────────────────────────────────────────────────────────
const string CorsPolicy = "FrontendPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration.GetValue<string>("Cors:AllowedOrigin")
                    ?? "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ── Swagger / OpenAPI ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title       = "PR Review Assistant API",
        Version     = "v1",
        Description = "AI-powered code review analysis service"
    });
});

var app = builder.Build();

// ── Auto-migrate in Development ──────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PR Review Assistant v1"));
}

app.UseCors(CorsPolicy);

// ── Endpoints ─────────────────────────────────────────────────────────────────
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
   .WithName("HealthCheck")
   .WithTags("Health")
   .ExcludeFromDescription();

app.MapReviewEndpoints();
app.MapSettingsEndpoints();

app.Run();

