Feature: Edit Team
	In order to keep team information up to date
	As a tournament administrator
	I want to be able to change team properties

Scenario: Edit existing team
    Given Team A team exists
    And name changed to A-Team
    When I execute EditTeam
    Then team is updated succesfully

Scenario: Change name to very long name
    Given Team A team exists
    And name changed to Very Looong team name which should be more than 30 symbols
    When I execute EditTeam
    Then EntityInvariantViolationException is thrown
    
Scenario: Edit missing team
    Given Team B team does not exist
    And name changed to B-Team
    When I execute EditTeam
    Then MissingEntityException is thrown

Scenario: Change captain
    Given Team A team exists
    And captain is changed to Captain B
    When I execute ChangeTeamCaptain
    Then team is updated succesfully
