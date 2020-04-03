Feature: Create Player
	In order to manage players in the tournament
	As a captain
	I want to create player

@ab:1022 @azurecloud
Scenario: Create player
	Given I have CreatePlayerRequest
		| Tenant  | Id      | FirstName | LastName |
		| tenantA | player1 | John      | Smith    |
	When I execute CreatePlayer
	Then player is created
	And player is returned