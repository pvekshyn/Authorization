Feature: Assign

Scenario: User got access when assigned
	Given I am logged in as admin
	And permission created
	And role with this permission created
	When user assigned to this role
	Then user got access

Scenario: User lost access when deassigned
	Given I am logged in as admin
	And permission created
	And role with this permission created
	And user assigned to this role
	And user got access
	When user deassigned from this role
	Then user lost access

Scenario: User lost access when role deleted
	Given I am logged in as admin
	And permission created
	And role with this permission created
	And user assigned to this role
	And user got access
	When role deleted
	Then user lost access
