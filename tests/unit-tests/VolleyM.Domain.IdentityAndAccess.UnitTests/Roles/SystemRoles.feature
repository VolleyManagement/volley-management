Feature: System Roles
	Documents all permission assignments for roles

@azurecloud
Scenario: Visitor
	Given I have Visitor role
	When I request role from the store
	Then role should be found
	And I have following permissions
		| Context      | Action |
		| contributors | getall |

@azurecloud
Scenario: SysAdmin
	Given I have SysAdmin role
	When I request role from the store
	Then role should be found
	And I have following permissions
		| Context      | Action |
		| contributors | getall |
		| players      | create |