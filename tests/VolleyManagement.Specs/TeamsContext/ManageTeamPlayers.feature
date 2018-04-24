Feature: Manage Team Players
	In order to manage team roster
	As a tournament administrator
	I want to be able to add and remove players from the team

Scenario: Add player to team
	Given Team A exists
	And I have added Jane Doe as a team player
	When I execute AddPlayersToTeam
	Then players are added

Scenario: Add several players to team
	Given Team A exists
    And Ivan Ivanov is a team player
	And I have added Jane Doe as a team player
	And I have added John Smith as a team player
	When I execute AddPlayersToTeam
	Then players are added

Scenario: Remove player from team
	Given Team A exists
    And Jane Doe is a team player
	And I have removed Jane Doe
	When I execute RemovePlayersFromTeam
	Then players are removed

Scenario: Remove several player from team
	Given Team A exists
    And Jane Doe is a team player
    And John Smith is a team player
    And Ivan Ivanov is a team player
	And I have added Jane Doe as a team player
	And I have added John Smith as a team player
	When I execute RemovePlayersFromTeam
	Then players are removed
    
Scenario: Remove captain from team
	Given Team A exists
    And Jane Doe is a team captain
	And I have removed Jane Doe
	When I execute RemovePlayersFromTeam
	Then InvalidOperationException is thrown
