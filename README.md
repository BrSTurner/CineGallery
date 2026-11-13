# 🎬 CineGallery

**A .NET 10 Minimal API project focused on Clean Architecture, observability, performance monitoring, and modern software engineering practices.**

## 📖 Overview

CineGallery is a movie management API built with **ASP.NET Core 10 Minimal APIs** and structured around **Clean Architecture** principles.

The main goal of this project is not just to implement CRUD operations, but to provide a practical environment for exploring and applying software engineering concepts commonly used in production applications.

The project focuses on understanding how applications behave internally, how to identify performance bottlenecks, and how to improve maintainability, reliability, and observability.

CineGallery is also an evolving learning project, with additional technologies and architectural patterns planned for future iterations.

## 🛠️ Tech Stack

| Technology | Purpose |
|---|---|
| C# / .NET 10 | Backend development |
| ASP.NET Core Minimal APIs | HTTP endpoints |
| Entity Framework Core | Data access and persistence |
| Clean Architecture | Separation of concerns |
| OpenTelemetry | Distributed tracing and telemetry |
| DbCommandInterceptor | Database query performance monitoring |
| IExceptionHandler | Centralized exception handling |

## 🏗️ Architecture

The application follows **Clean Architecture**, keeping business rules independent of infrastructure and external frameworks.

The architecture is organized around four main responsibilities:

- **Domain:** Core entities and business rules.
- **Application:** Use cases, application logic, and abstractions.
- **Infrastructure:** Database access, persistence, and external integrations.
- **API:** HTTP endpoints, dependency injection, and application configuration.

This separation helps maintain a testable, extensible, and loosely coupled codebase.

## 🔍 Observability & Performance

One of the primary motivations behind CineGallery is to explore how applications can be monitored and diagnosed beyond traditional logging.

### OpenTelemetry

OpenTelemetry is used to explore observability concepts such as:

- Distributed tracing and request execution flows.
- Application metrics and request latency.
- Trace correlation across application components.
- Identifying bottlenecks in API requests and database operations.

The goal is to better understand how telemetry can provide visibility into application behavior and support troubleshooting.

### Database Query Monitoring

The project explores Entity Framework Core's `DbCommandInterceptor` to measure and analyze database command execution times.

This provides an opportunity to investigate:

- Query execution duration.
- Slow database operations.
- Potential performance bottlenecks.
- The relationship between database execution time and overall request latency.

### Global Exception Handling

Centralized exception handling is implemented using ASP.NET Core's `IExceptionHandler`.

The approach focuses on:

- Consistent API error responses.
- Standardized `ProblemDetails` responses.
- Structured exception logging.
- Separating error handling from endpoint business logic.
- Avoiding exposure of sensitive internal details.

## 🗺️ Roadmap

CineGallery is an ongoing project. Future improvements and experiments include:

- [ ] **AWS Integration** — Explore cloud infrastructure, deployment, and AWS services.
- [ ] **Custom Middleware** — Implement middleware components to better understand the ASP.NET Core request pipeline.
- [ ] **BenchmarkDotNet** — Benchmark code execution, memory allocation, and performance optimizations.
- [ ] **Vanilla JavaScript Frontend** — Build a lightweight interface using HTML, CSS, and JavaScript without a frontend framework.

Additional improvements may include automated testing, enhanced telemetry, database optimization experiments, and CI/CD integration.

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- A compatible database configured for the application
- An IDE such as Visual Studio, JetBrains Rider, or VS Code

### Run Locally

Clone the repository:

```bash
git clone https://github.com/BrSTurner/CineGallery.git
cd CineGallery
```

Restore dependencies:

```bash
dotnet restore
```

Configure the database connection and any required application settings.

Navigate to the API project directory and run:

```bash
dotnet run
```

The API will start using its configured local HTTP or HTTPS endpoint.

## 🎯 Project Goals

The main objectives of CineGallery are to:

1. Apply Clean Architecture principles in a practical .NET application.
2. Gain hands-on experience with observability and telemetry.
3. Understand how to diagnose API and database performance issues.
4. Explore the ASP.NET Core request pipeline and exception handling.
5. Experiment with benchmarking and optimization techniques.
6. Expand the application through cloud integrations and frontend development.

## 📌 Project Status

**Active development — Learning & experimentation**

CineGallery is a personal engineering project designed to evolve as new concepts, technologies, and best practices are explored.

The focus is on learning through implementation, measuring real application behavior, and understanding the trade-offs behind architectural and technical decisions.

---

**Built manually with C# and .NET 10.**
