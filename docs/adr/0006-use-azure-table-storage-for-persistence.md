# Use Azure Table Storage for persistence

* Status: ✔ accepted
* Deciders: Sergii Diachenko
* Date: 2019-08-23

## Context and Problem Statement

We need a persistent state for the system. Previously I've used Azure SQL but it is quite pricey but has it's own set of features including relational model. But given that system is not very complex I can model persistence mechanism for NoSQL model.

## Decision Drivers

* Cost
* Backup capabilities

## Considered Options

* Azure SQL
* Azure Table Storage

## Decision Outcome

Chosen option: "Azure Table Storage", because cost.

### Positive Consequences <!-- optional -->

* Persistence bill should be down from ~$5/month to less than $1/month

### Negative Consequences <!-- optional -->

* Students in IT Academy won't be exposed to a relational model, which at this moment dominates work they will have to be doing. We will have to come up with a strategy to get them good experience.

## Pros and Cons of the Options <!-- optional -->

### Azure SQL

* ✔ Has good backup capabilities
* ✔ Relational model
* ❌ Cost
* ✔ Local dev env support

### Azure Table Storage

* ✔ Storage container can be backed up
* ❌ Relational model
* ✔ Cost
* ✔ Local dev env support
