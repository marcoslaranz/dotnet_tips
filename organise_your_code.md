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


![image](https://github.com/user-attachments/assets/cb6b3835-4472-47bc-bd23-08fc4a990d15)


### Then your Program.cs will be something like this:


![image](https://github.com/user-attachments/assets/6a2b686b-6ad6-4488-8ee3-1abb24a2835f)


## Modify your endpoint class.
The idea is to create a class extension for WebApplication.
As a class extension, we need to define it as static.
The extension method needs to be declared as receiving a parameter preceded by the word ‘this’.
Your REST/API methods, (GET, POST, PUT, DELETE,  ..), will all be declared here; however, their functionalities will be defined in services.
Starting to declare the namespace. The namespace is a path where your class is declared. This will be used for other codes to find where your code is, for example:

	  D:\dotnet_tips\OrganiseWebApi\EndPoints>

In the example above, the folder OrganiseWebApi is where your project was created.
The folder EndPoints is where you will create your class EndPoint. in this case, the namespace that you will need to create is:
	OrganiseWebApi\EndPoints in C# dotnet will be OrganiseWebApi.EndPoints, then create your 
namespace OrganiseWebApi.EndPoints;

Your WeatherforecastEndPoint.cs needs to be something like this:

// This is necessary as we need to call the method GetWeather() that is inside a class 
// declared in the code inside this folder:
// OrganiseWebApi\Services\WeatherforecastService.cs
using OrganiseWebApi.Services; 

![image](https://github.com/user-attachments/assets/a190ce27-deee-47bc-8fd1-4a037bb6aa80)



## Modify your service class.

![image](https://github.com/user-attachments/assets/050814b5-8628-479e-a18a-587f47ddbb45)

 
## Modify your Program.cs.
Add the call to the new extension method that was created.
 
![image](https://github.com/user-attachments/assets/6e8a04d6-a553-444f-9aac-bdf9598d169e)

## Compile and run.

![image](https://github.com/user-attachments/assets/9221228c-4690-4742-ae78-942fa430c8e3)


  
	
## Test your web API:
Use this URL in the web browser:

localhost:5003/weatherforecast

Just so you know, the port may vary.

 ![image](https://github.com/user-attachments/assets/3c167303-410a-4d90-84de-080f5270a3ee)

 ---



