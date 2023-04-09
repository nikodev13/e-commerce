# e-commerce

### School e-commerce system project

### Available features:
- Users (authentication and authorization by JWT):
  - Register user
  - Login user
  - Refresh tokens(access and refresh)
  - Change user email(used to log in)
- Customers:
  - Register customer (allowed for registered user)
  - managing address book (max. 3 addresses)
  - managing wishlist (max. 10 products)
- Products:
  - CRUD
  - setting sale data (price, quantity, is active)
- CRUD for categories
- Orders:
  - Placing order (by registered and logged in customers)
  - setting order status (for admin)
  - changing order line product quantity (for admin)
  - changing delivery address (for admin)
  - setting delivery tracking number (for admin)

### Tech stack
- [C# 11](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11), [.NET 7](https://dotnet.microsoft.com/en-us/), [ASP.NET Core](https://learn.microsoft.com/pl-pl/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-7.0) - The language and frameworks that we love are becoming faster and more modern.
- [CQRS](https://cezarypiatek.github.io/post/why-i-dont-use-mediatr-for-cqrs/) - We re-implemented this known pattern to improve performance and gain a better understanding of how it works.
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/) for commands and queries validation and is widely used and popular validation framework.
- [Entity Framework Core 7](https://learn.microsoft.com/en-us/ef/core/)
  - Object Relational Mapping tool that simplifies querying database.
  - Used with [Microsoft SQL Sever 2022](https://www.microsoft.com/pl-pl/sql-server/).
- [bcrypt.net](https://github.com/BcryptNet/bcrypt.net) for hashing user password
- [Serilog](https://serilog.net/) for logging mechanism

### Onion Architecture
- ApplicationCore
  - Place for data-centric entities
  - Implementation of commands and queries as use cases
  - Application services interfaces, i.a. UserContextProvider 
- Infrastructure
  - Implements application services
  - Provides logging, database access
- API
  - Provides HTTP endpoints
  - Interacts with application layer
  - Implements application services

#### Moreover
- The idea of using repository pattern with EF in most cases is just bad so we use `DbSet<TEntity>` as repositories in interface `IAppDbContext` in application layers which is implemented in infrastructure layer using provider for SQL Server.
- We do not use result types or similar to handle success or failure in commands or queries. C# is a language where errors are handled using exception mechanism so we use it to manage failures using `ExceptionMiddleware` in API layer.
- We do not use mapper libraries (e.g. AutoMapper) because that may hurt performance. We use `record` with static methods to map entities to read models or request to commands.  
- For authentication and authorization we use JWT tokens. Permissions are not stored in token for better security and always checked by middleware. 
- We do not use MVC Controllers for mapping endpoints because we believe that minimal apis are more performant and better for maintaining.
  
### Swagger (API endpoints):
![swagger-1.png](docs%2Fswagger-1.png)
![swagger-2.png](docs%2Fswagger-2.png)
![swagger-3.png](docs%2Fswagger-3.png)