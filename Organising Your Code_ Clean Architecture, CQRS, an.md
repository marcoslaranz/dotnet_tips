## Organising Your Code: Clean Architecture, CQRS, and Minimal WebAPI in .NET

This document provides practical guidance for structuring .NET applications with a focus on clean architecture, CQRS, event sourcing, and organizing minimal WebAPI projects for scalability and maintainability.

#### Disclaimer: Part of this document was created with AI help.

---

### **Business and Application Logic**

To ensure a clean architecture and future scalability, business logic should reside in **domain entities**, **domain services**, and **application services**—not in controllers or infrastructure layers.

#### **Best Places for Business Logic**

- **Domain Entities (Core Business Rules)**
    - Encapsulate logic related to a specific entity.
    - *Example:*

```csharp
public class Customer
{
    public int Age { get; set; }

    public bool IsEligibleForDiscount()
    {
        return Age > 60; // Business logic within the entity
    }
}
```

- **Domain Services (Business Operations)**
    - Manage logic involving multiple entities.
    - *Example:*

```csharp
public class MembershipService
{
    public decimal CalculateFee(Customer customer, Plan plan)
    {
        return plan.BasePrice - (customer.IsEligibleForDiscount() ? 10 : 0);
    }
}
```

- **Application Services (Use Cases)**
    - Handle high-level workflows (e.g., placing orders).
    - *Example:*

```csharp
public class OrderAppService
{
    private readonly IOrderRepository _orderRepo;
    private readonly MembershipService _membershipService;

    public OrderAppService(IOrderRepository orderRepo, MembershipService membershipService)
    {
        _orderRepo = orderRepo;
        _membershipService = membershipService;
    }

    public void PlaceOrder(Customer customer, Plan plan)
    {
        var fee = _membershipService.CalculateFee(customer, plan);
        _orderRepo.Save(new Order { Customer = customer, TotalPrice = fee });
    }
}
```


#### **Example Directory Organization**

```
📦 YourProject
 ├── Application
 │    └── Services
 │         ├── OrderAppService.cs
 │         └── MembershipAppService.cs
 ├── Domain
 │    ├── Entities
 │    │    ├── Customer.cs
 │    │    ├── Order.cs
 │    │    └── Plan.cs
 │    └── Services
 │         ├── MembershipService.cs
 │         └── DiscountService.cs
 ├── Infrastructure
 │    ├── Persistence
 │    │    └── Repositories
 │    │         ├── OrderRepository.cs
 │    │         └── CustomerRepository.cs
 │    └── Database
 │         └── GymDbContext.cs
 ├── API
 │    └── Controllers
 │         ├── OrderController.cs
 │         └── CustomerController.cs
 └── Tests
      └── UnitTests
           ├── OrderServiceTests.cs
           └── MembershipServiceTests.cs
```


#### **Why Avoid Business Logic in Controllers or Infrastructure?**

- **Controllers** should only handle HTTP requests and responses.
- **Infrastructure** (e.g., repositories) should only persist data.
- This separation keeps systems **extensible** and **testable**.

---

### **CQRS (Command Query Responsibility Segregation)**

CQRS is an architectural pattern that separates read and write operations into distinct models, optimizing each for its task.

#### **Why Use CQRS?**

- **Separation of Concerns:** Commands (writes) and queries (reads) are handled independently.
- **Performance:** Read models can be optimized for fast queries; write models focus on data integrity.
- **Scalability:** Scale reads and writes independently.
- **Reduced Complexity:** Enables better domain-driven design and event sourcing.


#### **How CQRS Works**

- **Commands:** Actions that change system state (e.g., `PlaceOrderCommand`).
- **Command Handlers:** Process commands and trigger events.
- **Queries:** Retrieve data without modifying it.
- **Event Sourcing (Optional):** Store changes as events for auditability.


#### **CQRS Example: E-commerce Order Processing**

| Aspect | Command Side (Write) | Query Side (Read) |
| :-- | :-- | :-- |
| Action | Customer places order (`PlaceOrderCommand`) | Customer checks order status |
| Processing | Validate stock, store order, trigger events | Retrieve order from read database |
| Data Model | Write-optimized (consistency, integrity) | Read-optimized (fast access) |

**Benefits:**

- Reads don’t block writes
- Read model can use caching/NoSQL
- Write operations are strictly controlled

---

