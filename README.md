# e-commerce

### School e-commerce system project 

### Tech stack
- ASP.NET Core 6.0 
- MediatR for CQRS
- FluentValidation
- Entity Framework Core 7 for SQL Server I/O

### Clean Architecture
- Domain 
  - Implementation based on basic DDD principles 
  - Business logic: entities (with behaviours), value objects, events
  - Domain services interfaces, i.a. unique id provider
- Application
  - Interaction logic with domain entities
  - Commands and queries as use cases
  - Implements domain services
  - Application services interfaces, i.a. e-mails senders
- Infrastructure
  - Implements application services
  - Provides logging, database access, e-mail communication
- API
  - Provides HTTP endpoints
  - Interacts with application layer
  - Implements application services

#### Moreover
The application layer throws exceptions as results of unexpected requests and handle them in exception handling middleware in API layer.


### Available features:
- CRUD for Product Category (without auth)
