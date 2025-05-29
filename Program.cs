using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Config;
using TodoApi.Data;
using TodoApi.Services;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
// ✅ Load configuration from appsettings.json
// builder.Configuration.AddEnvironmentVariables();
// ✅ Load JwtSettings from environment variables
DotNetEnv.Env.Load();
var jwtSettings = new JwtSettings
{
    Key = Environment.GetEnvironmentVariable("Jwt__Key") ?? throw new InvalidOperationException("Jwt__Key is missing."),
    Issuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? throw new InvalidOperationException("Jwt__Issuer is missing."),
    Audience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? throw new InvalidOperationException("Jwt__Audience is missing."),
    ExpirationInMinutes = int.Parse(Environment.GetEnvironmentVariable("Jwt__ExpirationInMinutes") ?? throw new InvalidOperationException("Jwt__ExpirationInMinutes is missing."))
};
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
var tokenExpiration = TimeSpan.FromMinutes(jwtSettings.ExpirationInMinutes);
//configure rate limiting
builder.Services.AddRateLimiter(options =>
{   
    options.AddPolicy("loginLimiter", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5, // 3 requests
                Window = TimeSpan.FromMinutes(1), // in 2 minutes
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 1
            }));
    options.AddPolicy("signupLimiter", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(2),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});
// ✅ Configure JWT Authentication globally
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine("JWT token was missing or invalid (401 Unauthorized)");
                return Task.CompletedTask;
            }
        };
    });
// ✅ Configure Database Context
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// ✅ Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200", "https://taskmanagerapi-john-0528.azurewebsites.net")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // 🛠️ This registers IHttpClientFactory
builder.Services.AddHttpClient<IAdviceService, AdviceService>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ✅ Use CORS before Authentication & Authorization
app.UseCors("AllowAngular");

app.UseAuthentication(); // 🔹 Ensure authentication middleware is used
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();