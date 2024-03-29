# Template.Backend
This is a solution template for creating an ASP.NET Core Web API migrated from .NET Framework To .NET 7

## Description
This is a .NET project template that targets the .NET 7.0. It provides an advanced structure for building .NET applications that can be used for your own projects.

## Features
- [x] Target .NET 7.0
- [x] Audit Operations Support
- [x] Multi Layered Architecture
- [x] Test Projects
- [x] Logging Support
- [x] API endpoints documentation (Swagger)
- [x] .NET Client library
- [x] Docker support

<details>
  <summary>Click to See More!</summary>

- [x] Uses Entity Framework Core 7 as DB Abstraction
- [x] Cors Support
- [x] Integration Tests
- [x] Unit Tests
- [x] Repository Pattern
- [x] Nlog Integration
- [x] Automapper
- [x] API Versioning
- [x] Audit Logging
- [x] Server Validation
- [x] & Much More
</details>

## Overview

### Architecture
The project follows a layered architecture pattern, with separate layers for the API controllers, services, repositories, and data access code. This helps to promote separation of concerns and makes the code easier to maintain and test.

### ORM
The project uses Entity Framework Core 7 as its Object-Relational Mapping (ORM) framework, which allows developers to map C# objects to database tables and provides many features for managing data, such as querying, updating, and deleting.

### Database
The project uses local SQL Server as its database engine.

### API
The project includes a basic API with CRUD (Create, Read, Update, Delete) operations for a single entity, implemented in a controller.

### Dependency Injection
The project uses the Microsoft built in injection framework to manage the dependencies between the various components.

### Logging
The project includes logging using Nlog, which allows developers to log messages to various targets, such as console, file.
Use some features of Archiving log files

### Error handling
The project includes some basic error handling, with exceptions being caught and returned as appropriate HTTP error responses.

### API endpoints documentation 
The project includes basic API endpoints documentation using Swagger so that users can easily understand how to interact with APIs.

### .NET Client library
The project includes a C# Client that wrap HTTP requests, handle authentication, and parse responses into model objects, using a client library can save development time and effort.

## Usage

 1- Clone or download the repository: To get started, clone or download the repository to your local machine.
 
 2- Open the solution file in Visual Studio 2022: The solution file is located in the root directory of the project. Open this file in Visual Studio to start working with the project.
 
 3- Restore NuGet Packages
 
 4- Build and run the project
 
 5- Modify the project as needed for your own application: This template is designed to be a starting point for your application. Modify the class files, add new dependencies, or create new files as needed for your own application.
 
 ### Database Configuration

The template is configured to use a local database by default [(localdb)\MSSQLLocalDB]. This ensures that all users will be able to run the solution without needing to set up additional infrastructure over Visual studio, you need to use a SQL Server instance if you want to target other platforms than windows.

If you would like to use a custom intance of SQL Server, you will need to update **Template.Backend.Api/appsettings.json** or **Template.Backend.Api/appsettings.Developement.json** for development environment

Verify that the **ConnectionStrings** points to a valid SQL Server instance. 

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.
 
 ### Docker

 Make sure you have docker installed (on windows install docker desktop)

 In order to build and start the solution with Docker, you will need to point to a SQL Server instance instead of using LocalDB (supported only on windows). 

 ### Docker Compose

 There are some prerequisites for using the included docker-compose.yml files:

 You can find [Microsoft Docs](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-7.0) that describe the steps required for Windows, macOS, and Linux.

 #### Windows using Linux containers

 1- Create and install an https certificate:

```
    dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p yourStrongP@ssw0rd
```

 Make sure you have the same password ```yourStrongP@ssw0rd``` in the docker-compose.yml file ```ASPNETCORE_Kestrel__Certificates__Default__Password=yourStrongP@ssw0rd``` configuration

 2- It's possible that the above step gives you an `A valid HTTPS certificate is already present` error.
   In that case you will have to run the following command, and then  `Re-Run Step 1`

```
 dotnet dev-certs https --clean
```

 3- Trust the certificate

```
 dotnet dev-certs https --trust
```

 4- Build and run docker containers
    
 Move to ```docker-compose.yml``` file directory

```
 docker-compose -f 'docker-compose.yml' up --build
```
    
 ### Add New Model
 
1- Create a new class for the new model: In the Models folder of the project, create a new class for the new model you want to add. Give the class a descriptive name that reflects what the model represents.

