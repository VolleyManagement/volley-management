Feature: EventBroker
	In order to be able to communicate assynchronously
	As a developer I need event publisher
	Which will deliver events to registered event handlers

@ab:1099
Scenario Outline: Published event to single handler
	Given I have event handler for <Event>
	When I publish <Event>
	Then handler result should be returned
	And handler should receive event

	Examples:
		| Event                   |
		| SingleSubscriberEvent   |
		| NoSubscribersEvent      |
		| SeveralSubscribersEvent |

@ab:1099
Scenario: Event published twice
	Given I have single event handler for SingleSubscriberEvent
	And SingleSubscriberEvent was published once
	When I publish SingleSubscriberEvent
	Then handler result should be returned
	And handler should receive all events