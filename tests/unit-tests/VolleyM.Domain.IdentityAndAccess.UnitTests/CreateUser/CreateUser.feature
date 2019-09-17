Feature: Create User
User will be enrolled into a system by Auth0
And Auth0 will store most of authentication data
But we still need a minimal feature to have users referenced in authorization policies

Scenario: Create new user
	Given UserId provided
    And Tenant provided
    And user does not exist
	When I execute CreateUser
	Then user is created

Scenario: Create existing user
    Given UserId provided
    And Tenant provided
    And such user already exists
    When I execute CreateUser
    Then conflict error is returned
