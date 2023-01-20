Feature: Role

Scenario: No access to create role
	Given I am logged in as user without permissions
	When role created
	Then forbidden result

Scenario: Create new role
	Given I am logged in as admin
	And permission created
	When role with this permission created
	Then success result
	And role in authorization service

Scenario: No access to delete role
	Given I am logged in as user without permissions
	When role deleted
	Then forbidden result

Scenario: Delete role
	Given I am logged in as admin
	And permission created
	And role with this permission created
	And role in authorization service
	When role deleted
	Then success result
	And role not in authorization service
	And permission in authorization service