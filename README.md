# Communication Platform - REST API
A REST API for a communication platform, built using .NET 7.

Version: Draft 1

## Features
- Classification: How to classify your receivers. Ex: Customer, Partner, Internal
- Metadata: Data-field that is used to generate messages, each metadata field can have one or many classifications: Ex: Firstname, Lastname, DateOfBirth
- Receiver: The receiver of the messeage sent by the communication platform.

## Requirements
- .NET 7 sdk
- Postgresql

# Techniques
- File Structure: Vertical Slicing/Feature Folder
- Dependency Injection (DI)
- Inversion of Control (IOC)
- Test Driven Development (TDD): Testing input & output to service, 
admin access required.
- Clean Code'ish: Refactoring needed
- Error/Result: Return error instead of throwing
- Crud rest api
- Repository Pattern

# Project references
## Service
[Entity Framework Core](https://github.com/dotnet/efcore) - 

[Npgsql Entity Framework](https://github.com/npgsql/efcore.pg) - Npgsql Entity Framework Core provider for PostgreSQL

[ErrorOr](URLhttps://github.com/amantinband/error-or)

## Testing
[xUnit.net](https://github.com/xunit/xunit) - testing tool

[FakeItEasy](https://fakeiteasy.github.io/) - Mocking

[AutoFixture](https://github.com/AutoFixture/AutoFixture) - Fixture Setup

[FluentAssertions ](https://github.com/fluentassertions/fluentassertions)- Naturally specify the expected outcome of a test

# Todo
- Logging: Serilog
- Security: Key or username/password?
- Reactoring: Extract generic code to class libary

# Lab
Fluent Docker?

[Marten - postgre as document db / event sourcing](https://github.com/JasperFx/marten)

[rabbit mq - message broker](https://www.rabbitmq.com/)  Pub Sub ?

[Bogus](https://github.com/bchavez/Bogus) Test/Demo data generator

Dto Mapper?