2- Inherit from IEntity: In the new class, inherit from the IEntity interface. This will ensure that the new model inherits the properties defined in the base interface.

 ```csharp
namespace Template.Backend.Model.Entities
{
    public class Foo : IEntity
    {
        public int ID { get; set; }
        public int RowVersion { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string Name { get; set; }
    }
}
 ```
 
3- Create a new class for the Audit

 ```csharp
namespace Template.Backend.Model.Audit.Entities
{
    public class FooAudit : IAuditEntity
    {
        public int FooAuditID { get; set; }
        public int RowVersion { get; set; }
        public int ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public AuditOperations AuditOperation { get; set; }
        public string LoggedUserName { get; set; }
        
        public string Name { get; set; }
    }
}
 ```
 
 4- Add a new DbSet: In the DbContext class of the project, add a new DbSet for the new model. This will allow you to access and manipulate the data for the new model in the database.

 ```csharp
        public DbSet<Foo> Foo { get; set; }
        // Audit
        public DbSet<FooAudit> FooAudit { get; set; }
 ```
 
 5- Add Database configuration for new models

 ```csharp
class FooConfiguration : IEntityTypeConfiguration<Foo>
    {
        public void Configure(EntityTypeBuilder<Foo> builder)
        {
            builder.ToTable("Foos");
            builder.Property(a => a.Name).IsRequired()
        }
    }
    
    class FooAuditConfiguration : IEntityTypeConfiguration<FooAudit>
    {
        public void Configure(EntityTypeBuilder<Foo> builder)
        {
            builder.ToTable("FooAudit");
        }
    }
 ```

6- Generate a new migration: Run the command `Add-Migration <MigrationName>`  in the Package Manager Console to generate a new migration that will create a new tables for the new models in the database.

7- Update the database: Run the command `Update-Database` in the Package Manager Console to apply the changes to the database.

8- Update the services and repositories: If you are adding new functionality to the new model, you will need to update the corresponding service and repository classes to support the new functionality. For example, if the new model has a complex business logic or requires additional data validation, you will need to update the service and repository classes to handle these requirements.

  ```csharp
  public class FooRepository : RepositoryBase<Foo>, IFooRepository
    {
        public FooRepository(StarterDbContext dbContext) : base(dbContext)
        {
        }
    }
  
  public class FooService : Service<Foo>, IFooService
    {
        private readonly IFooRepository _FooRepository;
        public FooService(IFooRepository companyRepository, IUnitOfWork unitOfWork, IValidationDictionary validatonDictionary)
            : base(fooRepository, unitOfWork, validatonDictionary)
        {
            _FooRepository = fooRepository;
        }
    }
  
  public class FooAuditService : ServiceAudit<FooAudit>, IFooAuditService
    {
        public FooAuditService(IAuditRepository<FooAudit> FooAuditRepository)
            : base(FooAuditRepository)
        {
        }
    }
 ```
  
9- Add Dto for new models.
  
   ```csharp

    public class FooDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
  ```
  
10- Add mapping for new models.
  
   ```csharp

    private IMappingExpression<FooDto, Foo> FooDtoMapping()
        {
            return CreateMap<FooDto, Foo>()
                .ForMember(m => m.RowVersion, map => map.Ignore())
                .ForMember(m => m.CreatedOn, map => map.Ignore());
        }
  ```
  
11- Add routes for new models.
  
   ```csharp
public const string FooPrefix = "api/v{version:apiVersion}/Foos";
  ```
  
12- Add Api controllers for new models.
  
   ```csharp
  
    [ApiController]
    [ApiVersion("1")]
    [Route(ApiRouteConfiguration.FooPrefix)]
    public class FooApiController : BaseApiController<Foo, FooAudit>
  ```

13- Add unit tests and integration tests for new models.
  
14- Update the client code: If the new model is used by client applications, you will need to update the client code to support the new model. This might involve updating the data access code, the user interface, or other parts of the client application.

```csharp
  public class FooClient : Client<Foo, FooAudit>, IFooClient
    {
        public FooClient()
        {
        }

        public IEnumerable<Foo> GetAll(AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetObjects(ApiConfiguration.FooApiRoute, authHeaderValue);
        }
    }
  ```

## Support
If you are having problems, please let us know by [raising a new issue](https://github.com/fouad-elouad/Template.Backend/issues/new/choose).

## License
This project is licensed with the [MIT license](LICENSE).
