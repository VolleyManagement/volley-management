Feature: Event Bus
	In order to communicate asynchronously between components
	EventBus will provide a mechanism to subscribe to events

@unit @ab:1099
Scenario: Receive event notification
	Given I have event subscriber for EventA
	When EventA is fired
	Then subscriber is invoked
