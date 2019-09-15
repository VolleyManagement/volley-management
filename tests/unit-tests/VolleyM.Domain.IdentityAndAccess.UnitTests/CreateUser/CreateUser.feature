Feature: Create User

User will be enrolled into a system by Auth0
And Auth0 will store most of authentication data
But we still need a minimal feature to have users referenced in authorization policies

Scenario: Create user
	Given UserId provided
    And Tenant provided
	When I execute CreateUser
	Then user is created
