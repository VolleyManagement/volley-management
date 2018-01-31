# README #

Welcome to Volley Management project repository

## Current Build Status

[![Build status](https://ci.appveyor.com/api/projects/status/fmfdph327qywmeju/branch/master?svg=true)](https://ci.appveyor.com/project/sdiachen/volleymanagement/branch/master)

## What is this repository for? ##

The goal of the project is to create system to manage Volleyball sport events for amateur leagues

Current Version: Pre-release

## How do I get set up? ##

### Summary of set up ###

Project is VS 2017 solution. Any edition will suffice.

Target framework is .NET 4.7.

### Dependencies ###

All dependencies are managed via NuGet

### Database configuration ###

Database is automatically deployed to server first time you run application. Specify proper connection string in Web.config file in VolleyManagement.UI project.

By default it is configured to use SQL Server 2016 LocalDB. But you can can change altering connection string.

### How to run tests ###

Project uses MSTest framework. Tests can be triggered via Visual Studio test menu.

🚧 ToDo: Add guide how to run Code Coverage using JetBrains dotCover CLI runner

### Deployment instructions ###

To deploy it locally you need to get AppSettingSecret.config file from Sergii Diachenko(mailto:sdiachenko AT outlook DOT com).
This file should be put one level above sln file. (This prevents from accidental checkin)

## Additional references ##

* [License](/LICENSE.md)
* [Contribution Guidelines](/CONTRIBUTING.md)
* [Code of Conduct](/CODE_OF_CONDUCT.md)

## Who do I talk to? ##

* All questions should be directed to Sergii Diachenko(mailto:sdiachenko AT outlook DOT com)