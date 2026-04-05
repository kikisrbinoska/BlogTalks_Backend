# BlogTalks

BlogTalks is a full-stack blogging platform built with ASP.NET Core, Clean Architecture, and CQRS principles. It supports blog post management, user authentication, commenting, real-time email notifications via a dedicated microservice, and a React-based frontend.


## Overview

BlogTalks allows users to register, authenticate, and manage blog posts with support for tagging, searching, filtering, and pagination. Commenting is supported with asynchronous email notifications sent to post authors when their content receives a comment. Communication between the main API and the email microservice is handled through RabbitMQ.

---

## Architecture

The application follows Clean Architecture principles with a clear separation of concerns across layers:

- **Domain** - Core entities and business rules
- **Application** - CQRS handlers, commands, queries, validators, and interfaces
- **Infrastructure** - Database access, repository implementations, messaging, and external integrations
- **Presentation (API)** - ASP.NET Core Web API controllers and middleware

The CQRS pattern is implemented throughout the Application layer, separating read and write operations for maintainability and scalability.

---

## Technology Stack

**Backend**

- ASP.NET Core (.NET 8)
- Entity Framework Core with PostgreSQL
- MediatR (CQRS)
- FluentValidation
- Serilog
- JWT Authentication
- RabbitMQ (via MassTransit or direct client)
- XUnit and Moq (unit testing)

**Infrastructure**

- PostgreSQL (containerized via Docker)
- RabbitMQ (containerized via Docker)

**Frontend**

- React (created with Lovable)
- Connected to BlogTalks API via REST

**Tooling**

- Docker and Docker Compose
- GitHub Flow (branching strategy)
- GitHub Copilot (AI-assisted development)

---

## Features

**Blog Posts**

- Create, edit, and delete blog posts (title, body, tags)
- List all posts with pagination
- View a single post with its associated comments
- Search posts by keyword, author, or tag
- Filter posts by tag
- View posts authored by other users
- Delete only your own posts

**Users**

- User registration
- Login and logout with JWT-based authentication
- Blog posts are associated with the authenticated user

**Comments**

- Add comments to any blog post
- View comments alongside a post

**Email Notifications**

- Email notification sent to a post author when their post receives a new comment
- Notifications are dispatched asynchronously through RabbitMQ to a dedicated email microservice

**Validation and Logging**

- FluentValidation on all incoming requests
- Structured logging throughout the application using Serilog

---

## Project Structure

```
BlogTalks/
  src/
    BlogTalks.Domain/           # Entities: BlogPost, Comment, User, Tag
    BlogTalks.Application/      # CQRS commands, queries, validators, interfaces
    BlogTalks.Infrastructure/   # EF Core, PostgreSQL, Repository pattern, RabbitMQ
    BlogTalks.API/              # ASP.NET Core Web API, controllers, JWT config

BlogTalks.EmailService/         # Standalone .NET Web API microservice for sending emails
  Controllers/
  MinimalApi/
  Services/

BlogTalks.Tests/                # Unit tests with XUnit and Moq

frontend/                       # React application (created with Lovable)

docker-compose.yml              # PostgreSQL and RabbitMQ containers
```

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker and Docker Compose
- Node.js (for the frontend)

### 1. Clone the repository

```bash
git clone https://github.com/your-username/blogtalks.git
cd blogtalks
```

### 2. Start infrastructure services

```bash
docker-compose up -d
```

This starts PostgreSQL and RabbitMQ in Docker containers.

### 3. Apply database migrations

```bash
cd src/BlogTalks.API
dotnet ef database update
```

### 4. Run the main API

```bash
dotnet run --project src/BlogTalks.API
```

### 5. Run the email microservice

```bash
dotnet run --project BlogTalks.EmailService
```

### 6. Run the frontend

```bash
cd frontend
npm install
npm run dev
```

---

## Configuration

### appsettings.json (Main API)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=blogtalks;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Key": "your_secret_key",
    "Issuer": "BlogTalks",
    "Audience": "BlogTalksUsers",
    "ExpiresInMinutes": 60
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  },
  "Serilog": {
    "MinimumLevel": "Information"
  }
}
```

### appsettings.json (Email Microservice)

```json
{
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  },
  "Email": {
    "SmtpHost": "smtp.example.com",
    "SmtpPort": 587,
    "SenderEmail": "noreply@blogtalks.com",
    "SenderName": "BlogTalks",
    "Username": "your_smtp_username",
    "Password": "your_smtp_password"
  }
}
```

---

## API Reference

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/auth/register | Register a new user |
| POST | /api/auth/login | Login and receive a JWT token |
| POST | /api/auth/logout | Logout |

### Blog Posts

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/posts | List posts with optional pagination |
| GET | /api/posts/{id} | Get a single post with comments |
| POST | /api/posts | Create a new post (authenticated) |
| PUT | /api/posts/{id} | Edit an existing post (owner only) |
| DELETE | /api/posts/{id} | Delete a post (owner only) |
| GET | /api/posts/search | Search posts by keyword, user, or tag |

### Comments

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/posts/{id}/comments | Add a comment to a post (authenticated) |

### Query Parameters

- `page` - Page number (default: 1)
- `pageSize` - Items per page (default: 10)
- `tag` - Filter posts by tag
- `keyword` - Search by keyword in title or body
- `userId` - Filter posts by author

---

## Email Notification Microservice

The email microservice is a standalone .NET Web API application responsible for sending notification emails. It exposes both a controller-based REST API and a minimal API endpoint for flexibility.

The microservice subscribes to a RabbitMQ queue. When the main BlogTalks API publishes a comment event, the microservice consumes the message and sends an email to the post author.

**Communication flow:**

```
User adds comment
      |
BlogTalks.API publishes message to RabbitMQ
      |
BlogTalks.EmailService consumes message
      |
Email sent to post author
```

---

## Frontend

The React frontend was generated using Lovable and connected to the BlogTalks API. It supports:

- User registration and login
- Browsing and searching blog posts
- Viewing a post and its comments
- Creating and managing your own posts
- Commenting on posts

The frontend communicates with the API using JWT tokens stored client-side and passed as Authorization headers.

---

## Testing

Unit tests are located in the `BlogTalks.Tests` project and use XUnit as the test framework with Moq for mocking dependencies.

```bash
cd BlogTalks.Tests
dotnet test
```

Tests cover Application layer handlers including commands and queries, with mocked repositories and services.

