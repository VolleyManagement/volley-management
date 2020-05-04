Feature: Authorize User
	Check user permissions and assign appropriate roles

@ab:1026
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

@ab:1026
Scenario: New user does not have supported Id Claim
	Given new user is being authorized
	And user has no claims
	When I authorize user
	Then user should not be authorized
	And new user should not be created in the system

@ab:1026
Scenario: Existing user authorizes
	Given existing user is being authorized
	When I authorize user
	Then user should be authorized

@ab:1026
Scenario: Error when retrieving existing user
	Given existing user is being authorized
	And get user operation has error
	When I authorize user
	Then user should not be authorized

@ab:1026
Scenario: Error when retrieving creating user
	Given new user is being authorized
	And user has correct ID claim
	And create user operation has error
	When I authorize user
	Then user should not be authorized

@ab:1026
Scenario: New user is set as current user
	Given new user is being authorized
	And user has correct ID claim
	When I authorize user
	Then this user is set into current context

@ab:1026
Scenario: Existing user is set as current user
	Given existing user is being authorized
	When I authorize user
	Then this user is set into current context

@ab:1026
Scenario: Not authenticated user
	Given unauthenticated user is being authorized
	When I authorize user
	Then user should be authorized
	And anonymous visitor set as current user
	And new user should not be created in the system

@ab:1026
Scenario Outline: Authorized user id matches system Id
	Given new user is being authorized
	And user has 'sub' claim with '<userId>' value
	When I authorize user
	Then user should not be authorized
	And new user should not be created in the system

	Examples: restricted users
		| userId                 |
		| anonym@volleym.idp     |
		| authz.user@volleym.idp |

@ab:1128
Scenario: API application authorized directly
	# We look at the case when Auth0 API has been authorized using ClientId
	# intention here is to give those users Admin rights to run API tests on non-prod envs
	Given new user is being authorized
	# API client is authorizing
	And user has 'sub' claim with 'clientIdString@clients' value
	And user has 'azp' claim with 'clientIdString' value
	And user has 'gty' claim with 'client-credentials' value
	And hosting environment is not Production
	And Auth0 client id is 'clientIdString'
	When I authorize user
	Then user should be authorized
	And user is assigned SysAdmin role