# Use Azure App Services for deploying VolleyManagement.API container

* Status: ✔ accepted
* Deciders: Sergii Diachenko
* Date: 2019-08-23

## Context and Problem Statement

We need a hosting provider to host application. It should be cloud capable of hosting Docker containers.

## Decision Drivers <!-- optional -->

* cost
* ease of deployment
* custom domains
* https

## Considered Options

* Azure App Service

## Decision Outcome

Chosen option: "Azure App Service".

Honestly this was chosen because I had experience running VolleyManagement V2 on Azure App Service. Price is acceptable and I did not have time to evaluate other options I'd want:

* Digital Ocean
* GCP App Engine Flexible environment

## Links

* Depends on [ADR-0004](0004-use-docker-for-hosting-application.md)
* Influences [ADR-0006](0006-use-azure-table-storage-for-persistence.md)
