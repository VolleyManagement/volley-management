﻿Feature: GetAllPlayers

	All contributors should be recognized and returned

@unit @ab:1072
Scenario: Query all players
	Given several players exist
	When I query all players
	Then all players received
