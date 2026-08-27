# Booking API

## Overview
REST API for managing room slot bookings.
Built as a learning project to practice production-grade .NET backend development.

**Live demo:** https://booking-api-production-3593.up.railway.app/scalar/v1

## Tech Stack
- **ASP.NET Core (.NET 10)** — web framework
- **EF Core + PostgreSQL** — ORM and database
- **FastEndpoints** — endpoint-per-class approach instead of MVC controllers
- **MediatR** — CQRS implementation, decouples endpoints from handlers
- **Ardalis.Result** — explicit result handling without exceptions
- **FluentValidation** — request validation
- **Serilog** — structured JSON logging
- **Testcontainers** — integration tests with real PostgreSQL
- **Docker + Railway** — containerization and deployment

## Architecture Decisions

**Clean Architecture** — business logic is isolated from external dependencies.
Domain and Application layers have no knowledge of EF Core, FastEndpoints or PostgreSQL.
This enables testability and maintainability.

**CQRS + MediatR** — commands (writes) and queries (reads) are separated.
Endpoints only send a command through MediatR and never call handlers directly.

**Pipeline Behaviors** — cross-cutting concerns extracted from handlers:
- `ValidationBehavior` — validates commands before they reach the handler
- `SaveChangesBehavior` — automatic SaveChanges after every command handler

**Optimistic Concurrency via xmin** — protects against race conditions when two users
attempt to book the same slot simultaneously. PostgreSQL xmin is used as a concurrency
token. On conflict, EF Core throws DbUpdateConcurrencyException which is handled
in the command handler and returned as a 409 Conflict.

**GlobalExceptionHandler** — centralized exception handling with proper HTTP status
code mapping.

## Running Locally

**Prerequisites:** Docker Desktop

```bash
git clone https://github.com/MynameisIrina/booking-api.git
cd booking-api
docker-compose up --build
```

API available at `http://localhost:8080/scalar/v1`

## Endpoints

**Bookings**
- `POST /bookings/create` — create a booking
- `GET /bookings/{id}` — get booking by ID
- `DELETE /bookings/{id}` — cancel a booking (not allowed after 24 hours from creation)

**Room Slots**
- `GET /room-slots/available` — list available slots with pagination

**Health**
- `GET /health` — health check including PostgreSQL connectivity
