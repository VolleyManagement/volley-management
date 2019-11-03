Feature: Authorization Decorator
	In order to authorize access to Request Handlers
	we want a decorator which will authorize user against handler

@ab:1026
Scenario: User has permission
	Given current user has permission to execute this handler
	When I call decorated handler
	Then success result is returned

@ab:1026
Scenario: No permission
	Given current user has no permission to execute this handler
	When I call decorated handler
	Then NotAuthorized error should be returned with message No permission