Feature: Authorization Role
	Basic role class to manage group of permissions

@unit @ab:1026
Scenario: Add permission to a Role
	Given Sample role
	When I add PermissionA to it
	Then role has PermissionA

@unit @ab:1026
Scenario: Check not assigned permission
	Given Sample role
	When I add PermissionA to it
	Then role does not have PermissionB

@unit @ab:1026
Scenario: Add several permissions to a Role
	Given Sample role
	When I add PermissionA to it
	When I add PermissionB to it
	Then role has PermissionA
	Then role has PermissionB

@unit @ab:1026
Scenario: Add same permissions to a Role
	Given Sample role
	When I add PermissionA to it
	When I add PermissionA to it
	Then role has PermissionA