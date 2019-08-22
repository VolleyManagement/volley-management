# Use SendGrid for sending emails

* Status: ✔ accepted
* Deciders: Sergii Diachenko
* Date: 2019-08-22

Technical Story: [Email Notifications Feature](https://dev.azure.com/VolleyManagement/%CE%94%CE%B9%CE%B1%CF%87%CE%B5%CE%B9%CF%81%CE%B9%CF%80%CE%B7/_workitems/edit/1003)

## Context and Problem Statement

We need a way to send emails to users for several workflows.

## Considered Options

* SendGrid
* GMail

## Decision Outcome

Chosen option: SendGrid, because it is SaaS and has generous free tier. Integration is easy.

### Positive Consequences <!-- optional -->

* Fast time to develop

### Negative Consequences

* We need to think about setting up development environment

## Pros and Cons of the Options <!-- optional -->

### SendGrid

* ✔ has free tier
* ✔ has mail customization and templates
* ✔ fast integration
* ❌ hard to say

### GMail

You can use GMail servers to send emails from your own email

* ✔ free to use with some limitations
* ❌ emails will be coming from GMail address always
