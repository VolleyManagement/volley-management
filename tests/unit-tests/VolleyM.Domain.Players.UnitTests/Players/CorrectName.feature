Feature: Correct Player Name
	In order to correct mistakes in the name or react to name changes
	I want VolleyM system to alow changing players name

@azurecloud
Scenario: Correct Name
	Given player exists
		| PlayerId | FirstName | LastName |
		| player1  | Marko     | Ivanov   |
	And I have CorrectNameRequest
		| PlayerId | FirstName | LastName |
		| player1  | Jane      | Doe      |
	When I execute CorrectName
	Then success result is returned
	And player name is
		| FirstName | LastName |
		| Jane      | Doe      |