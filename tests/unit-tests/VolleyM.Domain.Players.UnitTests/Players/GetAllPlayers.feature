Feature: Get All players
	All existing players should be listed

@unit @azurecloud @ab:1021
Scenario: Query all players
	Given several players exist
		| TenantId  | PlayerId | FisrstName | LastName |
		| <default> | player1  | Ivan       | Ivanov   |
		| <default> | player2  | John       | Doe      |
		| <default> | player3  | Marco      | Polo     |
	When I query all players
	Then all players are returned
		| Tenant  | Id | FisrstName | LastName |
		| <default> | player1  | Ivan       | Ivanov   |
		| <default> | player2  | John       | Doe      |
		| <default> | player3  | Marco      | Polo     |