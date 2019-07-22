Feature: Create Player
	As a tournament administrator
	I want to be able to create players
    So they can be assigned to teams

Scenario: Create very long name
Given first name set to Very looong name which should be more than 60 symbols
When I execute CreatePlayer
Then InvalidEntityException is thrown

Scenario Outline: Create simple player
	Given first name is <FirstName>
    And last name is <LastName>
	When I execute CreatePlayer
    Then new player gets new Id
	Then new player should be succesfully added
Examples: 
| FirstName | LastName  |
| Marcuss   | Nilsson   |
| Ivan      | Ivanov    |


Scenario Outline: Create player with all attributes
	Given first name is <FirstName>
    And last name is <LastName>
    And height is <Height>
    And weight is <Weight>
    And year of birth is <BirthYear>
	When I execute CreatePlayer
    Then new player gets new Id
	Then new player should be succesfully added
Examples: 
| FirstName | LastName | Height | Weight | BirthYear |
| John      | Smith    | 190    | 85     | 1985      |
| Jane      | Doe      | 180    | 55     | 1995      |

Scenario: Quick create players from names
    Given set full names from Table
    | FullName                  |
    | John Smith                |
    | Peter Petrovich Petrov    |
    When I execute QuickCreatePlayer
    Then players is created from Table with FirstName and LastName
    | FirstName             | LastName          |
    | John                  | Smith             |
    | Peter Petrovich       | Petrov            |

Scenario: Bulk create players
    Given collection of players to create
    When I execute CreatePlayerBulk
    Then all players are created
