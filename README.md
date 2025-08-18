# Job Board Backend API

A comprehensive, enterprise-grade job board application built with .NET 9, implementing Clean Architecture, Domain-Driven Design (DDD), and CQRS patterns.

## 🏗️ Architecture Overview

This project demonstrates modern .NET development practices with a focus on maintainability, scalability, and testability.

### Core Architectural Patterns

- **Clean Architecture**: Clear separation of concerns with dependency inversion
- **Domain-Driven Design (DDD)**: Rich domain models with aggregates and domain events
- **CQRS (Command Query Responsibility Segregation)**: Separate read and write operations using MediatR
- **Modular Monolith**: Self-contained modules with clear boundaries
- **Repository Pattern**: Data access abstraction with Entity Framework Core
- **Specification Pattern**: Flexible and reusable query logic

### Project Structure

```
src/
├── Api/                           # API entry point and composition root
├── Core/                          # Shared kernel and common functionality
│   ├── Application/               # Application services and cross-cutting concerns
│   │   ├── CQRS/                 # Command/Query interfaces and handlers
│   │   ├── Behaviors/            # MediatR pipeline behaviors
│   │   ├── Exceptions/           # Custom exceptions and global error handling
│   │   ├── Specifications/       # Specification pattern implementation
│   │   └── Pagination/           # Pagination abstractions
│   ├── Domain/                    # Core domain interfaces and base classes
│   └── Infrastructure/           # Infrastructure concerns and extensions
└── Modules/                       # Business modules (Bounded Contexts)
    ├── Authentication/           # User authentication and authorization
    ├── Job/                     # Job management
    └── JobApplication/          # Job application workflow
```

### Domain Modules

Each module follows the same structure:
- **Domain**: Entities, Value Objects, Domain Events, Aggregates
- **Application**: Use Cases (Commands/Queries), DTOs, Validators
- **Infrastructure**: Data persistence, external services, configurations

## 🚀 Getting Started

### Prerequisites

- **.NET 9 SDK** - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
- **Docker & Docker Compose** - For database and logging infrastructure
- **PostgreSQL** - Runs in Docker container
- **Git** - Version control

### Local Development Setup

#### 1. Clone the Repository
```bash
git clone <repository-url>
cd job-board-backend
```

#### 2. Start Infrastructure Services
```bash
# Start PostgreSQL and Seq logging server
docker-compose up -d

# Verify containers are running
docker ps
```

**Docker Services:**
- **PostgreSQL** (`job_board_db`): Primary database on port 5432
- **Seq** (`seq`): Centralized logging and monitoring on ports 5341 (ingestion) and 9091 (UI)

#### 3. Configure Environment Variables
```bash
# Copy and configure environment variables
cp .env.template .env

# Default configuration (already set in .env):
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_DB=job_board_db
DEFAULT_USER_PASSWORD=1234User
JWT_SECRET=q1p9B8kL2vXe0uR5zH7mN4aW6tY3sC8dF1gJ0rV5xP
JWT_ISSUER=JobPortal
JWT_AUDIENCE=JobPortal-Users
JWT_EXPIRATION=24
```

#### 4. Database Setup
```bash
# Navigate to API project
cd src/Api

# Run database migrations and seed data
dotnet ef database update --context AuthenticationDbContext
dotnet ef database update --context JobDbContext  
dotnet ef database update --context JobApplicationDbContext

# Or simply run the application (migrations run automatically)
dotnet run
```

#### 5. Run the Application
```bash
# Development mode with hot reload
dotnet run --urls="http://localhost:5000"

# Or use Visual Studio/Rider with the provided launch settings
```

### Docker Benefits for Development

- **Consistent Environment**: Same database version across all developers
- **Isolated Services**: No conflicts with local PostgreSQL installations
- **Centralized Logging**: Seq provides structured log viewing and filtering
- **Easy Reset**: `docker-compose down -v` removes all data for fresh start
- **Production Parity**: Development environment mirrors production setup

## 📚 API Documentation

### Swagger/OpenAPI Documentation

Access comprehensive API documentation at:
- **Swagger UI**: `http://localhost:5000/swagger`
- **OpenAPI JSON**: `http://localhost:5000/swagger/v1/swagger.json`

The documentation includes:
- All endpoints with request/response schemas
- Authentication requirements
- Validation rules and error responses
- Example payloads and responses

### Alternative Documentation

- **Seq Logs**: `http://localhost:9091` - Real-time application logs
- **Built-in .http files**: See `src/Api/Api.http` for request examples

## 🎯 Available Endpoints

### Authentication Module (`/api/v1/auth`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/register` | User registration | ❌ |
| POST | `/login` | User authentication | ❌ |

**Sample Registration Request:**
```json
{
  "email": "user@example.com",
  "password": "StrongPassword123!",
  "role": "JobSeeker"
}
```

