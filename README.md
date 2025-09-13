# SocialHub - Domain Driven Design Project

A social media platform built with .NET 8 and Domain Driven Design principles.

## Project Structure

### Domain Layer
- **Common**: Base classes (Entity, ValueObject, AggregateRoot, IDomainEvent)
- **Users**: User bounded context with aggregates, value objects, and domain events
- **Posts**: Post bounded context with aggregates, value objects, and domain events

### Application Layer
- **Commands**: Use cases for modifying data (CreateUser)
- **Queries**: Use cases for reading data (GetUser)

### Infrastructure Layer (TODO)
- **Repositories**: Data access implementations
- **Domain Services**: Business logic implementations

## DDD Concepts Implemented

### 1. Value Objects
- `Email`, `Username`, `UserId` - Strongly typed with validation
- `PostContent`, `Hashtag`, `PostId` - Encapsulate business rules

### 2. Entities
- `User` - Aggregate root with domain events
- `Post` - Aggregate root with domain events
- `Comment` - Regular entity (part of Post aggregate)

### 3. Domain Events
- `UserCreatedEvent`, `UserFollowedEvent`, `UserDeactivatedEvent`
- `PostCreatedEvent`, `PostPublishedEvent`, `PostLikedEvent`

### 4. Repository Pattern
- `IUserRepository`, `IPostRepository` - Abstract data access

### 5. Application Services
- Command/Query separation pattern
- Use case handlers with dependency injection

## API Endpoints

### Users
- `POST /api/users` - Create a new user
- `GET /api/users/{userId}` - Get user by ID

## Next Steps

1. Implement repositories with Entity Framework
2. Implement domain services
3. Add authentication and authorization
4. Create frontend application
5. Add more features (posts, comments, likes)

## Learning Goals

- Domain Driven Design principles
- Clean Architecture
- CQRS pattern
- Repository pattern
- Domain Events
- Value Objects vs Entities
- Aggregate design 