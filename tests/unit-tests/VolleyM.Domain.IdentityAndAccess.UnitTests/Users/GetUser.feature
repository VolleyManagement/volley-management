Feature: Get User by ID
	Retrieve user by ID

@unit @azurecloud @ab:1026
Scenario: User exist
	Given user exists
	When I get user
	Then user is returned

@unit @azurecloud @ab:1026
Scenario: User does not exist
	Given user does not exist
	When I get user
	Then NotFound error is returned