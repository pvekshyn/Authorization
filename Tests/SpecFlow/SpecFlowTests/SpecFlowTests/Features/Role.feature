Feature: Role

Scenario: Create new role
	Given permission created
	When role with this permission created
	Then role in authorization service

Scenario: Delete role
	Given permission created
	And role with this permission created
	And role in authorization service
	When role deleted
	Then role not in authorization service
	And permission in authorization service