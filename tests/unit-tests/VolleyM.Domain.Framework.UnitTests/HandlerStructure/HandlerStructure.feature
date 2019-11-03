Feature: Handler Structure validation

	As a developer I want a system to guide me towards approach used in this project
	Each user action is translated into a handler call, each handler has corresponding IRequest,
	and potentially IResponse class. Also Handler might have some additional definitions like Validators.
	Approach that we use in VolleyManagement is that we define parent class and name it as an action, e.g. ApplyForTournament.
	It will have nested classes for Request, Handler, Validator, etc.
	This set of tests validates that subsystems which rely on this structure communicate proper errors to the developers.

@unit @ab:996
Scenario: Handler implements multiple IRequestHandler<,> interfaces
	Given I have a handler with several IRequestHandler<,> interfaces 
	When I call decorated handler
	Then DesignViolation error should be returned with message 'Handler is allowed to implement only one IRequestHandler'

@unit @ab:996
Scenario: Handler is not nested
	Given I have a handler that is not nested in a class
	When I call decorated handler
	Then DesignViolation error should be returned with message 'Handler should be nested in a class to group handler related classes together'

@unit @ab:996
Scenario: Sample Handler
	Given I have an example handler
	When I call decorated handler
	Then handler result should be returned

# ToDo: test cached instance