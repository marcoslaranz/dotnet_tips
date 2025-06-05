using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

using JwtWeather.Services;
using JwtWeather.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
IConfiguration configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<WeatherService>();

builder.Services.AddSingleton<JwtService>();

// Register JwtService with configuration
//builder.Services.AddSingleton<JwtService>(sp => new JwtService(configuration));




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Config Jwt token
builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options => {

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? 
                          throw new ArgumentNullException("JWT Secret Key is missing in appsettings.json")))
                };
        });

builder.Services.AddAuthorization();




var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGetWeather();

app.Run();