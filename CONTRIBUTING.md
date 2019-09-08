# Contributing to Volley Management

First off, thanks for taking the time to contribute!

The following is a set of guidelines for contributing to Volley Management. These are mostly guidelines, not rules. Use your best judgment, and feel free to propose changes to this document in a pull request.

🏗 UNDER CONSTRUCTION 🏗

More content to be added :timer_clock::timer_clock::timer_clock:

## Code of Conduct

This project and everyone participating in it is governed by the [Code of Conduct](/CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to Sergii Diachenko(mailto:sdiachenko AT outlook DOT com).

## How to reach team and/or ask question?

First of all check out [Wiki](https://github.com/VolleyManagement/volley-management/wiki) to get some basic information.

We have Slack chat [volleymanagement.slack.com](https://volleymanagement.slack.com/). As the project is relatively small all questions can be asked in `#general` channel

As for now to get access to this chat please write email to Sergii Diachenko(mailto:sdiachenko AT outlook DOT com). In future we might get this automated.

## How Can I Contribute?

### Reporting Bugs

Log an issue in the [issue tracker](https://github.com/VolleyManagement/volley-management/issues).

Before creating bug reports, please check existing open issues as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible.

### Suggesting Improvements

Log issue in the [issue tracker](https://github.com/VolleyManagement/volley-management/issues).

Before creating suggestions reports, please check existing open issues as you might find out that you don't need to create one. Please include as many details as possible including but not limited to reasoning, design decisions, potential impact on existing code.

### Branch naming

Project has a CI configured to work with following branch naming pattern:

**ab#{issue number}_short_issue_description_with_underscores**

_Note: 'ab' stands for Azure Boards and used to link issue to the corresponding work item._

### Pull Requests

If you want to directly contribute to the project you are highly welcome!

You can do so by creating pull request into `master` branch. After review your request will either be merged or set of questions/suggestions will be given to you to address before accepting PR.

As part of PR approval process project uses CI checks. Please following branch naming so it can be picked up by CI and properly verified.

### Issue tracker

Check out integrated issue tracker to see what team is working on. We have a couple of project where we track work for active collaborators.

## Development Environment

Minimum software requirements:

* .NET Core 3
* VS 2019 any edition
* Azure Storage Emulator

All dependencies are managed via NuGet so you don't need to take any additional steps to acquire dependencies.

Project uses integration with several external systems:

* Auth0
* Gmail and/or SendGrid

In order to test/debug/use features using those integrations you might need to create appropriate profiles at following providers and use secrets provided in order to run system.

Check out [Development Environment Setup](../../wiki/Dev-Env-Guide) page for detailed instructions.

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

C# code style is controlled by SonarLint Code Analyzer. You will see errors and warnings if you break code style.

If you feel that some particular rule is too strict or too lose. Also if you have ideas how it can be enhanced please log issue or contact Sergii Diachenko(mailto:sdiachenko AT outlook DOT com)

### Angular

This projects follows official [Angular Style Guide](https://angular.io/guide/styleguide)

### Unit Tests

We put a lot of emphasis on writing useful unit tests which will communicate requirements, ease maintainability and serve as a safety net.

See more details at [Unit Testing Conventions](../../wiki/Unit-Testing-Conventions)

## Additional references

See development environment setup guide at [Development Environment Setup](../../wiki/Dev-Env-Guide) page.

Some of the details we try to keep up to date at [Development Process](../../wiki/Development-Process) page
