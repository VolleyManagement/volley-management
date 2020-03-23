Feature: EventBroker
	In order to be able to communicate assynchronously
	As a developer I need event publisher
	Which will deliver events to registered event handlers

@ab:1099
Scenario: Published event to single handler
	Given I have single event handler for EventA
	When I publish EventA
	Then handler result should be returned
	And handler should receive event