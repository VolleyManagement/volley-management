Feature: Get All players

	All existing players should be listed

@unit @azurecloud @ab:1021 
Scenario: Query all players
	Given several players exist
	When I query all players
	Then all players received
