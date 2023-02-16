# Communication Platform - REST API
A REST API for a communication platform, built using .NET 7.

Version: Draft 1 (Work in progress)

## Features
- Classification: How to classify your receivers. Ex: Customer, Partner, Internal
- Metadata: Data-field that is used to generate messages, each metadata field can have one or many classifications: Ex: Firstname, Lastname, DateOfBirth
- Receiver: The receiver of the messeage sent by the communication platform.

## Requirements
- .NET 7 sdk
- Postgresql

# Coding
- File Structure: Vertical Slicing/Feature Folder
- Dependency Injection (DI)
- Inversion of Control (IOC)
- Test Driven Development (TDD): Testing input & output to service, admin access required.
- Error/Result: Return error instead of throwing
- Crud rest api
- Repository Pattern    
- WarningAsError
- Nullible
- Clean Code'ish: Refactoring needed
- Asynchronous


# Project references
## Service
- [Entity Framework Core](https://github.com/dotnet/efcore) - 
- [Npgsql Entity Framework](https://github.com/npgsql/efcore.pg) - Npgsql Entity Framework Core provider for PostgreSQL
- [ErrorOr](URLhttps://github.com/amantinband/error-or)
- [Serilog](https://github.com/serilog/serilog)

## Testing
- [xUnit.net](https://github.com/xunit/xunit) - testing tool
- [FakeItEasy](https://fakeiteasy.github.io/) - Mocking
- [AutoFixture](https://github.com/AutoFixture/AutoFixture) - Fixture Setup
- [FluentAssertions](https://github.com/fluentassertions/fluentassertions)- Naturally specify the expected outcome of a test

# Todo
- [Serilog - logging](https://github.com/serilog/serilog)
- [Fluent Docker](https://github.com/mariotoffia/FluentDocker/)
- Security: Key or username/password?
- Reactoring: Hide boilerplate code
- Validation 
- Null Guards
- Secrets
- OpenTelemetry

# Maybe
- [Marten - postgre as document db / event sourcing](https://github.com/JasperFx/marten)
- [rabbit mq - message broker](https://www.rabbitmq.com/)  Pub Sub ?
- [Bogus](https://github.com/bchavez/Bogus) Test/Demo data generator
- Dto Mapper?
- Integration testing
