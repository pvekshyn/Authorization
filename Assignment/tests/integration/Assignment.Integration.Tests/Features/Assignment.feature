Feature: Assignment

Scenario: Assign user to not existing role validation
	Given role not exist
	When user assigned to this role
	Then validation error

Scenario: Assign user success
	Given role created
	When user assigned to this role
	Then success result
	And outbox message in database

Scenario: Assign already assigned user idempotent
	Given role created
	And user assigned to this role
	When user assigned to this role
	Then idempotent result

Scenario: Deassign not assigned user idempotent
	Given user not assigned to role
	When user deassigned from this role
	Then idempotent result

Scenario: Deassign user success
	Given role created
	And user assigned to this role
	When user deassigned from this role
	Then success result
	And outbox message in database
