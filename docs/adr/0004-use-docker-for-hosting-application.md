# Use Docker for hosting application

* Status: ✔ accepted
* Deciders: Sergii Diachenko
* Date: 2019-08-23

## Context and Problem Statement

[Describe the context and problem statement, e.g., in free form using two to three sentences. You may want to articulate the problem in form of a question.]

## Decision Drivers

* Consistent environment
* Automation capabilities
* Ease of deployment
* Ease of development

## Considered Options

* dotnet CLI
* Docker

## Decision Outcome

Chosen option: "Docker", because it allows to have same environment locally as well as in production. Every dev and environment will have same configuration.

dotnet CLI is quite good at it as well but it requires a little bit more specifics, while Docker can be hosted virtually everywhere.

### Positive Consequences

* Docker has large support among hosting providers

### Negative Consequences

Local development for docker might be harder as you need to rebuild container. While VS team puts a lot of effort into it it is still slower than run it locally on .NET Core.

We can do major development using local setup with .NET Core, but all CI steps automated using docker.

## Pros and Cons of the Options

### dotnet CLI

* ✔ Consistent environment
* ✔ Automation capabilities
* ✔+- Ease of deployment
* ✔ Ease of development

### Docker

* ✔ Consistent environment
* ✔ Automation capabilities
* ✔ Ease of deployment
* ✔+- Ease of development

## Links

* Affects [ADR-0005](0005-use-azure-app-services-for-hosting.md)
