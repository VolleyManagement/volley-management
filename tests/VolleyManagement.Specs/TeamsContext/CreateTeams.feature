﻿Feature: CreateTeams
	In order to keep track of teams in tournament
	As a tournament administrator
	I want to be able to create teams

@ignore
Scenario Outline: Create team
	Given team name is <TeamName>
    And captain is <Captain>
    And coach is <Coach>
    And achievements are <Achievements>
	When I execute CreateTeam
    Then new team gets new Id
	Then new team should be succesfully added
Examples: 
| TeamName  | Captain    | Coach       | Achievements                  |
| Team A    | Jane Doe   |             |                               |
| Full Team | John Smith | Ivan Ivanov | Winner of Galactic tournament |

@ignore
Scenario: Create team without captain
    Given team name is Team B
    And captain is empty
    When I execute CreateTeam
    Then EntityInvariantViolationException is thrown