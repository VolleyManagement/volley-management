Feature: EventBroker
	In order to be able to communicate assynchronously
	As a developer I need event publisher
	Which will deliver events to registered event handlers

@ab:1099
Scenario: Published event to single handler
	Given I have single event handler for SingleSubscriberEvent
	When I publish SingleSubscriberEvent
	Then handler result should be returned
	And handler should receive event

Scenario: Event published twice
	Given I have single event handler for SingleSubscriberEvent
	And SingleSubscriberEvent was published once
	When I publish SingleSubscriberEvent
	Then handler result should be returned
	And handler should receive all events

	Scenario: No event handlers
	Given I have no event handlers for NoSubscribersEvent
	When I publish NoSubscribersEvent
	Then handler result should be returned
	And handler should receive all events

