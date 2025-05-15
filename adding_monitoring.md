# Adding monitoring to your WebApi with Graphana

## Create a minimal webapi:

	dotnet new webapi -n Monitoring
	cd Monitoring
 
## Install the following NuGet packages into your project.
   Top-level Package                           	  Requested   	  Resolved
   
 ## Install the following NuGet packages into your project

| Package                                      | Requested  | Resolved   |
|----------------------------------------------|------------|------------|
| Microsoft.AspNetCore.OpenApi                 | 9.0.4      | 9.0.4      |
| OpenTelemetry                                | 1.12.0     | 1.12.0     |
| OpenTelemetry.Exporter.Console               | 1.12.0     | 1.12.0     |
| OpenTelemetry.Exporter.Prometheus.AspNetCore | 1.12.0-beta.1 | 1.12.0-beta.1 |
| OpenTelemetry.Extensions.Hosting             | 1.12.0     | 1.12.0     |
| OpenTelemetry.Instrumentation.AspNetCore     | 1.12.0     | 1.12.0     |
| OpenTelemetry.Instrumentation.Http           | 1.12.0     | 1.12.0     |
| Serilog                                      | 4.2.0      | 4.2.0      |
| Serilog.AspNetCore                           | 9.0.0      | 9.0.0      |
| Serilog.Extensions.Hosting                   | 9.0.0      | 9.0.0      |
| Serilog.Sinks.Console                        | 6.0.0      | 6.0.0      |
| Serilog.Sinks.Loki                           | 3.0.0      | 3.0.0      |
   

### Run these instructions in the command line:

```csharp
dotnet add package Microsoft.AspNetCore.OpenApi --version 9.0.4
dotnet add package OpenTelemetry --version 1.12.0
dotnet add package OpenTelemetry.Exporter.Console --version 1.12.0
dotnet add package OpenTelemetry.Exporter.Prometheus.AspNetCore --version 1.12.0-beta.1
dotnet add package OpenTelemetry.Extensions.Hosting --version 1.12.0
dotnet add package OpenTelemetry.Instrumentation.AspNetCore --version 1.12.0
dotnet add package OpenTelemetry.Instrumentation.Http --version 1.12.0
dotnet add package Serilog --version 4.2.0
dotnet add package Serilog.AspNetCore --version 9.0.0
dotnet add package Serilog.Extensions.Hosting --version 9.0.0
dotnet add package Serilog.Sinks.Console --version 6.0.0
dotnet add package Serilog.Sinks.Loki --version 3.0.0
```

### Check if all packages were installed successfully:
```csharp
dotnet list package
Get more information about the packages:
dotnet list package --include-transitive
```

### Check if you have some outdated packages:
```csharp
dotnet list package-- outdated
```

## Modify your Program.cs

### Add these lines:
```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;
```

After his line: builder.Services.AddOpenApi(); added the block of code below:

```csharp
// Configure OpenTelemetry for logging and tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddPrometheusExporter());


// Configure Serilog for Loki logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
```
 

 

## Compile and test.
Run your Grafana sever:
  Please, look at document xxx to get instructions in how to run the grafana processes
  
Check if your servers are up and running.

  
```csharp
vagrant@ubuntu-jammy:~/Projects/Monitoring$ docker ps
CONTAINER ID   IMAGE             COMMAND                  CREATED      STATUS          PORTS                                       NAMES
992c4ad44d74   grafana/loki      "/usr/bin/loki -conf…"   4 days ago   Up 35 minutes   0.0.0.0:3100->3100/tcp, :::3100->3100/tcp   loki
d11f8627b4da   prom/prometheus   "/bin/prometheus --c…"   4 days ago   Up 35 minutes   0.0.0.0:9090->9090/tcp, :::9090->9090/tcp   prometheus
4163c40fc290   grafana/grafana   "/run.sh"                4 days ago   Up 35 minutes   0.0.0.0:3000->3000/tcp, :::3000->3000/tcp   grafana
4f789bf2403f   postgres          "docker-entrypoint.s…"   6 days ago   Up 35 minutes   0.0.0.0:5432->5432/tcp, :::5432->5432/tcp   postgres-container
```


 
If not, refer to the document load_grafana.md to learn how to run these containers.
Run your application 

```csharp
vagrant@ubuntu-jammy:~/Projects/Monitoring$ dotnet run
Using launch settings from /home/vagrant/Projects/Monitoring/Properties/launchSettings.json...
Building...
[02:29:20 INF] Now listening on: http://0.0.0.0:5140
[02:29:20 INF] Application started. Press Ctrl+C to shut down.
[02:29:20 INF] Hosting environment: Development
[02:29:20 INF] Content root path: /home/vagrant/Projects/Monitoring
```

### Test your webapi

```csharp
http://192.168.56.17:5140/weatherforecast
```

![image](https://github.com/user-attachments/assets/4ed187e4-5564-49ac-a705-e9de6ac38930)



### Check your console log:

```csharp
[02:30:52 INF] Request starting HTTP/1.1 GET http://192.168.56.17:5140/weatherforecast - null null
[02:30:52 WRN] Failed to determine the https port for redirect.
[02:30:52 INF] Executing endpoint 'HTTP: GET /weatherforecast'
[02:30:52 INF] Executed endpoint 'HTTP: GET /weatherforecast'
[02:30:52 INF] Request finished HTTP/1.1 GET http://192.168.56.17:5140/weatherforecast - 200 null application/json; charset=utf-8 242.8334ms
Activity.TraceId:            cb1a8c7c3cd008e4be773510720c7429
Activity.SpanId:             d9c7cbbefcd40ba3
Activity.TraceFlags:         Recorded
Activity.DisplayName:        GET /weatherforecast
Activity.Kind:               Server
Activity.StartTime:          2025-05-15T02:30:52.1594317Z
Activity.Duration:           00:00:00.2846691
Activity.Tags:
    server.address: 192.168.56.17
    server.port: 5140
    http.request.method: GET
    url.scheme: http
    url.path: /weatherforecast
    network.protocol.version: 1.1
    user_agent.original: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36 Edg/136.0.0.0
    http.route: /weatherforecast
    http.response.status_code: 200
Instrumentation scope (ActivitySource):
    Name: Microsoft.AspNetCore
Resource associated with Activity:
    telemetry.sdk.name: opentelemetry
    telemetry.sdk.language: dotnet
    telemetry.sdk.version: 1.12.0
    service.name: unknown_service:Monitoring
```
 

 

## Access Grafana.
At first use: 

  - Username: Admin 
  - Password: Admin
  
- Please, to find out how to configure and create dashboards, go to Grafana: The open and composable observability platform | Grafana Labs
Or following the guides in the application

 
![image](https://github.com/user-attachments/assets/0df21062-b4bc-4025-bc64-044e7ebf683b)





