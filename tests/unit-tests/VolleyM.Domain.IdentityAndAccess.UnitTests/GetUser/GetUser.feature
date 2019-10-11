Feature: Get User by ID
	Retrieve user by ID

@unit @ab:1026
Scenario: User exist
	Given user exists
	When I get user
	Then user is returned
