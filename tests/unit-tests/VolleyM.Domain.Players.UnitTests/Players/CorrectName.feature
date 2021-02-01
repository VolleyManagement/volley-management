Feature: Correct Player Name
	In order to correct mistakes in the name or react to name changes
	I want VolleyM system to alow changing players name

@azurecloud @api:512
Scenario: Name corrected
	Given player exists
		| Id                     | Version  | FirstName | LastName |
		| player-to-correct-name | version1 | Marko     | Ivanov   |
	And I have CorrectNameRequest
		| PlayerId               | EntityVersion | FirstName | LastName |
		| player-to-correct-name | version1      | Jane      | Doe      |
	When I execute CorrectName
	Then success result is returned
	And player name is
		| FirstName | LastName |
		| Jane      | Doe      |

@azurecloud @api:512
Scenario: PlayerNameCorrected event
	Given player exists
		| Id                  | Version  | FirstName | LastName |
		| correct-name-evt-id | version1 | Marko     | Ivanov   |
	And I have CorrectNameRequest
		| PlayerId            | EntityVersion | FirstName | LastName |
		| correct-name-evt-id | version1      | Jane      | Doe      |
	When I execute CorrectName
	Then PlayerNameCorrected event is produced
		| TenantId  | PlayerId            | Version  | FirstName | LastName |
		| <default> | correct-name-evt-id | version2 | Jane      | Doe      |
	And PlayerNameCorrected event is Public

@azurecloud @api:512
Scenario: Validation Cases
	Given player exists
		| Id                         | Version  | FirstName | LastName |
		| correct-name-validation-id | version1 | Marko     | Ivanov   |
	And I have CorrectNameRequest
		| PlayerId                   | EntityVersion | FirstName   | LastName   |
		| correct-name-validation-id | version1      | <FirstName> | <LastName> |
	When I execute CorrectName
	Then player is not changed
	And ValidationError is returned
	And PlayerNameCorrected event is not produced

	# make sure we have at least one test for e2e flow
	@azurecloud
	Examples:
		| FirstName          | LastName           |
		| <60+ symbols name> | Smith              |

	# cover rest of the cases with faster unit tests
	@unit
	Examples:
		| FirstName          | LastName           |
		| <60+ symbols name> | Smith              |
		| <null>             | Smith              |
		|                    | Smith              |
		| John               | <60+ symbols name> |
		| John               | <null>             |
		| John               |                    |