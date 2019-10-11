Feature: Authorize User
	Check user permissions and assign appropriate roles

Scenario Outline: Enroll new user
	Given new user is being authorized
	And user has '<idClaimType>' claim with '<claimValue>' value
	When I authorize user
	Then user should be authorized
	And new user should be created in the system

	Examples: different claims that should be used as User Id
		| idClaimType                                                          | claimValue                               |
		| sub                                                                  | google\|123                              |
		| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | A4tbWQojYmGgmxiFcA21KeoFbnG4c5Ex@clients |
		| sub                                                                  | google-oauth2\|101770239932937079313     |

Scenario: New user does not have supported Id Claim
	Given new user is being authorized
	And user has no claims
	When I authorize user
	Then user should not be authorized
	And new user should not be created in the system

Scenario: Existing user authorizes
	Given existing user is being authorized
	When I authorize user
	Then user should be authorized

Scenario: Error when retrieving existing user
	Given existing user is being authorized
	And get user operation has error
	When I authorize user
	Then user should not be authorized

Scenario: Error when retrieving creating user
	Given new user is being authorized
	And user has correct ID claim
	And create user operation has error
	When I authorize user
	Then user should not be authorized

Scenario: New user is set as current user
	Given new user is being authorized
	And user has correct ID claim
	When I authorize user
	Then this user is set into current context

Scenario: Existing user is set as current user
	Given existing user is being authorized
	When I authorize user
	Then this user is set into current context
	