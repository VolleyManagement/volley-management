Feature: Edit Player
	In order to keep player information up to date
	As a tournament administrator
	I want to be able to change player properties

Scenario: Edit existing player
    Given John Smith player exists
    And first name changed to Jack
    When I execute EditPlayer
    Then player is saved with new name

Scenario: Change to very long name
    Given John Smith player exists
    And first name changed to Looong name which should be more than 60 symbols
    When I execute EditPlayer
    Then ArgumentException is thrown

Scenario: Edit missing player
    Given Ivan Ivanov player does not exist
    And first name changed to Jack
    When I execute EditPlayer
    Then MissingEntityException is thrown
