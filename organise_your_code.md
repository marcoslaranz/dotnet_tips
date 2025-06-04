# Organising Your Code

## Overview
When you create a minimalist .NET Web API, all your code is in `Program.cs`. If you intend to add more functionalities or use other REST methods besides `GET`, it would be a good idea to refactor your code by adding folders and moving parts of `Program.cs` into these new locations.

## Creating Your Project
To create a new project, use:
```sh
dotnet new webapi -n OrganiseWebApi
```

## Setting Up Endpoints
Navigate to your project folder:
```sh
cd OrganiseWebApi
mkdir EndPoints
cp Program.cs EndPoints/WeatherforecastEndPoint.cs
```

## Creating Services
```sh
mkdir Services
cp Program.cs Services/WeatherforecastService.cs
```

## Modifying `Program.cs`
Remove the implementations from Program.cs for use in the Services and the Endpoints..


![image](https://github.com/user-attachments/assets/977e40e2-fc3b-445b-9f42-5a778653848e)



### Then your Program.cs will be something like this:


![image](https://github.com/user-attachments/assets/6a2b686b-6ad6-4488-8ee3-1abb24a2835f)


## Modify your endpoint class.
The idea is to create a class extension for WebApplication.
As a class extension, we need to define it as static.
The extension method needs to be declared as receiving a parameter preceded by the word ‘this’.
Your REST/API methods (GET, POST, PUT, DELETE, ..) will all be declared here; however, their functionalities will be defined in services.
Starting to declare the namespace. The namespace is a path where your class is declared. This will be used for other codes to find where your code is, for example:

	  D:\dotnet_tips\OrganiseWebApi\EndPoints>

In the example above, the folder OrganiseWebApi is where your project was created.
The folder EndPoints is where you will create your class Endpoint. In this case, the namespace that you will need to create is:
	OrganiseWebApi\EndPoints in C# dotnet will be OrganiseWebApi.EndPoints, then create your 
namespace OrganiseWebApi.EndPoints;

Your WeatherforecastEndPoint.cs needs to be something like this:

```sh
// The GET method will receive a complete object WeatherforecastService. 
// The dotnet Component container DI will be called in the
// Program.cs to register the class WeatherforecastService.
// Every time that this class is requested, the DI will give a complete class to
// the requested method, so this can be used as a parameter of a method.
```

```sh
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
```




## Modify your service class.

```sh
namespace OrganiseWebApi.Services;

public class WeatherService
{
        public static readonly string[] summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> GetWeather()
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
```

 
## Modify your Program.cs.
Add the call to the new extension method that was created.

```sh
using OrganiseWebApi.Services;
using OrganiseWebApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


//This allows the WeatherService to be injected
//into any class/method that needs it.
builder.Services.AddScoped<WeatherService>();
// As Scoped, this class will be instantiated 
// for every request and will last 
// till the processing the whole request

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); //This will allows us to see the OpenApi specification document
	                  // using this link: http://localhost:5003/openapi/v1.json
}

app.UseHttpsRedirection();

//This will run the Extension
//class/method.
app.MapWeatherEndPoints();

app.Run();

```


## Compile and run.

![image](https://github.com/user-attachments/assets/9221228c-4690-4742-ae78-942fa430c8e3)


  
	
## Test your web API:
Use this URL in the web browser:

localhost:5003/weatherforecast

Just so you know, the port may vary.

 ![image](https://github.com/user-attachments/assets/3c167303-410a-4d90-84de-080f5270a3ee)


![image](https://github.com/user-attachments/assets/f9c43f67-097d-4ec5-84da-cfa537713979)


 ---



