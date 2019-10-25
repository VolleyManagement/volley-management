Feature: GetAllPlayers

	All contributors should be recognized and returned

@unit
Scenario: Query all players
	Given several players exist
	When I query all players
	Then all players received
