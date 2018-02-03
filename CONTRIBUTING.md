# Contributing to Volley Management

First off, thanks for taking the time to contribute!

The following is a set of guidelines for contributing to Volley Managment. These are mostly guidelines, not rules. Use your best judgment, and feel free to propose changes to this document in a pull request.

🏗 UNDER CONSTRUCTION 🏗

More content to be added :timer_clock:

## Code of Conduct

This project and everyone participating in it is governed by the [Code of Conduct](/CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to Sergii Diachenko(mailto:sdiachenko AT outlook DOT com).

## How to reach team and/or ask question?

First of all check out [Wiki](https://github.com/VolleyManagement/volley-management/wiki) to get some basic information.

Note: :warning: Some articles in requirements section might not be up to date. It is hard to keep it up to date :(. There are couple of ideas to use some kind of live documentation tool. As for now look at unit tests to see actual requirements.

We have Slack chat [volleymanagement.slack.com](https://volleymanagement.slack.com/). As the project is relatively small all questions can be asked in `#general` channel

As for now to get access to this chat please write email to Sergii Diachenko(mailto:sdiachenko AT outlook DOT com). In future we might get this automated.

## How Can I Contribute?

### Reporting Bugs

Log issue as __bug__ in the [issue tracker](https://github.com/VolleyManagement/volley-management/issues).

Before creating bug reports, please check existing open issues as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible.

### Suggesting Improvements

Log issue as __enhcancement__ or __proposal__ in the [issue tracker](https://github.com/VolleyManagement/volley-management/issues).

Before creating suggestions reports, please check existing open issues as you might find out that you don't need to create one. Please include as many details as possible including but not limited to reasononing, design decisions, potential impact on existing code.

### Branch naming

Project has a CI configured to work with following branch naming pattern:

**issue_#{issue number}_short_issue_description_with_underscores**

### Pull Requests

If you want to directly contribute to the project you are highly welcome!

You can do so by creating pull request into `master` branch. After review your request will either be merged or set of questions/suggestions will be given to you to address before accepting PR.

As part of PR approval process project uses CI checks. Please following branch naming so it can be picked up by CI and properly verified.

### Visual Studio online task tracker

As this project has been mainly contributed by group of students learning .NET web development majority of the functional features is kept in Visual Studio Online. As it provides better workflow for student group working on the project every day.

Having said that OSS contributions is better managed by Bitbucket cloud issue tracker. Thus this is the recommended place to log new issues.

At some point in the future we will have single consolidated backlog.

If you want to see issues stored in VS Online please contact Sergii Diachenko(mailto:sdiachenko AT outlook DOT com) for access.

## Development Environment

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

## Styleguides

### Git Commit Messages

* Use the present tense ("Add feature" not "Added feature")
* Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
* Limit the first line to 72 characters or less
* Reference issues and pull requests liberally after the first line
* Consider starting the commit message with an applicable emoji:
  * :art: `:art:` when improving the format/structure of the code
  * :racehorse: `:racehorse:` when improving performance
  * :bug: `:bug:` when fixing a bug
  * :fire: `:fire:` when removing code or files
  * :green_heart: `:green_heart:` when fixing the CI build
  * :white_check_mark: `:white_check_mark:` when adding tests
  * :arrow_up: `:arrow_up:` when upgrading dependencies
  * :arrow_down: `:arrow_down:` when downgrading dependencies
  * :shirt: `:shirt:` when removing linter warnings

### C#

C# code style is controlled by StyleCop Code Analyzer. You will see errors and warnings if you break code style.

If you feel that some particular rule is too strict or too lose. Also if you have ideas how it can be enhanced please log __enhancement__ issue or contact Sergii Diachenko(mailto:sdiachenko AT outlook DOT com)

### Unit Tests

We put a lot of emphasis on writing useful unit tests which will communicate requirements, ease maintainability and serve as a safety net.

See more details at [Unit Testing Conventions](../../wiki/Unit-Testing-Conventions)

## Additional references

Some of the deatils we try to keep up to date at [Development Process](../../wiki/Development-Process) page