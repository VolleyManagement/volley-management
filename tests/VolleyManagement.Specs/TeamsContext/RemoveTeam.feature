Feature: Remove Team
	In order to keep data storage
	As a tournament administrator
	I want to be able to delete teams

@ignore
Scenario: Remove existing team
    Given Volley.org.ua team exists
    When I execute DeleteTeam
    Then team is removed

@ignore
Scenario: Remove missing team
    Given football.org.ua team does not exist
    When I execute DeleteTeam
    Then ConcurrencyException is thrown
