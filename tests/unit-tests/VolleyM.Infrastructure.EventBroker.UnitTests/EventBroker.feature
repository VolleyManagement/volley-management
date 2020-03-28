Feature: EventBroker
	In order to be able to communicate assynchronously
	As a developer I need event publisher
	Which will deliver events to registered event handlers

@ab:1099
Scenario Outline: Published event to single handler
	Given I have <HandlerCount> event handlers for <Event>
	When I publish <Event>
	Then handler result should be returned
	And handler should receive event

	Examples:
		| Event                   | HandlerCount |
		| SingleSubscriberEvent   | 1            |
		| NoSubscribersEvent      | 0            |
		| SeveralSubscribersEvent | 2            |

@ab:1099
Scenario: Event published twice
	Given I have 1 event handler for SingleSubscriberEvent
	And SingleSubscriberEvent was published once
	When I publish SingleSubscriberEvent
	Then handler result should be returned
	And handler should receive all events