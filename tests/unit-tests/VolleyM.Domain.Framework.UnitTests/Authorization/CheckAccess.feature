Feature: Check Access
	Validates user permission against given operation

@ab:1026
Scenario: User is authorized when user role has permission
	Given user has RoleA assigned
	And RoleA has Permission1
	When I check access to Permission1
	Then access is granted

@ab:1026
Scenario: User is not authorized when user role has no permission
	Given user has RoleA assigned
	And RoleA has Permission1
	When I check access to Permission2
	Then access is denied

@ab:1026
Scenario: User is not authorized when exception occured
	Given role storage returns error
	When I check access to Permission1
	Then access is denied

@ab:1026
Scenario: User does not have any role assigned
	Given user has no role
	When I check access to Permission1
	Then access is denied