Feature: Authorization Decorator
	In order to authorize access to Request Handlers
	we want a decorator which will authorize user against handler

@ab:1026
Scenario: Handler class have permission attribute
	Given I have handler with permission attribute
	And current user has permission to execute this handler
	When I call decorated handler
	Then success result is returned

@ab:1026
Scenario: Handler class does not have Permission attribute
	Given I have no permission attribute on a handler
	When I call decorated handler
	Then NotAuthorized error should be returned with message Handler does not have single permission attribute

@ab:1026
Scenario: Handler has attribute but user has no permission
	Given I have handler with permission attribute
	And current user has no permission to execute this handler
	When I call decorated handler
	Then NotAuthorized error should be returned with message No permission