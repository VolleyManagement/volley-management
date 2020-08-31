Feature: Correct Player Name
	In order to correct mistakes in the name or react to name changes
	I want VolleyM system to alow changing players name

@azurecloud @api:512
Scenario: Name corrected
	Given player exists
		| PlayerId               | FirstName | LastName |
		| player-to-correct-name | Marko     | Ivanov   |
	And I have CorrectNameRequest
		| PlayerId               | FirstName | LastName |
		| player-to-correct-name | Jane      | Doe      |
	When I execute CorrectName
	Then success result is returned
	And player name is
		| FirstName | LastName |
		| Jane      | Doe      |

@azurecloud @api:512
Scenario: PlayerNameCorrected event
	Given player exists
		| PlayerId            | FirstName | LastName |
		| correct-name-evt-id | Marko     | Ivanov   |
	And I have CorrectNameRequest
		| PlayerId            | FirstName | LastName |
		| correct-name-evt-id | Jane      | Doe      |
	When I execute CorrectName
	Then PlayerNameCorrected event is produced
		| TenantId  | PlayerId            | FirstName | LastName |
		| <default> | correct-name-evt-id | Jane      | Doe      |
	And PlayerNameCorrected event is Public