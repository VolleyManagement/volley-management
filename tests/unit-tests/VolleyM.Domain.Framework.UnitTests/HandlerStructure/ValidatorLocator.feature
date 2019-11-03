Feature: Validator locator

	See HandlerStructure.feature for more context.
	Validator locator is used for conditional registration of validation decorator

@unit @ab:1103
Scenario: Handler is not nested
	Given I have a handler that is not nested in a class
	When has validator check is performed
	Then false is returned