### **Transactional Domain**

A transactional domain ensures operations are executed with atomicity, consistency, isolation, and durability (ACID).

- **Atomicity:** All-or-nothing execution.
- **Consistency:** Valid states across entities.
- **Isolation:** No interference between transactions.
- **Durability:** Changes persist after commit.

*Example:*
In an e-commerce system, an `Order` aggregate ensures inventory is updated, payment is processed, and confirmation is sent—all within a single transaction.

---

### **Event Sourcing in a Transactional Domain**

Event sourcing stores state changes as a sequence of immutable events.

- **Events as Source of Truth:** Every change is an event.
- **Rebuild State:** Replay events to reconstruct state.
- **Auditability:** Full history of changes.

**Integration with CQRS:**

- Commands generate events
- Events stored in an append-only log
- Read model updated asynchronously

---

### **CQRS \& Event Sourcing Example in .NET**

#### **Command Model (Write Side)**

```csharp
public class PlaceOrderCommand {
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItem> Items { get; set; }
}

public class OrderItem {
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```

**Command Handler:**

```csharp
public class PlaceOrderHandler : ICommandHandler<PlaceOrderCommand> {
    private readonly IEventStore _eventStore;

    public PlaceOrderHandler(IEventStore eventStore) {
        _eventStore = eventStore;
    }

    public async Task Handle(PlaceOrderCommand command) {
        var orderPlacedEvent = new OrderPlacedEvent {
            OrderId = command.OrderId,
            CustomerId = command.CustomerId,
            Items = command.Items
        };
        await _eventStore.SaveEventAsync(orderPlacedEvent);
    }
}
```


#### **Query Model (Read Side)**

```csharp
public class GetOrderDetailsQuery {
    public Guid OrderId { get; set; }
}
public class OrderDetailsDto {
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
public class OrderItemDto {
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```

**Query Handler:**

```csharp
public class GetOrderDetailsHandler : IQueryHandler<GetOrderDetailsQuery, OrderDetailsDto> {
    private readonly IReadOnlyRepository<Order> _orderReadRepository;

    public GetOrderDetailsHandler(IReadOnlyRepository<Order> orderReadRepository) {
        _orderReadRepository = orderReadRepository;
    }

    public async Task<OrderDetailsDto> Handle(GetOrderDetailsQuery query) {
        var order = await _orderReadRepository.GetByIdAsync(query.OrderId);
        return new OrderDetailsDto {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            Items = order.Items.Select(i => new OrderItemDto {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }
}
```

**Event Store Interface:**

```csharp
public interface IEventStore {
    Task SaveEventAsync<T>(T @event) where T : class;
    Task<IEnumerable<object>> GetEventsAsync(Guid aggregateId);
}
```


---

### **Minimal WebAPI Organization in .NET**

When starting with a minimal WebAPI, all code is in `Program.cs`. For maintainability, refactor into folders:

#### **Project Setup**

```bash
dotnet new webapi -n OrganiseWebApi
cd OrganiseWebApi
mkdir EndPoints
copy Program.cs EndPoints/WeatherforecastEndPoint.cs
mkdir Services
copy Program.cs Services/WeatherforecastService.cs
```


#### **Endpoint Class (Extension Method Example)**

```csharp
using OrganiseWebApi.Services;

namespace OrganiseWebApi.EndPoints;

public static class WeatherforecastEndPoint
{
    public static WebApplication MapWeatherEndPoints(this WebApplication app)
    {
        app.MapGet("/weatherforecast", (WeatherService weather) =>
        {
            return weather.GetWeather();
        })
        .WithName("GetWeatherForecast");

        return app;
    }
}
```


#### **Service Class Example**

```csharp
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


#### **Program.cs Example**

```csharp
using OrganiseWebApi.Services;
using OrganiseWebApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapWeatherEndPoints();
app.Run();
```


#### **Testing and OpenAPI**

- Test your API:
`http://localhost:5003/weatherforecast`
- View OpenAPI spec:
`http://localhost:5003/openapi/v1.json`

---

### **Further Resources**

- CQRS and Event Sourcing in .NET Core 8 – Detailed guides and real-world examples.
- Event Sourcing in .NET – Tutorials and sample implementations.
- Domain Events in .NET – Microsoft documentation.

---


<div style="text-align: center">⁂</div>

[^1]: Organising-your-code.docx

