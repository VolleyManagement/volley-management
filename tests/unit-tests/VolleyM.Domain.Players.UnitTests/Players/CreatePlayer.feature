Feature: Create Player
	In order to manage players in the tournament
	As a captain
	I want to create player

@ab:1022 @unit @azurecloud
Scenario: Create player
	Given I have CreatePlayerRequest
		| Tenant  | FirstName | LastName |
		| tenantA | John      | Smith    |
	When I execute CreatePlayer
	Then player is created
	And player is returned

@ab:1022 @unit
Scenario: Validation cases
	Given I have CreatePlayerRequest
		| Tenant   | FirstName   | LastName   |
		| <Tenant> | <FirstName> | <LastName> |
	When I execute CreatePlayer
	Then player is not created
	And ValidationError is returned

	Examples:
		| Tenant  | FirstName          | LastName           |
		| tenantA | <60+ symbols name> | Smith              |
		| tenantA | <null>             | Smith              |
		| tenantA |                    | Smith              |
		| tenantA | John               | <60+ symbols name> |
		| tenantA | John               | <null>             |
		| tenantA | John               |                    |
		| <null>  | John               | Smith              |