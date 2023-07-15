Feature: Delete Permission

Scenario: Delete not existing permission idempotent
	Given Permission not exist
	When Permission deleted
	Then Idempotent result

Scenario: Delete existing permission success
	Given Permission exist
	When Permission deleted
	Then Success result
	And Permission not in database
