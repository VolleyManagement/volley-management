Feature: Remove Player
	In order to keep data storage
	As a tournament administrator
	I want to be able to delete players

@ignore
Scenario: Remove existing player
    Given John Smith player exists
    When I execute DeletePlayer
    Then player is removed

@ignore
Scenario: Remove missing player
    Given Ivan Ivanov player does not exist
    When I execute DeletePlayer
    Then ConcurrencyException is thrown