**Sample Login Response:**
```json
{
  "user": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "user@example.com",
    "role": "JobSeeker"
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Job Management (`/api/v1/jobs`)

| Method | Endpoint | Description | Auth Required | Roles |
|--------|----------|-------------|---------------|-------|
| GET | `/` | List all jobs with filtering | ❌ | All |
| GET | `/{id}` | Get job by ID | ❌ | All |
| POST | `/` | Create new job | ✅ | Admin, Employer |
| PATCH | `/{id}/status` | Update job status | ✅ | Admin, Employer |

**Sample Job Creation Request:**
```json
{
  "title": "Senior Backend Developer",
  "description": "We're looking for an experienced backend developer...",
  "requirements": "5+ years .NET experience, Docker, microservices",
  "companyName": "TechCorp Inc.",
  "companyWebsite": "https://techcorp.com",
  "location": "Remote",
  "workMode": "Remote",
  "status": "Active",
  "jobType": "FullTime",
  "applicationDeadline": "2025-12-31T23:59:59Z"
}
```

**Sample Jobs List Response:**
```json
{
  "jobs": {
    "pageIndex": 0,
    "pageSize": 10,
    "count": 25,
    "items": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440000",
        "title": "Senior Backend Developer",
        "description": "We're looking for an experienced...",
        "companyName": "TechCorp Inc.",
        "location": "Remote",
        "workMode": "Remote",
        "status": "Active",
        "jobType": "FullTime",
        "applicationDeadline": "2025-12-31T23:59:59Z",
        "createdAt": "2025-08-18T10:30:00Z",
        "updatedAt": "2025-08-18T10:30:00Z"
      }
    ]
  }
}
```

### Job Applications (`/api/v1/jobs` & `/api/v1/job-applications`)

| Method | Endpoint | Description | Auth Required | Roles |
|--------|----------|-------------|---------------|-------|
| POST | `/jobs/{jobId}/apply` | Apply for a job | ✅ | JobSeeker |
| PATCH | `/job-applications/{id}/status` | Update application status | ✅ | Admin, Employer |

**Sample Job Application Request:**
```json
{
  "coverLetter": "I am excited to apply for this position...",
  "resumeUrl": "https://example.com/resume.pdf"
}
```

**Sample Application Status Update:**
```json
{
  "status": "UnderReview"
}
```

### Query Parameters & Filtering

**Job Listing Filters:**
```
GET /api/v1/jobs?title=developer&location=remote&workMode=Remote&status=Active&pageSize=20&pageIndex=0
```

## 🔐 Authentication & Authorization

### User Roles
- **Admin**: Full system access
- **Employer**: Job and application management
- **JobSeeker**: Job browsing and application submission

### JWT Token Usage
```bash
# Include in Authorization header
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Default Test Users
```
Admin: admin@jobboard.com / 1234User
Employer: employer@jobboard.com / 1234User  
JobSeeker: jobseeker@jobboard.com / 1234User
```

## 🏛️ Design Patterns & Best Practices

### Implemented Patterns

1. **Command Query Responsibility Segregation (CQRS)**
   - Commands for write operations
   - Queries for read operations
   - MediatR for request/response handling

2. **Repository Pattern**
   - Abstracted data access
   - Testable data layer
   - Entity Framework Core implementation

3. **Specification Pattern**
   - Reusable query logic
   - Composable filtering criteria
   - AndSpecification, OrSpecification, NotSpecification

4. **Domain Events**
   - Decoupled domain logic
   - Event-driven architecture
   - Automatic event publishing via MediatR

5. **Pipeline Behaviors**
   - Cross-cutting concerns
   - Logging, validation, performance monitoring
   - Decorator pattern implementation

6. **Value Objects**
   - Encapsulated business logic
   - Immutable data structures
   - Rich domain modeling

### Code Quality Features

- **FluentValidation**: Comprehensive input validation
- **Mapster**: Fast object-to-object mapping
- **Carter**: Minimal API routing and organization
- **Serilog**: Structured logging with Seq integration
- **Global Exception Handling**: Consistent error responses
- **API Versioning**: Future-proof endpoint design

## 🛠️ Development Tools

### Database Migrations
```bash
# Add new migration
dotnet ef migrations add MigrationName --context AuthenticationDbContext

# Update database
dotnet ef database update --context AuthenticationDbContext

# Drop database (development)
dotnet ef database drop --context AuthenticationDbContext
```

### Logging & Monitoring
- **Seq Dashboard**: `http://localhost:9091`
- **Application Logs**: Real-time structured logging
- **Performance Metrics**: Request duration tracking
- **Error Tracking**: Exception details and stack traces

### Testing
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Data Models

### Core Entities

**JobEntity**
- Job postings with company information
- Work mode, location, and type specifications
- Application deadlines and status management

**UserEntity**  
- Authentication and role management
- Secure password hashing
- Email-based identification

**JobApplicationEntity**
- Application tracking and status workflow
- Resume and cover letter management
- Audit trail with timestamps

### Status Workflows

**Job Status**: `Active` → `Paused` → `Closed` / `Expired`
**Application Status**: `Submitted` → `UnderReview` → `Interviewed` → `Shortlisted` → `Hired` / `Rejected`

## 🔧 Configuration

Key configuration points:
- **Connection Strings**: Database connectivity
- **JWT Settings**: Token validation and expiration
- **Logging Levels**: Development vs production verbosity
- **CORS Policies**: Cross-origin request handling
- **Validation Rules**: Business logic enforcement

## 🚦 Production Considerations

- **Database Migrations**: Automated deployment pipelines
- **Health Checks**: Application and dependency monitoring
- **Rate Limiting**: API protection against abuse
- **Caching Strategy**: Redis for performance optimization
- **Monitoring**: Application Performance Monitoring (APM)
- **Security Headers**: HTTPS, HSTS, content security policies

---

**Built with ❤️ using .NET 9, Clean Architecture, and Domain-Driven Design principles.**