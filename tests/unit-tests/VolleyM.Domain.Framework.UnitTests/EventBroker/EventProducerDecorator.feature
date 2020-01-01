Feature: Domain Event dispatching
	Wires events produced by the request handler into event bus

@ab:1099
Scenario: Handler produced event
	Given I have a handler which can produce events
	And handler produces event
	When I call decorated handler
	Then event is published to event broker

@ab:1099
Scenario: Handler produced no events
	Given I have a handler which can produce events
	And handler does not produce event
	When I call decorated handler
	Then nothing is published to event broker

@ab:1099
Scenario: Handler produced several events
	Given I have a handler which can produce events
	And handler produced several events
	When I call decorated handler
	Then all events are published to event broker

@ab:1099
Scenario: Handler returns value
	Given I have a handler which can produce events
	When I call decorated handler
	Then handler result should be returned


	#no error when no interface
	#shows design error if domain events are not initialized
