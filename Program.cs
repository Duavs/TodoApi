using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Config;
using TodoApi.Data;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Bind JWT settings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
var tokenExpiration = TimeSpan.FromMinutes(jwtSettings.ExpirationInMinutes);           
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
            ValidAudience = jwtSettings.Audience
        };
    });
// ✅ Configure Database Context
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// ✅ Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200") 
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // 🛠️ This registers IHttpClientFactory
// builder.Services.Configure<OpenAiSettings>(builder.Configuration.GetSection("OpenAI"));
// builder.Services.AddScoped<IOpenAiService, OpenAiService>();
builder.Services.AddHttpClient<IAdviceService, AdviceService>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ✅ Use CORS before Authentication & Authorization
app.UseCors("AllowAngular");

app.UseAuthentication();  // 🔹 Ensure authentication middleware is used
app.UseAuthorization();  

app.MapControllers();

app.Run();