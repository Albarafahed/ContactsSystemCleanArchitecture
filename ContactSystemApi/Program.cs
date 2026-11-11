using ContactsSystemApi.Middleware;
using ContactsSystemApi.Services;
using ContactsSystemApplication.Interfaces;
using ContactsSystemInfrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Read JWT settings from configuration
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings");

// Validate that the SecretKey is configured
var secretKey = jwtSettings["SecretKey"];

if (string.IsNullOrWhiteSpace(secretKey))
{
    throw new InvalidOperationException(
        "JWT SecretKey is not configured.");
}
// ===============================
// 1) Authentication (JWT Bearer)
// ===============================
//
// This tells ASP.NET Core that the API will use JWT Bearer tokens.
// After this is configured, the middleware can validate tokens sent in:
// Authorization: Bearer <token>

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),

            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define the "Bearer" security scheme.
    // Swagger will use it to display an input box for Authorization header.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // The header name where the JWT should be placed.
        Name = "Authorization",

        // HTTP authentication scheme (Authorization header).
        Type = SecuritySchemeType.Http,

        // The scheme name must be "Bearer" for JWT Bearer tokens.
        Scheme = "Bearer",

        // Optional, but helps documentation and UI.
        BearerFormat = "JWT",

        // Token is sent in the request header.
        In = ParameterLocation.Header,

        // Instruction shown in Swagger UI.
        Description = "Enter: Bearer {your JWT token}"
    });

    // Apply the Bearer scheme globally so secured endpoints in Swagger
    // can automatically include the Authorization header after you authorize.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                // Reference the security scheme defined above by its Id: "Bearer".
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },

            // No specific scopes are required for JWT Bearer in this setup.
            new string[] {}
        }
    });
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<
    ICurrentUserService,
    CurrentUserService>();
builder.Services.AddInfrastructure(builder.Configuration);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("SecureCors", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:3000",
                "https://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors("SecureCors");

// Global Exception Handling Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
