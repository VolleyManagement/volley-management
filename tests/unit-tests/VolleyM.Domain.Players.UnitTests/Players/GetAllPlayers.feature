Feature: Get All players
	All existing players should be listed

@unit @azurecloud @ab:1021
Scenario: Query all players
	Given several players exist
		| TenantId  | Version           | Id      | FirstName | LastName |
		| <default> | <initial-version> | player1 | Ivan      | Ivanov   |
		| <default> | <initial-version> | player2 | John      | Doe      |
		| <default> | <initial-version> | player3 | Marco     | Polo     |
	When I query all players
	Then all players are returned
		| Tenant    | Version        | Id      | FirstName | LastName |
		| <default> | <some-version> | player1 | Ivan      | Ivanov   |
		| <default> | <some-version> | player2 | John      | Doe      |
		| <default> | <some-version> | player3 | Marco     | Polo     |