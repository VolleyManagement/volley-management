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

Not yet faced :)

## Pros and Cons of the Options <!-- optional -->

### dotnet CLI

[example | description | pointer to more information | …] <!-- optional -->

* ✔ Consistent environment
* ✔ Automation capabilities
* ✔+- Ease of deployment
* ✔ Ease of development

### [option 2]

[example | description | pointer to more information | …] <!-- optional -->

* ✔ [argument a]
* ✔ [argument b]
* ❌ [argument c]
* Consistent environment
* Automation capabilities
* Ease of deployment
* Ease of development
