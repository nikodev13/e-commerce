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
- The Entity Framework Core provides application database in the application layer and replaces domain unit of work and domain repositories (probably we never change database provider).
- The application layer uses Results type as result of command and queries. They are handled (successes and failures) in API layer. 

### Available features:
- CRUD for Product Category
- Authentication:
  - Register user
  - Login user
  
