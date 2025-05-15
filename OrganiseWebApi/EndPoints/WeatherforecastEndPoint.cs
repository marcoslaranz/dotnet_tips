using OrganiseWebApi.Services;

namespace OrganiseWebApi.EndPoints;

public static class WeatherEndPoint
{
        public static WebApplication MapWeatherEndPoints(this WebApplication app)
        {
                app.MapGet("/weatherforecast", () =>
                {
                    return WeatherService.GetWeather();
                })
                .WithName("GetWeatherForecast");

                return app;
        }
}