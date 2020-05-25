Feature: Players API

	Background:
		Given url vmAppUrl

	Scenario: Create Player
		Given path 'api/players'
		And header Authorization = auth_header
		And request {'firstName': 'Ivan', 'lastName': 'Petrov'}
		And method POST
		Then status 200
		And match response == {tenant: '#string', 'id':'#string', 'firstName': 'Ivan', 'lastName': 'Petrov'}

	Scenario: Create another player
		Given path 'api/players'
		And header Authorization = auth_header
		And request {'firstName': 'John', 'lastName': 'Smith'}
		And method POST
		Then status 200

	Scenario: List players
		Given path 'api/players'
		And header Authorization = auth_header
		And method GET
		Then status 200
		And match response == '#[2]'
		And match response contains {tenant: '#string', 'id':'#string', 'firstName': 'Ivan', 'lastName': 'Petrov'}
		And match response contains {tenant: '#string', 'id':'#string', 'firstName': 'John', 'lastName': 'Smith'}