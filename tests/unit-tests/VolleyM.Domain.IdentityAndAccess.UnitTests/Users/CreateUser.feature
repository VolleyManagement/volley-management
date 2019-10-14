Feature: Create User
User will be enrolled into a system by Auth0
And Auth0 will store most of authentication data
But we still need a minimal feature to have users referenced in authorization policies

@unit @azurecloud @ab:1026
Scenario: Create new user
	Given UserId provided
    And Tenant provided
	And Role provided
    And user does not exist
	When I execute CreateUser
	Then user is created
	And user is returned

@unit @azurecloud @ab:1026
Scenario: Create existing user
    Given UserId provided
    And Tenant provided
    And such user already exists
    When I execute CreateUser
    Then Conflict error is returned

@unit @azurecloud @ab:1026
Scenario: Create new without Role
	Given UserId provided
    And Tenant provided
    And user does not exist
	When I execute CreateUser
	Then user is created
	And user is returned