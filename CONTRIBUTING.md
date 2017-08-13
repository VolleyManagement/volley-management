# Contributing to Volley Management

First off, thanks for taking the time to contribute!

The following is a set of guidelines for contributing to Volley Managment. These are mostly guidelines, not rules. Use your best judgment, and feel free to propose changes to this document in a pull request.

🏗 UNDER CONSTRUCTION 🏗

More content to be added :timer_clock:

## Code of Conduct ##

This project and everyone participating in it is governed by the [Code of Conduct](https://bitbucket.org/VolleyManagement/volleymanagement/src/master/CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to [Sergii Diachenko](https://bitbucket.org/sdiachen/).

## How to reach team and/or ask question? ##

First of all check out [Wiki](https://bitbucket.org/VolleyManagement/volleymanagement/wiki/Home) to get some basic information.

Note: :warning: Some articles in requirements section might not be up to date. It is hard to keep it up to date :(. There are couple of ideas to use some kind of live documentation tool. As for now look at unit tests to see actual requirements.

We have Slack chat [volleymanagement.slack.com](https://volleymanagement.slack.com/). As the project is relatively small all questions can be asked in `#general` channel

As for now to get access to this chat please write email to [Sergii Diachenko](https://bitbucket.org/sdiachen/). In future we might get this automated.

## How Can I Contribute? ##
### Reporting Bugs ###
Log issue as __bug__ in the [issue tracker](https://bitbucket.org/VolleyManagement/volleymanagement/issues?status=new&status=open).

Before creating bug reports, please check existing open issues as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible.
### Suggesting Improvements ###
Log issue as __enhcancement__ or __proposal__ in the [issue tracker](https://bitbucket.org/VolleyManagement/volleymanagement/issues?status=new&status=open).

Before creating suggestions reports, please check existing open issues as you might find out that you don't need to create one. Please include as many details as possible including but not limited to reasononing, design decisions, potential impact on existing code.
### Pull Requests ###
If you want to directly contribute to the project you are highly welcome!

You can do so by creating pull request into `master` branch. After review your request will either be merged or set of questions/suggestions will be given to you to address before accepting PR.
### Visual Studio online task tracker ###
As this project has been mainly contributed by group of students learning .NET web development majority of the functional features is kept in Visual Studio Online. As it provides better workflow for student group working on the project every day.

Having said that OSS contributions is better managed by Bitbucket cloud issue tracker. Thus this is the recommended place to log new issues.

At some point in the future we will have single consolidated backlog.

If you want to see issues stored in VS Online please contact [Sergii Diachenko](https://bitbucket.org/sdiachen/) for access.

## Development Environment ##
Minimum sofware requirements:
* .NET 4.7
* VS 2017 any edition
* SQL Server 2008-2016 (2016 LocalDB is default but you can change connection string)

All dependencies are managed via NuGet so you don't need to take any additional steps to acquire dependencies.

Project uses integration with several external systems:
* Google OAuth provider
* Re-captcha
* Gmail and/or SendGrid
In order to test/debug/use features using those integrations you might need to create appropriate profiles at following providers and use secrets provided in order to run system.

🚧 ToDo: Provide short guide on how to accomplish that.