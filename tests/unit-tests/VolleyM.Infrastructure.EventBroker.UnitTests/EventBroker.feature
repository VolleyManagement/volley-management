Feature: EventBroker
	In order to be able to communicate assynchronously
	As a developer I need event publisher
	Which will deliver events to registered event handlers

@ab:1099
Scenario Outline: Publish event
	Given I have <HandlerCount> <HandlerType> event handlers for <Event>
	When I publish <Event>
	Then handler result should be returned
	And handler should receive event

	Examples:
		| Event                         | HandlerCount | HandlerType |
		| SingleSubscriberEvent         | 1            | Internal    |
		| NoSubscribersEvent            | 0            | Internal    |
		| SeveralSubscribersEvent       | 2            | Internal    |
		| PublicSingleSubscriberEvent   | 1            | Public      |
		| PublicNoSubscribersEvent      | 0            | Public      |
		| PublicSeveralSubscribersEvent | 2            | Public      |

@ab:1099
Scenario: Event published twice
	Given I have 1 internal event handler for SingleSubscriberEvent
	And SingleSubscriberEvent was published once
	When I publish SingleSubscriberEvent
	Then handler result should be returned
	And handler should receive all events
# 1. Design violation if cannot find EventType
# 2. Event published to both internal and public
# 3. Public consumer can skip properties