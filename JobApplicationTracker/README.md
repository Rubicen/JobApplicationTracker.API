# Job Application Tracker API

A .NET 8 Web API for tracking job applications, using Entity Framework Core with SQLite and OpenAPI/Swagger documentation.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/download.html) (optional, for inspecting the database)
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or VS Code

---

## Getting Started

### 1. Clone the Repository
```git clone https://github.com/Rubicen cd JobApplicationTracker.API```

### 2. Configure the Database

The API uses SQLite. The connection string is set in `JobApplicationTracker/appsettings.json`:
"ConnectionStrings": { "DefaultConnection": "Data Source=jobapplications.db" 

You can change the database file path if needed.

---

### 3. Create the Database

Open a terminal in the solution root and run:
```dotnet ef database update --project JobApplicationTracker --startup-project JobApplicationTracker```
> **Note:**  
> If you haven't installed the EF Core CLI tools, run:  
> `dotnet tool install --global dotnet-ef`

---

### 4. Build and Run the API

Run the http API 
The API will need to be on port 5086 for it to be proxied by the Reach app.

You can view and test all endpoints directly from the Swagger UI.

### 4. Testing the API
Swagger UI is available after running the API to test.
There is also a unit test project included in the solution for testing the API endpoints.