using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using JwtWeather.Services;
using JwtWeather.Dtos;

namespace JwtWeather.EndPoints;

public static class JwtWeatherEndPoint
{
    public static WebApplication MapGetWeather(this WebApplication app)
    {
        app.MapGet("/weatherforecast", (WeatherService weather) =>
        {
            return weather.Getweatherforecast();
        })
        .RequireAuthorization() // This ensures authentication is required for thi;
        .WithName("GetWeatherForecast");


        app.MapPost("/weatherforecast", (LoginInfo loginInfo, JwtService jwt) =>
        {
           if(loginInfo.LoginName == "Admin" && loginInfo.Password == "Admin123")
           {
              var token = jwt.GenerateToken(loginInfo.LoginName);
              return Results.Ok(new {Token = token });
           }
           return Results.Unauthorized();

        });

	return app;
    }
}