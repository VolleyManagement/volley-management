Feature: Create Player
	In order to manage players in the tournament
	As a captain
	I want to create player

@ab:1022 @unit @azurecloud
Scenario: Create player
	Given I have CreateRequest
		| FirstName | LastName |
		| John      | Smith    |
	When I execute Create
	Then player is created
	And player is returned
	And PlayerCreated event is produced
		| TenantId  | PlayerId | FirstName | LastName |
		| <default> | player1  | John      | Smith    |
	And PlayerCreated event is Public

@ab:1022 @unit
Scenario: Validation cases
	Given I have CreateRequest
		| FirstName   | LastName   |
		| <FirstName> | <LastName> |
	When I execute Create
	Then player is not created
	And ValidationError is returned
	And PlayerCreated event is not produced

	Examples:
		| FirstName          | LastName           |
		| <60+ symbols name> | Smith              |
		| <null>             | Smith              |
		|                    | Smith              |
		| John               | <60+ symbols name> |
		| John               | <null>             |
		| John               |                    |