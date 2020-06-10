﻿Feature: Check Access
	Validates user permission against given operation

@ab:1026
Scenario: User is authorized when user role has permission
	Given user has RoleA role assigned
	And RoleA has Permission1
	When I check access to Permission1
	Then access is granted

@ab:1026
Scenario: User is not authorized when user role has no permission
	Given user has RoleA role assigned
	And RoleA has Permission1
	When I check access to Permission2
	Then access is denied

@ab:1026
Scenario: User is not authorized when exception occured
	Given role storage returns error
	When I check access to Permission1
	Then access is denied

@ab:1026
Scenario Outline: AuthZ Handler role authorization
	Given user has authz.handler role assigned
	When I check access to permission from '<permissionContext>' for '<permissionAction>'
	Then access is granted

	Examples: predefined AuthZ handler permissions
		| permissionContext | permissionAction |
		| IdentityAndAccess | GetUserOld       |
		| IdentityAndAccess | CreateUserOld    |

@ab:1026
Scenario: Permission check is case insensitive
	Given user has RoleB role assigned
	And RoleB has LowerCasePermission3
	When I check access to UpperCasePermission3
	Then access is granted

@ab:1026
Scenario: User does not have any role assigned
	Given user has no role
	When I check access to Permission1
	Then access is denied