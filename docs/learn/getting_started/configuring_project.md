# Configuring the Project

In this guide, we will walk you through how to modify your Program.cs file to authenticate using Firebase, connect to both PostgreSQL and MongoDB databases, create an API documentation page and register the Codeed.Framework services.

## Program.cs in ASP.NET Minimal API Project

Program.cs is a key file in an ASP.NET Minimal API Project. It is the entry point of the application and is responsible for configuring and running the application.

In this section, we will go through the code in Program.cs line by line to explain what each line does.

```csharp
var builder = WebApplication.CreateBuilder(args);
```

This line creates a new instance of the WebApplication class and initializes it with the command-line arguments passed to the application.

```csharp
builder.Services.AddControllers();
```

This line adds the Controllers feature to the application. This feature allows you to define controllers to handle HTTP requests.

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

These two lines add support for Swagger/OpenAPI documentation to the application. Swagger/OpenAPI is a tool that allows you to document your APIs and generate client libraries in multiple languages.

```csharp
var app = builder.Build();
```

This line builds the WebApplication instance and returns it as an IApplicationBuilder instance. This IApplicationBuilder instance is used to configure the application's request processing pipeline.

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

This block of code adds Swagger/OpenAPI middleware to the application only if it's running in the development environment. The middleware is responsible for generating the Swagger/OpenAPI documentation and serving it to clients.

```csharp
app.UseHttpsRedirection();
```

This line adds middleware to redirect HTTP requests to HTTPS if the application is running on a secure server.

```csharp
app.UseAuthorization();
```

This line adds middleware to enable authentication and authorization for the application. This middleware is responsible for enforcing access control rules on incoming requests.

```csharp
app.MapControllers();
```

This line maps the controllers defined in the application to the incoming HTTP requests. This enables the application to route requests to the appropriate controller based on the request URL and HTTP verb.

```csharp
app.Run();
```

This line starts the application and runs it indefinitely. This is the last line of code in the Program.cs file.

## Install PostgreSQL packages

The Codeed.Framework utilizes EntityFramework as an Object-Relational Mapping (ORM) tool, which allows developers to work with different databases in their application. For our specific example, we have decided to use PostgreSQL as our database management system. In order to incorporate PostgreSQL into the Learn.Web project, it is necessary to install a specific package. This package will provide the necessary tools and functionalities for the Learn.Web project to interface with the PostgreSQL database.

=== "Learn.Web"

    ``` powershell
    Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
    ```

=== "Learn.Data"

    ``` powershell
    Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
    ```    

## Changing the Program.cs

Configure Serilog









