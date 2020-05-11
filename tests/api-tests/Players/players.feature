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