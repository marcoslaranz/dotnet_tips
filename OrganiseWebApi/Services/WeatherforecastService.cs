namespace OrganiseWebApi.Services;

static public class WeatherService
{
        public static readonly string[] summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        static public IEnumerable<WeatherForecast> GetWeather()
        {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                         (
                             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                             Random.Shared.Next(-20, 55),
                             summaries[Random.Shared.Next(summaries.Length)]
                         ))
                        .ToArray();
        }

        public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
        {
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }
}