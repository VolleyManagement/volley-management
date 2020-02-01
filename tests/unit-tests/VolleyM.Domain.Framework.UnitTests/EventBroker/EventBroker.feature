Feature: EventBroker

	In order to provide asynchronous communication between componnets
	as a dev I want to have EventBroker component

@ab:1099 
Scenario: Deliver event to listener
	Given I have EventA listener
	When I publish this event
	Then event is delivered to the listener
