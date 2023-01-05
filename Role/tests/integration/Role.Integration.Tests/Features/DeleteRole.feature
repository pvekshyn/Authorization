Feature: Delete Role

Scenario: Delete not existing role idempotent
	Given Role not exist
	When Role deleted
	Then Idempotent result

Scenario: Delete existing role success
	Given Permission exist
	And Role with this permission exist
	When Role deleted
	Then Success result
	And Role not in database
	And Outbox message in database
