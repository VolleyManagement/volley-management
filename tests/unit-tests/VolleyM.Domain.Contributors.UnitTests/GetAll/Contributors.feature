Feature: Contributors

All contributors should be recognized and returned

Scenario: Query all contributors
	Given several contributors exist
	When I query all contributors
	Then all contributors received

