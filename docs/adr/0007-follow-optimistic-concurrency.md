# Use Optimistic Concurrency

* Status: ✔ accepted
* Deciders: Sergii Diachenko
* Date: 2021-04-21

## Context and Problem Statement

As any modern system, Volley Management faces a problem of concurrent changes to data and we need to support such scenario.

We explicitly do not consider an option to go without concurrency checks - time will tell if it is a good decision).

## Decision Drivers <!-- optional -->

* Performance - decision should support high throughput scenarios
* Maintainability - amount of code needed to write should be minimized

## Considered Options

* Optimistic Concurrency
* Pessimistic Concurrency

## Decision Outcome

Chosen option: "Optimistic concurrency", because it is a better from the perfromance perspective and it will support our choice to [use Azure Table Storage](0006-use-azure-table-storage-for-persistence.md).

### Positive Consequences <!-- optional -->

* Concurrency check will fail very rarely so it will have almost no overhead
* It is aligned with Azure Table storage Optimistic concurrency

### Negative Consequences <!-- optional -->

* Whole application will have a responsibility to manage versions properly
* If we change a datastore we might need to have a larger change

## Pros and Cons of the Options <!-- optional -->

### Optimistic concurrency

Storage layer has some indication of 'row version' for each record which changes every time the change to the record is made. When write operation happens it checks for row version match beforehand.

* ✔ Has better performance
* ✔ Azure Table Storage uses Optimistic concurrency model
* ❌ our business logic will have to take this into account

### Pessimistic Concurrency

When something is going to be edited record is locked for change. And everyone waits while operation completes.

* ✔ Easier to implement
* ❌ Terrible performance
* ❌ Is not aligned to Azure Table storage

## Links <!-- optional -->

* Partially a consequence of [ADR-0006](0006-use-azure-table-storage-for-persistence.md)
