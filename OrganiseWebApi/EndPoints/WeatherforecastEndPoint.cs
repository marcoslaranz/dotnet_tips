using OrganiseWebApi.Services;

namespace OrganiseWebApi.EndPoints;

public static class WeatherEndPoint
{
        public static WebApplication MapWeatherEndPoints(this WebApplication app)
        {
			    // The WeatherService is injected here
                app.MapGet("/weatherforecast", (WeatherService wather) =>
                {
                    return wather.GetWeather();
                })
                .WithName("GetWeatherForecast");

                return app;
        }
}
