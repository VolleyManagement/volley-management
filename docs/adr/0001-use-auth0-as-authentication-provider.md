# Use Auth0 as authentication provider

* Status: ✔ accepted
* Deciders: Sergii Diachenko
* Date: 2019-08-22

## Context and Problem Statement

How to implement authentication?

## Decision Drivers

* Volley Management system should restrict access only to the authenticated users
* Prefer buy vs build as authentication is not a core domain
* cost should be minimized

## Considered Options

* Write custom logic based on ASP.NET Identity and/or IdentityServer
* Auth0
* Azure AD B2C

## Decision Outcome

Chosen option: Auth0, because it has free tier and integration will take 2-5 hours of work.

### Positive Consequences

* Fast time to implement

* We have a possibility to move to another solution quickly once need arises

### Negative Consequences

* We might be limited by Auth0 free tier

  * Free tier has limit on number of authentications per minute

## Pros and Cons of the Options

### Write custom logic based on ASP.NET Identity and/or IdentityServer

Use ASP.NET identity to store user information and issue authentication tokens.

* ✔ you have full control and implement what you need
* ❌ writing custom security is not a good practice
* ❌ you have to write code
* ❌ you need to persist user information and passwords

### Auth0

* ✔ quick to integrate
* ✔ Free tier
* ✔ SaaS
* ❌ limited free tier
* ❌ rate limiting on free tier

### Azure AD B2C

Azure AD B2C is IDaaS solution intended to work with various identity providers

* ✔ quick to integrate
* ✔ Free tier
* ✔ SaaS
* ❌ Does not have own user storage ❓
* ❌ free tier limitations
