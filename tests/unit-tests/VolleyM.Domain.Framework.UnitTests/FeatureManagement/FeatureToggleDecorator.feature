Feature: Feature Toggle Decorator
	Decorator that validates state of the feature toggle for particular handler
	If Toggle is turned off handler is not executed and FeatureDisabled error is returned
	
@unit @ab:996
Scenario: Feature Toggle turned on
	Given I have a handler
	And feature toggle exists
	And feature toggle is enabled
	When I call decorated handler
	Then handler result should be returned

@unit @ab:996
Scenario: Feature Toggle turned off
	Given I have a handler
	And feature toggle exists
	And feature toggle is disabled
	When I call decorated handler
	Then feature disabled error should be returned