Feature: Create Player
	As a tournament administrator
	I want to be able to create players
    So they can be assigned to teams

Scenario Outline: Create simple player
	Given first name is <FirstName>
    And last name is <LastName>
	When I execute CreatePlayer
	Then new player should be succesfully added
    Then new player gets new Id
Examples: 
| FirstName | LastName  |
| Marcuss   | Nilsson   |
| Ivan      | Ivanov    |
