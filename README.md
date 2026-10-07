# UniManager

UniManager is a .NET 10 Web API for managing university students, teachers, courses, classrooms, and course enrollments. The solution is organized into API, business-logic, repository, and model projects, and uses Entity Framework Core with SQL Server.

## Projects

- **UniManagerApi** — ASP.NET Core API and dependency-injection setup
- **UniManager.Logic** — application and enrollment logic
- **UniManager.Repository** — generic EF Core repository, `UniDbContext`, and database migrations
- **UniManager.Models** — domain entities and request DTOs

## Requirements

- .NET 10 SDK
- SQL Server or SQL Server LocalDB
- The `dotnet-ef` tool for applying migrations (`dotnet tool install --global dotnet-ef`, if not already installed)

## Configure and run

The API reads its SQL Server connection string from `ConnectionStrings:DefaultConnection`. Configure it using user secrets, environment variables, or local configuration; do not commit credentials.

For example, in PowerShell, set an environment variable for a local LocalDB instance:

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=(localdb)\MSSQLLocalDB;Database=UniManagerDb;Trusted_Connection=True;TrustServerCertificate=True;'
```

From the solution directory, apply the EF Core migrations and start the API:

```powershell
dotnet ef database update --project UniManager.Repository --startup-project UniManagerApi
dotnet run --project UniManagerApi
```

The HTTP launch profile uses `http://localhost:5234`; the HTTPS profile uses `https://localhost:7191`. In Development, Swagger UI is available at `/swagger` (for example, `http://localhost:5234/swagger`).

## API endpoints

The API uses `api/[controller]` routes. Controller names determine route segments, so the teachers route is `/api/Teachers`.

| Resource | Method | Route | Notes |
|---|---|---|---|
| Students | GET | `/api/Student/all` | List students |
| Students | GET | `/api/Student/{id}` | Get a student |
| Students | POST | `/api/Student` | Create a student |
| Students | PUT | `/api/Student?id={id}` | Update a student; `id` is a query parameter |
| Courses | GET | `/api/Course/all` | List courses |
| Courses | GET | `/api/Course/{id}` | Get a course |
| Courses | POST | `/api/Course` | Create a course |
| Courses | PUT | `/api/Course/{id}` | Update a course |
| Teachers | GET | `/api/Teachers/all` | List teachers |
| Teachers | GET | `/api/Teachers/{id}` | Get a teacher |
| Teachers | POST | `/api/Teachers` | Create a teacher |
| Teachers | PUT | `/api/Teachers?id={id}` | Update a teacher; `id` is a query parameter |
| Classrooms | GET | `/api/ClassRoom/all` | List classrooms |
| Classrooms | GET | `/api/ClassRoom/{id}` | Get a classroom |
| Classrooms | POST | `/api/ClassRoom` | Create a classroom |
| Classrooms | PUT | `/api/ClassRoom/{id}` | Update a classroom |
| Enrollments | GET | `/api/Enrollment/all` | List enrollments |
| Enrollments | POST | `/api/Enrollment` | Enroll a student in a course |
| Enrollments | PUT | `/api/Enrollment/{id}` | Update an enrollment |

### Example requests

Create a student (the API sets `EnrollmentDate` to the current server time):

```json
{
  "firstName": "Ada",
  "lastName": "Lovelace"
}
```

Create a classroom:

```json
{
  "roomName": "Science 101",
  "capacity": 30
}
```

Create a course:

```json
{
  "title": "Introduction to Computing",
  "credits": 3,
  "teacherId": 1,
	"classRoomId": 1
}
```

Enroll an existing student in an existing course. The logic sets the initial grade to `Not Graded` and status to `Enrolled`:

```json
{
  "studentId": 1,
  "courseId": 1
}
```

## Database migrations

Migrations are in `UniManager.Repository/Migrations`. To create a migration after changing the EF Core model:

```powershell
dotnet ef migrations add MigrationName --project UniManager.Repository --startup-project UniManagerApi
```

Apply pending migrations with `dotnet ef database update` as shown above. Ensure the API and migration commands use the same `ConnectionStrings:DefaultConnection` value and database.
