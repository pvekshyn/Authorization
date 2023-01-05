Feature: Update Role Permissions

Scenario: Update role permissions validation
	Given New permission exist
	And Update payload with not existing role
	When Role permissions updated
	Then Validation error

Scenario:  Update role permissions success
	Given Permission exist
	And New permission exist
	And Role with this permission exist
	And Update payload valid 
	When Role permissions updated
	Then Success result
	And Role permissions changed in database
	And Outbox message in database
