# JourneyJoy App

Backend-oriented travel planning application developed as an engineering thesis project at Warsaw University of Technology.

## Tech Stack

- C#
- ASP.NET Core / .NET 6
- Entity Framework Core
- SQL Server / SQLite
- JWT Authentication
- Swagger / OpenAPI
- Azure App Configuration
- GitHub Actions
- Azure Web App

## Project Overview

JourneyJoy is a travel planning application created as an engineering thesis project.  
My main responsibility was backend development, including API structure, data access, authentication, configuration, tests and deployment workflow.

## Architecture

The solution is organized into multiple projects:

- `JourneyJoy.Backend` — main ASP.NET Core backend application
- `JourneyJoy.Repository` — data access layer
- `JourneyJoy.Model` — domain/data models
- `JourneyJoy.Contracts` — shared contracts/DTOs
- `JourneyJoy.ExternalAPI` — external API integrations
- `JourneyJoy.Algorithm` — application-specific algorithmic logic
- `JourneyJoy.BackendTests` — backend tests
- `JourneyJoy.IntegrationTests` — integration tests

## Engineering Highlights

- Modular backend structure
- Entity Framework Core data access
- JWT-based authentication
- Swagger/OpenAPI documentation
- Unit and integration tests
- GitHub Actions build/test workflow
- Azure Web App deployment workflow

## How to Run Locally

1. Clone the repository.
2. Configure the database connection string.
3. Apply Entity Framework migrations.
4. Run the backend project.
5. Open Swagger UI to test available endpoints.

## Status

Engineering thesis project / portfolio backend project.
