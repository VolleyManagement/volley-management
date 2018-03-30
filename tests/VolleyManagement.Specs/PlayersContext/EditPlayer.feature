Feature: Edit Player
	In order to keep player information up to date
	As a tournament administrator
	I want to be able to change player properties

@ignore
Scenario: Edit existing player
    Given John Smith player exists
    And first name changed to Jack
    When I execute EditPlayer
    Then player is saved with new name

@ignore
Scenario: Change to very long name
    Given John Smith player exists
    And first name changed to Very looooooooooooooooooooooooong name which should be more than 60 symbols
    When I execute EditPlayer
    Then EntityInvariantViolationException is thrown

@ignore
Scenario: Edit missing player
    Given Ivan Ivanov player does not exist
    And first name changed to Jack
    When I execute EditPlayer
    Then ConcurrencyException is thrown
