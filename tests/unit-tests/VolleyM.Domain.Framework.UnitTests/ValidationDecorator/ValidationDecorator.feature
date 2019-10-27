Feature: Validation Decorator

	As a developer I want a sub-system that will automatically hookup up validation logic 
	based on presense of some interface, e.g. IValidator<TRequest>
	so Validators can be added by creating validator implementation

# Validation success
# Validation failed
# Several validators

@unit @ab:1103
Scenario: No validator exists
	Given I have a handler
	And validator is not defined
	When I call decorated handler
	Then handler result should be returned

@unit @ab:1103
Scenario: Validator success
	Given I have a handler
	And single validator defined
	And validator passes
	When I call decorated handler
	Then handler result should be returned
