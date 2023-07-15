Feature: Create Role

Scenario: Create role validation
	Given Permission exist
	And Role name empty
	When Role created
	Then Validation error
	And Role not in database

Scenario: Create role success
	Given Permission exist
	And Role valid 
	When Role created
	Then Success result
	And Role in database

Scenario: Concurrent create same role twice
	Given Permission exist
	And Two roles with same id and name
	When Roles created
	Then One success result
	And One idempotent result

Scenario: Concurrent create roles with same name twice
	Given Permission exist
	And Two roles with same name
	When Roles created
	Then One success result
	And One validation error