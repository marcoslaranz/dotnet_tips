using OrganiseWebApi.Services;
using OrganiseWebApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


//This allows the WeatherService to be injected
//into any class/method that needs it.
builder.Services.AddScoped<WeatherService>();
// As Scoped this class will be instantiated 
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

